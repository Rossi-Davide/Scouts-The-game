using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class CampManager : MonoBehaviour
{
	#region Singleton
	public static CampManager instance;
	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);
		instance = this;
		DontDestroyOnLoad(instance);
		Screen.orientation = ScreenOrientation.LandscapeLeft;
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

	#region Status

	public static CampSettings CreateDefaultSettings()
	{
		return new CampSettings
		{
			campName = "NuovoCampo",
			playerName = "Player",
			playerSqIndex = 0,
			hair = Hair.Castano,
			gender = Gender.Maschio,
			difficulty = Difficulty.Facile,
			duration = Duration.Breve,
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
	#endregion
	#region Challenges
	int puntiRischiati;
	public void StartChallenge(Challenge type, int puntiRischiati)
	{
		SaveSystem.instance.GetSaveAll();
		this.puntiRischiati = puntiRischiati;
		SceneManager.LoadSceneAsync(type.ToString());
	}
	public IEnumerator GameEnded(bool hasWon)
	{
		SceneManager.LoadSceneAsync("MainScene");
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (hasWon) { GameManager.instance.ChangeCounter(Counter.Punti, puntiRischiati * 2); }
		GameManager.instance.WarningOrMessage(hasWon ? (puntiRischiati > 0 ? $"Hai vinto! Ottieni {puntiRischiati * 2} punti!" : "Hai vinto, ma non hai 'rischiato' nessun punto, perciò non ottieni punti aggiuntivi!") : (puntiRischiati > 0 ? $"Sei stato sconfitto! Perdi {puntiRischiati} punti." : "Hai perso! Fortunatamente non avevi 'rischiato' alcun punto!"), false);
	}
	#endregion
}

[System.Serializable]
public class Camp
{
	public CampSettings settings;
	public Camp(CampSettings settings)
	{
		this.settings = settings.Clone();
	}
}

[System.Serializable]
public class CampSettings
{
	public string campName;
	public string playerName;
	public int playerSqIndex;
	public int[] femaleSqs;
	public int[] maleSqs;
	public Gender gender;
	public Hair hair;
	public Difficulty difficulty;
	public Duration duration;

	public CampSettings Clone()
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
		return new CampSettings
		{
			campName = campName,
			playerName = playerName,
			playerSqIndex = playerSqIndex,
			maleSqs = maleSqsTemp,
			femaleSqs = femaleSqsTemp,
			gender = gender,
			hair = hair,
			difficulty = difficulty,
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
public enum Duration
{
	Breve,
	Media,
	Lunga,
}