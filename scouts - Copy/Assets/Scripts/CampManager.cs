using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;
using System.Collections;

public class CampManager : MonoBehaviour
{
	#region Singleton
	public static CampManager instance;
	private void Awake()
	{
		if (instance != null)
			Destroy(this);
		instance = this;
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		DontDestroyOnLoad(instance);
	}
	#endregion

	[Header("Information")]
	public Squadriglia[] possibleFemaleSqs;
	public Squadriglia[] possibleMaleSqs;


	[HideInInspector] [System.NonSerialized]
	public Camp camp;

	[HideInInspector] [System.NonSerialized]
	public bool campCreated;

	void Start()
	{
		campCreated = false;
		camp = new Camp(CreateDefaultSettings());
		SetStatus(SaveSystem.instance.LoadData<Status>(SaveSystem.instance.campManagerFileName, false));
	}

	public static Settings CreateDefaultSettings()
	{
		return new Settings
		{
			campName = "NuovoCampo",
			playerName = "Player",
			playerSqIndex = 0,
			hair = Hair.Castano,
			gender = Gender.Maschio,
			difficulty = Difficulty.Facile,
			femaleSqs = new int[] { 0, 2, 4 },
			maleSqs = new int[] { 1, 2, 3 },
		};
	}

	public void CreateCamp(Camp c)
	{
		camp = c;
		campCreated = true;
	}

	public Status SendStatus()
	{
		return new Status
		{
			camp = camp,
			campCreated = campCreated,
		};
	}
	void SetStatus(Status status)
	{
		if (status != null)
		{
			camp.settings = status.camp.settings;
			campCreated = status.campCreated;
		}
	}
	public class Status
	{
		public Camp camp;
		public bool campCreated;
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
	public Gender gender;
	public Hair hair;
	public Difficulty difficulty;

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
			campName = this.campName,
			playerName = this.playerName,
			playerSqIndex = this.playerSqIndex,
			maleSqs = maleSqsTemp,
			femaleSqs = femaleSqsTemp,
			gender = this.gender,
			hair = this.hair,
			difficulty = this.difficulty,
		};
	}
}
public enum Gender
{
	Femmina,
	Maschio,
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