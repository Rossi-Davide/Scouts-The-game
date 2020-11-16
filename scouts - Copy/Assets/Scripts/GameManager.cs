using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManager : MonoBehaviour
{
	[HideInInspector]
	public InGameObject[] InGameObjects { get; private set; }

	public int pointsValue, materialsValue, energyValue;
	public int energyMaxValue = 100, materialsMaxValue = 300, pointsMaxValue = 70;
	public GameObject buttonCanvas;

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
	public event System.Action OnPlayerDeath;
	public event System.Action OnCampEnd;
	public event System.Action OnCampStart;
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

	private void PlayerDied()
	{
		OnPlayerDeath?.Invoke();
	}
	private void CampEnded()
	{
		OnCampEnd?.Invoke();
	}
	public void CampStarted()
	{
		OnCampStart?.Invoke();
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
	public static void DestroyItems(ObjectBase b)
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


	public static IEnumerator Wait(float time, System.Action onEnd)
	{
		yield return new WaitForSeconds(time);
		onEnd();
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

	public GameObject[] decorations;
	public Vector2[] startAreaSpawn, endAreaSpawn;
	void SpawnDecorations()
	{
		foreach (var d in decorations)
		{
			for (int spawned = 0; spawned < Random.Range(5, 8); spawned++)
			{
				int currentArea = Random.Range(0, startAreaSpawn.Length);
				float posX = Random.Range(startAreaSpawn[currentArea].x, endAreaSpawn[currentArea].x);
				float posY = Random.Range(startAreaSpawn[currentArea].y, endAreaSpawn[currentArea].y);
				GameObject decoration = Instantiate(d, new Vector3(posX, posY, 0), Quaternion.identity, wpCanvas.transform);
				decoration.GetComponent<Plant>().wpCanvas = wpCanvas;
			}
		}
	}


	#endregion
	#region DayNightCycle & Rain
	const float minuteDuration = 0.1f; //a minute actually lasts 0.1 seconds
	[HideInInspector]
	public int currentMinute, currentHour, currentDay;
	[HideInInspector]
	public int totalDays = 14;
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
		if (currentDay > totalDays)
		{
			CampEnded();
		}
		isDay = !(currentHour > 20 || currentHour < 7);
		if (!isDay) { StartCoroutine(ChangeLight()); };
	}

	[HideInInspector]
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
	IEnumerator ChangeLight()
	{
		if (isDay)
		{
			while (globalLight.intensity > .6f)
			{
				globalLight.intensity -= .01f;
				yield return new WaitForSeconds(.08f);
			}
		}
		else
		{
			while (globalLight.intensity < 1)
			{
				globalLight.intensity += .01f;
				yield return new WaitForSeconds(.08f);
			}
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
	public bool isRaining;
	[HideInInspector]
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
	SaveSystem saveSystem;
	void Start()
	{
		OnCampStart += WhenCampStarts;
		globalLight = transform.Find("MainLights/GlobalLight").GetComponent<Light2D>();
		OnInGameoObjectsChange += RefreshInGameObjs;
		OnCounterValueChange += CheckPlayerDeath;
		SpawnDecorations();
		InvokeRepeating(nameof(SpawnDecorations), 30, Random.Range(45, 75));
		InvokeRepeating(nameof(PeriodicItemActionSlow), 60, 60);
		InvokeRepeating(nameof(PeriodicItemActionMedium), 30, 30);
		InvokeRepeating(nameof(PeriodicItemActionFast), 15, 15);
		InvokeRepeating(nameof(CheckRain), 1, 1);
		InvokeRepeating(nameof(RefreshWaitToUseObjects), 1, 1);
		InvokeRepeating(nameof(IncreaseTime), minuteDuration, minuteDuration);
	}

	public Status GetStatus()
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
			currentDay = currentDay
		};
	}
	public void SetStatus(Status status)
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
	}







	void WhenCampStarts()
	{
		currentDay = 1;
		currentHour = 7;
	}

	void RefreshInGameObjs()
	{
		StartCoroutine(RefreshInGameObjsCoroutine());
	}
	IEnumerator RefreshInGameObjsCoroutine()
	{
		InGameObjects = FindObjectsOfType<InGameObject>();
		yield return new WaitForEndOfFrame();
		ObjectArrayUpdated();
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
		foreach (var o in InGameObjects)
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