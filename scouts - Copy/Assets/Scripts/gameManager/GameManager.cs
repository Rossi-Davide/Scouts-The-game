using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManager : MonoBehaviour
{
	public int pointsValue, materialsValue, energyValue = 100;
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
		DontDestroyOnLoad(this);
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
	#endregion
	#region Events
	public event System.Action<int> OnEnergyChange;
	public event System.Action<int> OnMaterialsChange;
	public event System.Action<int> OnPointsChange;
	public event System.Action OnPlayerDeath;
	public event System.Action OnCampEnd;
	public event System.Action OnCampStart;
	public event System.Action OnDayEndOrStart;
	public event System.Action OnRain;
	public event System.Action<PlayerAction> OnActionDo;
	public event System.Action OnInventoryChange;


	public void ActionDone(PlayerAction a)
	{
		OnActionDo?.Invoke(a);
	} 
	public void InventoryChanged()
	{
		OnInventoryChange?.Invoke();
	}

	private void EnergyChanged(int newValue)
	{
		OnEnergyChange?.Invoke(newValue);
	}
	private void MaterialsChanged(int newValue)
	{
		OnMaterialsChange?.Invoke(newValue);
	}
	private void PointsChanged(int newValue)
	{
		OnPointsChange?.Invoke(newValue);
	}
	private void PlayerIsDead()
	{
		OnPlayerDeath?.Invoke();
	}
	private void CampEnded()
	{
		OnCampEnd?.Invoke();
	}

	void CampStarted()
	{
		OnCampStart?.Invoke();
	}



	public void ChangeCounter(Counter counter, int delta)
	{
		switch (counter)
		{
			case Counter.Materiali:
				materialsValue = CheckRange(materialsValue + delta, 0, 1000);
				MaterialsChanged(materialsValue);
				break;
			case Counter.Energia:
				energyValue = CheckRange(energyValue + delta, 0, 100);
				EnergyChanged(energyValue);
				break;
			case Counter.Punti:
				pointsValue = CheckRange(pointsValue + delta, 0, 100);
				PointsChanged(pointsValue);
				break;
			default:
				throw new System.Exception("Counter non valido");
		}
	}
	#endregion
	#region UtilityFunctions
	private static int CheckRange(int value, int min, int max)
	{
		return Mathf.Min(Mathf.Max(min, value), max);
	}
	public TextMeshProUGUI warning;
	List<Coroutine> currentWarningCoroutines = new List<Coroutine>();
	public void WarningMessage(string text)
	{
		warning.text = text;
		foreach (var i in currentWarningCoroutines)
		{
			StopCoroutine(i);
		}
		currentWarningCoroutines.Add(StartCoroutine(Warning()));
	}
	IEnumerator Warning()
	{
		warning.gameObject.SetActive(true);
		yield return new WaitForSeconds(3f);
		warning.gameObject.SetActive(false);
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

	public int CheckCounterValue(Counter counter)
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


	public bool CanDoAction(PlayerAction a)
	{
		bool canDoAction = false;
		foreach (Item i in a.neededItems)
		{
			canDoAction = i.currentAmount >= 1;
		}
		return canDoAction;
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
		foreach (InventorySlot i in InventoryManager.instance.slots)
		{
			for (int y = 0; y < i.amount; y++)
				UseItem(i, interval);
		}
		foreach (InventorySlot i in ChestManager.instance.slots)
		{
			for (int y = 0; y < i.amount; y++)
				UseItem(i, interval);
		}
	}
	void UseItem(InventorySlot i, PeriodicActionInterval interval)
	{
		if (i.item != null && i.item.periodicUseInterval == interval && i.item.currentAmount >= 1)
		{
			for (int y = 0; y < i.item.currentAmount; y++) { i.item.DoAction(); };
		}
	}
	public enum PeriodicActionInterval
	{
		None,
		Once,
		Slow,
		Medium,
		Fast,
	}
	public enum Counter
	{
		None,
		Materiali,
		Energia,
		Punti
	}
	public enum ShopScreen
	{
		None,
		Pioneristica,
		Cucina,
		Infermieristica,
		Topografia,
		Espressione,
		NegozioIllegale,
	}
	#endregion
	#region Spawn stuff
	public GameObject[] actionButtons;
	public TextMeshProUGUI buttonsText;
	public Canvas wpCanvas;

	public GameObject[] decorations;
	[HideInInspector]
	public List<GameObject> spawnedDecorations = new List<GameObject>();
	public Vector2[] startAreaSpawn, endAreaSpawn;
	private int toSpawnPerType;
	void SpawnDecorations()
	{
		if (spawnedDecorations.Count <= 10)
		{
			foreach (var d in decorations)
			{
				for (int spawned = 0; spawned < toSpawnPerType; spawned++)
				{
					int currentArea = Random.Range(0, startAreaSpawn.Length);
					float posX = Random.Range(startAreaSpawn[currentArea].x, endAreaSpawn[currentArea].x);
					float posY = Random.Range(startAreaSpawn[currentArea].y, endAreaSpawn[currentArea].y);
					GameObject decoration = Instantiate(d, new Vector3(posX, posY, 0), Quaternion.identity, wpCanvas.transform);
					decoration.GetComponent<Decorations>().wpCanvas = wpCanvas;
					spawnedDecorations.Add(decoration);
				}
			}
		}
	}


	#endregion
	#region DayNightCycle & Rain
	private bool coroutineStarted;
	[HideInInspector]
	public bool hasSkippedNight, isDay = true;
	private bool hasOpenedCounter = false;
	private int secondsPast = 0, totalHourDuration = 3, currentHour = 7, totalDays = 14, currentDay = 1;
	public GameObject closeDayCounter, openDayCounter;
	public void ToggleDayCounter()
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

		hasOpenedCounter = !hasOpenedCounter;
		closeDayCounter.SetActive(!hasOpenedCounter);
		openDayCounter.SetActive(hasOpenedCounter);
		RefreshCounterText();

	}

	IEnumerator StartCounter()
	{
		yield return new WaitForSeconds(totalHourDuration - secondsPast);
		currentHour += 1;
		if (currentHour == 24)
		{
			isDay = false;
			currentHour = 0;
		}
		else if (currentHour == 7)
		{
			isDay = true;
		}
		if (isDay)
		{
			StartCoroutine(DayCounter());
		}
		else
		{
			StartCoroutine(NightCounter());
		}
	}
	private Light2D globalLight;
	IEnumerator DayCounter()
	{
		for (int t = currentHour; t < 24; t++)
		{
			for (int i = 0; i < totalHourDuration; i++)
			{
				yield return new WaitForSeconds(1f);
				secondsPast++;
			}
			if (currentHour == 21)
			{
				StartCoroutine(ChangeLight());
				isDay = false;
				OnDayEndOrStart?.Invoke();
			}
			currentHour++;
			RefreshCounterText();
		}
		currentHour = 0;
		currentDay++;
		RefreshCounterText();
		StartCoroutine(NightCounter());
	}
	IEnumerator NightCounter()
	{
		while (!hasSkippedNight && currentHour < 7)
		{
			for (int i = 0; i < totalHourDuration; i++)
			{
				if (!hasSkippedNight)
				{
					yield return new WaitForSeconds(1f);
					secondsPast++;
				}
				else
				{
					break;
				}
			}
			if (currentHour == 5)
			{
				StartCoroutine(ChangeLight());
				isDay = true;
				OnDayEndOrStart?.Invoke();
			}
			currentHour++;
			RefreshCounterText();
		}
		currentHour = 7;
		isDay = true;
		StartCoroutine(ChangeLight());
		OnDayEndOrStart?.Invoke();
		RefreshCounterText();
		StartCoroutine(DayCounter());
	}
	IEnumerator ChangeLight()
	{
		if (globalLight.intensity > .7f)
		{
			while (globalLight.intensity > .6f)
			{
				globalLight.intensity -= .01f;
				yield return new WaitForSeconds(.08f);
			}
		}
		else if (globalLight.intensity < .7f)
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
		if (hasOpenedCounter)
		{
			if (isDay)
			{
				openDayCounter.GetComponent<Animator>().Play("OpenDay");
			}
			else
			{
				openDayCounter.GetComponent<Animator>().Play("OpenNight");
			}
			openDayCounter.GetComponentInChildren<TextMeshProUGUI>().text = "Giorno " + currentDay + ", " + currentHour + ":00";
		}
		else
		{
			if (isDay)
			{
				closeDayCounter.GetComponent<Animator>().Play("Day");
			}
			else
			{
				closeDayCounter.GetComponent<Animator>().Play("Night");
			}
			closeDayCounter.GetComponentInChildren<TextMeshProUGUI>().text = currentDay.ToString();
		}
	}
	public bool isRaining;
	IEnumerator Rain()
	{
		isRaining = true;
		OnRain?.Invoke();
		transform.Find("ParticleManager/pioggia").gameObject.SetActive(true);
		yield return new WaitForSeconds(Random.Range(20, 80));
		transform.Find("ParticleManager/pioggia").gameObject.SetActive(false);
		isRaining = false;
		OnRain?.Invoke();
	}
	private int rainProbability = 50;
	void GenerateRainProbability()
	{
		int randomValue = Random.Range(0, 100);
		if (randomValue <= rainProbability && !isRaining)
		{
			StartCoroutine(Rain());
		}
	}
	#endregion
	#region General
	void Start()
	{
		isDay = true;
		globalLight = transform.Find("MainLights/GlobalLight").GetComponent<Light2D>();
		toSpawnPerType = Random.Range(5, 8);
		inGameObjects = FindObjectsOfType<InGameObject>();
		SpawnDecorations();
		InvokeRepeating("SpawnDecorations", 30, Random.Range(45, 75));
		InvokeRepeating("PeriodicItemActionSlow", 60, 60);
		InvokeRepeating("PeriodicItemActionMedium", 30, 30);
		InvokeRepeating("PeriodicItemActionFast", 15, 15);
		InvokeRepeating("GenerateRainProbability", 30, 10);
		InvokeRepeating("RefreshWaitToUseObjects", 1, 1);
		if (!coroutineStarted)
		{
			coroutineStarted = true;
			RefreshCounterText();
			StartCoroutine(DayCounter());
		}
		else
		{
			StartCoroutine(StartCounter());
			RefreshCounterText();
		}
	}

	private void Update()
	{
		if (currentDay == totalDays)
		{
			CampEnded();
		}
		if (energyValue == 0)
		{
			PlayerIsDead();
		}
	}
	public enum Ruolo
	{
		Novizio,
		Terzino,
		Vice,
		Capo,
	}


	public enum Color
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
	#endregion
	#region Objects
	InGameObject[] inGameObjects;

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
