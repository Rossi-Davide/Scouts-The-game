using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class CampManager : MonoBehaviour
{
	#region Singleton
	public static CampManager instance;
	private void Awake()
	{
		if (instance == null)
			instance = this;
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
	#endregion

	[Header("Information")]
	public Settings standardSettings;
	public Squadriglia[] possibleFemaleSqs;
	public Squadriglia[] possibleMaleSqs;


	[HideInInspector]
	public Camp camp;
	[HideInInspector]
	public CurrentAppSettings appSettings;
	public CurrentAppSettings standardAppSettings;

	public void CreateCamp(Camp c)
	{
		camp = c;
	}
	private void Start()
	{
		appSettings = new CurrentAppSettings(standardAppSettings.generalVolume, standardAppSettings.musicVolume, standardAppSettings.effectsVolume, standardAppSettings.qualityIndex, standardAppSettings.resIndex, standardAppSettings.fullScreen);
		DontDestroyOnLoad(this);
		SaveSystem.instance.OnReadyToLoad += ReceiveSavedData;
	}

	void ReceiveSavedData()
	{
		camp = (Camp)SaveSystem.instance.RequestData(DataCategory.CampManager, DataKey.camp);
	}
}

[System.Serializable]
public class Camp
{
	public Settings settings;
	public Camp(Settings settings)
	{
		this.settings = settings.Clone();
	}
}

[System.Serializable]
public class Settings
{
	public string campName;
	public string playerName;
	public int playerSqIndex;
	public int[] femaleSqs;
	public int[] maleSqs;
	public SavingInterval savingInterval;
	public Gender gender;
	public Hair hair;
	public Difficulty difficulty;
	public Map map;
	public DaylightCycle dayCycle;

	public Settings Clone()
	{
		int[] maleSqsTemp = new int[maleSqs.Length];
		int[] femaleSqsTemp = new int[femaleSqs.Length];
		for (int m = 0; m < maleSqs.Length; m++)
		{
			maleSqsTemp[m] = maleSqs[m];
		}
		for (int m = 0; m < femaleSqs.Length; m++)
		{
			femaleSqsTemp[m] = femaleSqs[m];
		}
		return new Settings
		{
			campName = this.campName.Clone().ToString(),
			playerName = this.playerName.Clone().ToString(),
			playerSqIndex = this.playerSqIndex,
			maleSqs = maleSqsTemp,
			femaleSqs = femaleSqsTemp,
			savingInterval = this.savingInterval,
			gender = this.gender,
			hair = this.hair,
			difficulty = this.difficulty,
			map = this.map,
			dayCycle = this.dayCycle,
		};
	}
}
public enum Gender
{
	Femmina,
	Maschio,
}
public enum DaylightCycle
{
	Normale,
	SoloNotte,
	SoloGiorno,
}

public enum Map
{
	Hill,
	Magma,
	Snowy,
}
public enum Difficulty
{
	Facile,
	Media,
	Difficile,
	Impossibile,
}
public enum Hair
{
	Biondo,
	Castano,
}
public enum SavingInterval
{
	Slow,
	Medium,
	Fast,
	VeryFast,
	Fastest,
}