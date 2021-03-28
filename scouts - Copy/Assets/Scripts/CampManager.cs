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
	public Duration[] possibleDurations;


	[HideInInspector] [NonSerialized]
	public Camp camp;

	[HideInInspector] [NonSerialized]
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
			durationIndex = 0,
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

	public int MultiplyByDurationFactor(int baseValue, DurationFactor factor)
	{
		int r = baseValue;
		for (int i = 1; i <= camp.settings.durationIndex; i++)
		{
			r = (int)(r * GetFactorValue(factor, i));
			//Debug.Log($"factor: {GetFactorValue(factor, i)}, r: {r}");
		}
		//Debug.Log(r);
		return r;
	}
	double GetFactorValue(DurationFactor factor, int durationIndex)
	{
		switch (factor)
		{
			case DurationFactor.actionDurationFactor: return possibleDurations[durationIndex].actionDurationFactor;
			case DurationFactor.actionWaitTimeFactor: return possibleDurations[durationIndex].actionWaitTimeFactor;
			case DurationFactor.shopPricesFactor: return possibleDurations[durationIndex].shopPricesFactor;
			case DurationFactor.actionPricesFactor: return possibleDurations[durationIndex].actionPricesFactor;
			case DurationFactor.prizesFactor: return possibleDurations[durationIndex].prizesFactor;
			default: throw new System.NotImplementedException();
		}
	}

	#region Challenges
	int puntiRischiati;
	public void StartChallenge(Challenge type, int puntiRischiati)
	{
		SaveSystem.instance.GetSaveAll();
		this.puntiRischiati = puntiRischiati;
		StartCoroutine(CaricaGioco(type));
	}

	private IEnumerator CaricaGioco(Challenge type)
    {
		yield return new WaitForSeconds(2f);
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
	public int durationIndex;

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
		};
	}
}
public enum Gender
{
	Femmina,
	Maschio,
}
public enum Hair
{
	Biondo,
	Castano,
}
public enum DurationFactor
{
	actionDurationFactor,
	actionWaitTimeFactor,
	shopPricesFactor,
	actionPricesFactor,
	prizesFactor,
}