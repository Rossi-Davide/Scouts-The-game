using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.Universal;



public class GameManager : MonoBehaviour
{
	[HideInInspector]
	[System.NonSerialized]
	public InGameObject[] inGameObjects;

	public int pointsValue, materialsValue, energyValue;
	public int energyMaxValue, materialsMaxValue, pointsMaxValue;
	public GameObject buttonCanvas;
	public VolumeProfile mainSceneProf;
	ColorCurves night;
	ColorAdjustments night2;
	#region Singleton
	public static GameManager instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("GameManager singleton has been created more than once!");
		}
		instance = this;
	}
	#endregion
	#region Events
	public event System.Action<Counter, int> OnCounterValueChange;
	public event System.Action<bool> OnSunsetOrSunrise;
	public event System.Action<int> OnHourChange;
	public event System.Action<PlayerAction> OnActionDo;
	public event System.Action<ObjectBase> OnInventoryChange;
	public event System.Action OnInGameoObjectsChange;
	public event System.Action<ObjectBase> OnBuild;
	public event System.Action OnObjectArrayUpdate;
	public event System.Action<Counter, int> OnCounterMaxValueChange;

	public void Built(ObjectBase obj)
	{
		OnBuild?.Invoke(obj);
	}
	public void BuildingChanged()
	{
		OnInGameoObjectsChange?.Invoke();
	}
	public void ObjectArrayUpdated()
	{
		OnObjectArrayUpdate?.Invoke();
	}
	public void DayEndedOrStarted(bool d)
	{
		OnSunsetOrSunrise?.Invoke(d);
	}
	public void HourChanged(int h)
	{
		OnHourChange?.Invoke(h);
	}

	public void ActionDone(PlayerAction a)
	{
		OnActionDo?.Invoke(a);
	}
	public void InventoryChanged(ObjectBase obj)
	{
		OnInventoryChange?.Invoke(obj);
	}

	public void ChangeCounter(Counter counter, int delta)
	{
		switch (counter)
		{
			case Counter.None:
				break;
			case Counter.Materiali:
				materialsValue += delta;
				break;
			case Counter.Energia:
				energyValue += delta;
				break;
			case Counter.Punti:
				pointsValue += delta;
				break;
			default: throw new System.NotSupportedException("Il counter richesto non esiste!");
		}
		OnCounterValueChange?.Invoke(counter, delta);
	}
	public void CounterMaxValueChanged(Counter counter, int delta)
	{
		switch (counter)
		{
			case Counter.None:
				break;
			case Counter.Materiali:
				materialsMaxValue += delta;
				break;
			case Counter.Energia:
				energyMaxValue += delta;
				break;
			case Counter.Punti:
				pointsMaxValue += delta;
				break;
			default: throw new System.NotSupportedException("Il counter richesto non esiste!");
		}
		OnCounterMaxValueChange?.Invoke(counter, delta);
	}

	#endregion
	#region UtilityFunctions
	public static bool DoIfPercentage(float percentage)
	{
		return Random.Range(1f, 101f) <= percentage;
	}
	private static int CheckRange(int value, int min, int max)
	{
		return Mathf.Min(Mathf.Max(min, value), max);
	}
	public TextMeshProUGUI warning, message;
	List<Coroutine> currentWarningOrMessageCoroutines = new List<Coroutine>();
	public void WarningOrMessage(string text, bool isWarning)
	{
		warning.text = text;
		message.text = text;
		foreach (var i in currentWarningOrMessageCoroutines)
		{
			StopCoroutine(i);
		}
		currentWarningOrMessageCoroutines.Add(isWarning ? StartCoroutine(Warning()) : StartCoroutine(Message()));
	}
	public void CleanWarningOrMessage()
	{
		warning.gameObject.SetActive(false);
		message.gameObject.SetActive(false);
	}
	IEnumerator Warning()
	{
		warning.gameObject.SetActive(true);
		yield return new WaitForSeconds(3f);
		warning.gameObject.SetActive(false);
	}
	IEnumerator Message()
	{
		message.gameObject.SetActive(true);
		yield return new WaitForSeconds(3f);
		message.gameObject.SetActive(false);
	}

	public static string ChangeToFriendlyString(string text)
	{
		if (string.IsNullOrEmpty(text)) { return text; }

		var newString = new System.Text.StringBuilder();

		char first = text[0];
		if (first != ' ' && first != '_')
			newString.Append(char.ToUpper(first));

		for (int i = 1; i < text.Length; i++)
		{
			char ch = text[i];

			if (char.IsUpper(ch) || char.IsDigit(ch) && !char.IsDigit(newString[newString.Length - 1]))
				newString.Append(' ');

			else if (char.IsLower(ch) && newString[newString.Length - 1] == ' ' || char.IsDigit(newString[newString.Length - 1]))
				ch = char.ToUpper(ch);

			else if (ch == '_')
				ch = ' ';

			newString.Append(ch);
		}
		return newString.ToString();
	}

	public int GetCounterValue(Counter counter)
	{
		switch (counter)
		{
			case Counter.Materiali: return materialsValue;
			case Counter.Energia: return energyValue;
			case Counter.Punti: return pointsValue;
			default: throw new System.NotImplementedException("counter non valido");
		}
	}

	public static string IntToMinuteSeconds(int time)
	{
		string st = "";
		int other = time % 3600;
		int hours = (time - other) / 3600;
		int seconds = other % 60;
		int minutes = (other - seconds) / 60;
		if (hours > 0)
			st = hours + "h ";
		if (minutes > 0)
			st += minutes + "m ";
		st += seconds + "s";
		return st;
	}

	public static int FindMax(int[] nums)
	{
		int max = 0;
		foreach (int i in nums)
		{
			if (i > max)
				max = i;
		}
		return max;
	}

	public static bool HasItemsToBuy(ObjectBase b)
	{
		bool canBuild = true;
		if ((b.exists && b.itemsNeededs.Length > b.level + 1) || (!b.exists && b.itemsNeededs.Length > b.level))
		{
			foreach (var i in b.itemsNeededs[b.exists ? b.level + 1 : b.level].items)
			{
				canBuild = i.item.currentAmount >= i.amount;
			}
		}
		return canBuild;
	}
	public static void DestroyItemsNeededToBuyItem(ObjectBase b)
	{
		if (b.itemsNeededs.Length > b.level)
		{
			foreach (var i in b.itemsNeededs[b.level].items)
			{
				if (i.isDestroyed)
				{
					i.item.currentAmount -= i.amount;
				}
			}
		}
	}


	public IEnumerator Wait(float time, System.Action onEnd)
	{
		yield return new WaitForSeconds(time);
		onEnd();
	}
	public IEnumerator Wait(float time, IEnumerator onEnd)
	{
		yield return new WaitForSeconds(time);
		StartCoroutine(nameof(onEnd));
	}



	#endregion
	#region Abilities
	void PeriodicItemActionSlow() => PeriodicItemAction(PeriodicActionInterval.Slow);
	void PeriodicItemActionMedium() => PeriodicItemAction(PeriodicActionInterval.Medium);
	void PeriodicItemActionFast() => PeriodicItemAction(PeriodicActionInterval.Fast);
	void PeriodicItemAction(PeriodicActionInterval interval)
	{
		foreach (var o in Shop.instance.itemDatabase)
		{
			CheckPeriodicUse(o, interval);
		}
		foreach (var o in Shop.instance.buildingDatabase)
		{
			CheckPeriodicUse(o, interval);
		}
	}
	void CheckPeriodicUse(ObjectBase o, PeriodicActionInterval interval)
	{
		if (o.usingAmount)
		{
			if (o.currentAmount > 0 && o.periodicUses.Length > 0)
			{
				UseItem(o, interval);
			}
		}
		else if (o.periodicUses.Length > 0)
		{
			UseItem(o, interval);
		}
	}

	void UseItem(ObjectBase o, PeriodicActionInterval interval)
	{
		if (o != null && o.periodicUses[o.level].interval == interval && o.currentAmount >= 1)
		{
			for (int y = 0; y < o.currentAmount; y++) { o.DoAction(); };
		}
	}
	#endregion
	#region Spawn stuff
	public GameObject[] actionButtons;
	public TextMeshProUGUI buttonsText;
	public GameObject wpCanvas;
	public GameObject healthBarPrefab, loadingBarPrefab, nameTextPrefab, subNameTextPrefab;

	public Plant[] plantPrefabs;
	public PlantSpawnArea[] spawnAreas;
	public Plant[] spawnedPlants;
	void SpawnDecorations(int? toSpawn)
	{
		if (toSpawn == null) { toSpawn = Random.Range(35, 50); }
		for (int spawned = 0; spawned < toSpawn; spawned++)
		{
			int currentArea = Random.Range(0, spawnAreas.Length);
			float posX = Random.Range(spawnAreas[currentArea].start.x, spawnAreas[currentArea].end.x);
			float posY = Random.Range(spawnAreas[currentArea].start.y, spawnAreas[currentArea].end.y);
			Plant decoration = Instantiate(plantPrefabs[Random.Range(0, plantPrefabs.Length)], new Vector3(posX, posY, 0), Quaternion.identity, wpCanvas.transform);
			decoration.wpCanvas = wpCanvas;
		}
	}

	[System.Serializable]
	public class PlantSpawnArea
	{
		public Vector2 start;
		public Vector2 end;
	}


	#endregion
	#region DayNightCycle & Rain
	const float minuteDuration = 0.1f; //a minute actually lasts 0.1 seconds
	[HideInInspector]
	[System.NonSerialized]
	public int currentMinute, currentHour, currentDay;
	void IncreaseTime()
	{
		currentMinute++;
		if (currentMinute >= 60)
		{
			currentHour++;
			currentMinute = 0;
			HourChanged(currentHour);
		}
		if (currentHour >= 24)
		{
			currentDay++;
			currentHour = 0;
		}
		CheckTimeConditions();
		RefreshCounterText();
	}

	public void SkipNight()
	{
		if (currentHour < 7 || currentHour > 20)
		{
			currentHour = 7;
			currentMinute = 0;
		}
	}

	void CheckTimeConditions()
	{
		if (currentDay > 14)
		{
			CampEnded();
		}
		isDay = !(currentHour > 20 || currentHour < 7);
		ChangeLight();
	}

	[HideInInspector]
	[System.NonSerialized]
	public bool isDay;
	private bool hasOpenedCounter = false;
	public GameObject closeDayCounter, openDayCounter;
	public void ToggleDayCounter()
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

		hasOpenedCounter = !hasOpenedCounter;
		closeDayCounter.SetActive(!hasOpenedCounter);
		openDayCounter.SetActive(hasOpenedCounter);
		RefreshCounterText();
	}
	private Light2D globalLight;
	
	void ChangeLight()
	{
		if (globalLight.intensity <= .7f)
		{
			mainSceneProf.TryGet<ColorCurves>(out night);
			night.active = true;
			mainSceneProf.TryGet<ColorAdjustments>(out night2);
			night2.active = false;
		}

		if (!isDay)
		{
			
			if (globalLight.intensity > .6f)
				globalLight.intensity -= .01f;
		}
		else if (isDay && globalLight.intensity < 1)
		{
			mainSceneProf.TryGet<ColorCurves>(out night);
			night.active = false;
			mainSceneProf.TryGet<ColorAdjustments>(out night2);
			night2.active = true;
			globalLight.intensity += .01f;
		}
	}
	void RefreshCounterText()
	{
		closeDayCounter.SetActive(!hasOpenedCounter);
		openDayCounter.SetActive(hasOpenedCounter);
		if (closeDayCounter.activeSelf) { closeDayCounter.GetComponent<Animator>().Play(isDay ? "Day" : "Night"); }
		if (openDayCounter.activeSelf) { openDayCounter.GetComponent<Animator>().Play(isDay ? "OpenDay" : "OpenNight"); }
		string s = "Giorno " + currentDay + ", " + (currentHour >= 10 ? currentHour.ToString() : "0" + currentHour) + ":" + (currentMinute >= 10 ? currentMinute.ToString() : "0" + currentMinute);
		openDayCounter.GetComponentInChildren<TextMeshProUGUI>().text = s;
		closeDayCounter.GetComponentInChildren<TextMeshProUGUI>().text = currentDay.ToString();
	}

	[HideInInspector]
	[System.NonSerialized]
	public bool isRaining;
	[HideInInspector]
	[System.NonSerialized]
	public int rainingTimeLeft, rainingWaitTimeLeft;
	void CheckRain()
	{
		if (!isRaining)
		{
			if (rainingTimeLeft > 0)
			{
				rainingWaitTimeLeft--;
			}
			else if (DoIfPercentage(0.6f))
			{
				isRaining = true;
				transform.Find("ParticleManager/pioggia").gameObject.SetActive(true);
				rainingTimeLeft = Random.Range(30, 75);
				rainingWaitTimeLeft = Random.Range(25, 40);
			}
		}
		else
		{
			rainingTimeLeft--;
			if (rainingTimeLeft <= 0)
			{
				isRaining = false;
				transform.Find("ParticleManager/pioggia").gameObject.SetActive(false);
			}
		}
	}
	#endregion
	#region General

	void Start()
	{
		globalLight = transform.Find("MainLights/GlobalLight").GetComponent<Light2D>();
		OnInGameoObjectsChange += RefreshInGameObjs;
		OnCounterValueChange += CheckPlayerDeath;

		InvokeRepeating(nameof(SpawnDecorations), 55f, 55);
		InvokeRepeating(nameof(PeriodicItemActionSlow), 60, 60);
		InvokeRepeating(nameof(PeriodicItemActionMedium), 30, 30);
		InvokeRepeating(nameof(PeriodicItemActionFast), 15, 15);
		InvokeRepeating(nameof(CheckRain), 1, 1);
		InvokeRepeating(nameof(RefreshWaitToUseObjects), 1, 1);
		InvokeRepeating(nameof(IncreaseTime), minuteDuration, minuteDuration);

		currentDay = 1;
		currentHour = 7;
		energyMaxValue = 100;
		materialsMaxValue = 500;
		pointsMaxValue = 70;
		energyValue = 100;
		globalLight.intensity = 1f;
		SetStatus(SaveSystem.instance.LoadData<Status>(SaveSystem.instance.gameManagerFileName, false));
	}


	public GameObject victoryPanel, deathPanel, overlay;
	private void CampEnded()
	{
		Time.timeScale = 0;

		var manager = SquadrigliaManager.instance;

		var arr = (ConcreteSquadriglia[])manager.squadriglieInGioco.Clone();
		System.Array.Sort(arr, new SquadrigliaComparer());
		System.Array.Reverse(arr);

		var positions = victoryPanel.transform.Find("Texts/Positions").GetComponentsInChildren<TextMeshProUGUI>();
		for (int i = 0; i < positions.Length; i++)
		{
			var sq = arr[i];
			positions[i].text = $"#{i + 1}: {sq.baseSq.name} " + (sq.baseSq == Player.instance.squadriglia ? "(Tu) " : "") + $"con {sq.points} punti";
		}

		victoryPanel.SetActive(true);
		overlay.SetActive(true);
		PanZoom.instance.canDo = false;
		Joystick.instance.enabled = false;
	}
	class SquadrigliaComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((ConcreteSquadriglia)x).points, ((ConcreteSquadriglia)y).points);
		}
	}

	void PlayerDied()
	{
		Time.timeScale = 0;
		deathPanel.SetActive(true);
		overlay.SetActive(true);
	}
	public void MenuAndDestroyCamp()
	{
		SceneLoader.instance.LoadMainMenuScene();
		SaveSystem.instance.DeleteGameFiles();
		CampManager.instance.campCreated = false;
	}

	public Status SendStatus()
	{
		return new Status
		{
			energyValue = energyValue,
			materialsValue = materialsValue,
			pointsValue = pointsValue,
			energyMaxValue = energyMaxValue,
			materialsMaxValue = materialsMaxValue,
			pointsMaxValue = pointsMaxValue,
			isRaining = isRaining,
			rainingTimeLeft = rainingTimeLeft,
			rainingWaitTimeLeft = rainingWaitTimeLeft,
			currentMinute = currentMinute,
			currentHour = currentHour,
			currentDay = currentDay,
			globalLight = globalLight.intensity,
			totalPlantsSpawned = spawnedPlants.Length,
		};
	}
	public void SetStatus(Status status)
	{
		if (status != null)
		{
			energyValue = status.energyValue;
			materialsValue = status.materialsValue;
			pointsValue = status.pointsValue;
			energyMaxValue = status.energyMaxValue;
			materialsMaxValue = status.materialsMaxValue;
			pointsMaxValue = status.pointsMaxValue;
			isRaining = status.isRaining;
			rainingTimeLeft = status.rainingTimeLeft;
			rainingWaitTimeLeft = status.rainingWaitTimeLeft;
			currentMinute = status.currentMinute;
			currentHour = status.currentHour;
			currentDay = status.currentDay;
			globalLight.intensity = status.globalLight;
			SpawnDecorations(status.totalPlantsSpawned);
		}
		else
			SpawnDecorations(null);
	}
	public class Status
	{
		public int energyValue;
		public int materialsValue;
		public int pointsValue;
		public int energyMaxValue;
		public int materialsMaxValue;
		public int pointsMaxValue;
		public bool isRaining;
		public int rainingTimeLeft;
		public int rainingWaitTimeLeft;
		public int currentMinute;
		public int currentHour;
		public int currentDay;
		public float globalLight;
		public int totalPlantsSpawned;
	}

	void RefreshInGameObjs()
	{
		inGameObjects = FindObjectsOfType<InGameObject>();
		ObjectArrayUpdated();
		spawnedPlants = FindObjectsOfType<Plant>();
	}
	void CheckPlayerDeath(Counter c, int delta)
	{
		if (c == Counter.Energia && energyValue <= 0)
			PlayerDied();
	}
	#endregion
	#region Objects

	void RefreshWaitToUseObjects()
	{
		foreach (var o in inGameObjects)
		{
			foreach (var b in o.buttons)
			{
				if (b.isWaiting)
				{
					o.CountDownTime(b);
				}
			}
		}
	}
	#endregion
}

public enum Counter
{
	None,
	Materiali,
	Energia,
	Punti
}

public enum Ruolo
{
	Novizio,
	Terzino,
	Vice,
	Capo,
}


public enum GameColor
{
	Red,
	Yellow,
	Green,
	Orange,
	Brown,
	Gray,
	Black,
	White,
	Pink,
	Purple,
	Blue,
	LightBlue,
	LightGray,
}
public enum PeriodicActionInterval
{
	None,
	Once,
	Slow,
	Medium,
	Fast,
}
public enum SpecificShopScreen
{
	Pioneristica,
	Cucina,
	Infermieristica,
	Topografia,
	Espressione,
	NegozioIllegale,
	Costruzioni,
	Decorazioni,
}
public enum MainShopScreen
{
	Costruzioni,
	Item,
}