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
		if (instance != null)
			throw new System.Exception("CampSettings non è un singleton!");
		instance = this;
	}
	#endregion

	bool isCreating;
	[Header("UI")]
	public GameObject panel;
	public GameObject noCampPanel;
	public GameObject[] settingsPanels;
	[Header("Information")]
	public Settings standardSettings;
	public Squadriglia[] possibleFemaleSqs;
	public Squadriglia[] possibleMaleSqs;


	[HideInInspector]
	public Camp newCamp;

	int currentPanelIndex;

	public void ToggleCampo()
	{
		isCreating = !isCreating;
		panel.SetActive(isCreating);
		settingsPanels[currentPanelIndex].SetActive(isCreating);
		noCampPanel.SetActive(!isCreating);
		ResetSettings();
	}
	public void SwitchPanel()
	{
		settingsPanels[currentPanelIndex].SetActive(false);
		currentPanelIndex = currentPanelIndex == settingsPanels.Length - 1 ? 0 : currentPanelIndex + 1;
		settingsPanels[currentPanelIndex].SetActive(true);
	}

	public void ResetSettings()
	{
		newCamp.settings = standardSettings.Clone();
		RefreshUI();
	}
	public void CreateCamp()
	{
		Debug.Log("created");
	}

	Button campName, playerName, playerSq, gender, hair, difficulty, rain, dayCycle, map, savingInterval;
	Button[] femaleSqs, maleSqs;
	private void Start()
	{
		campName = panel.transform.Find("Base/NomeCampo/Button").GetComponent<Button>();
		playerName = panel.transform.Find("Base/NomePlayer/Button").GetComponent<Button>();
		playerSq = panel.transform.Find("Base/Squadriglia/Button").GetComponent<Button>();
		gender = panel.transform.Find("Base/Genere/Button").GetComponent<Button>();
		hair = panel.transform.Find("Base/Aspetto/Button").GetComponent<Button>();
		difficulty = panel.transform.Find("Base/Difficoltà/Button").GetComponent<Button>();
		femaleSqs = panel.transform.Find("Advanced1/Squadriglie/Femminili").GetComponentsInChildren<Button>();
		maleSqs = panel.transform.Find("Advanced1/Squadriglie/Maschili").GetComponentsInChildren<Button>();
		savingInterval = panel.transform.Find("Advanced1/Salvataggio/Button").GetComponent<Button>();
		map = panel.transform.Find("Advanced1/Mappa/Button").GetComponent<Button>();
		dayCycle = panel.transform.Find("Advanced1/CicloDelGiorno/Button").GetComponent<Button>();
		rain = panel.transform.Find("Advanced2/Pioggia/Button").GetComponent<Button>();
	}
	void RefreshUI()
	{
		//base settings
		campName.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.campName;
		playerName.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.playerName;
		playerSq.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = RefreshPlayerSq();
		gender.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.gender.ToString();
		hair.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.hair.ToString();
		difficulty.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.difficulty.ToString();
		//advanced settings 1
		for (int sq = 0; sq < femaleSqs.Length; sq++)
		{
			femaleSqs[sq].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = possibleFemaleSqs[newCamp.settings.femaleSqs[sq]].name;
		}
		for (int sq = 0; sq < maleSqs.Length; sq++)
		{
			maleSqs[sq].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = possibleMaleSqs[newCamp.settings.maleSqs[sq]].name;
		}
		savingInterval.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.savingInterval.ToString();
		map.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.map.ToString();
		dayCycle.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.dayCycle.ToString();
		//advanced settings 2
		rain.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.rain.ToString();
	}

	#region mobile keyboard
	TouchScreenKeyboard keyboard;
	bool editingCampName;
	bool editingPlayerName;
	public void ChangeCampName()
	{
		keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "Nome del campo", 15);
		editingCampName = true;
	}
	public void ChangePlayerName()
	{
		keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "Nome del giocatore", 15);
		editingPlayerName = true;
	}
	private void Update()
	{
		if (keyboard.status == TouchScreenKeyboard.Status.Done)
		{
			if (editingCampName)
			{
				newCamp.settings.campName = keyboard.text;
				editingCampName = false;
			}
			else if (editingPlayerName)
			{
				newCamp.settings.playerName = keyboard.text;
				editingPlayerName = false;
			}
			RefreshUI();
		}
	}
	#endregion

	#region change settings
	int NextInArray(int current, int lenght)
	{
		return current < lenght - 1 ? current + 1 : 0;
	}
	public void ToggleRain()
	{
		newCamp.settings.rain = !newCamp.settings.rain;
		RefreshUI();
	}
	public void SwitchSavingInterval()
	{
		newCamp.settings.savingInterval = (SavingInterval)NextInArray((int)newCamp.settings.savingInterval, Enum.GetNames(typeof(SavingInterval)).Length);
		RefreshUI();
	}
	public void SwitchDayCycle()
	{
		newCamp.settings.dayCycle = (DaylightCycle)NextInArray((int)newCamp.settings.dayCycle, Enum.GetNames(typeof(DaylightCycle)).Length);
		RefreshUI();
	}
	public void SwitchMap()
	{
		newCamp.settings.map = (Map)NextInArray((int)newCamp.settings.map, Enum.GetNames(typeof(Map)).Length);
		RefreshUI();
	}
	public void SwitchDifficulty()
	{
		newCamp.settings.difficulty = (Difficulty)NextInArray((int)newCamp.settings.difficulty, Enum.GetNames(typeof(Difficulty)).Length);
		RefreshUI();
	}
	public void SwitchHair()
	{
		newCamp.settings.hair = (Hair)NextInArray((int)newCamp.settings.hair, Enum.GetNames(typeof(Hair)).Length);
		RefreshUI();
	}
	public void SwitchGender()
	{
		newCamp.settings.gender = (Gender)NextInArray((int)newCamp.settings.gender, Enum.GetNames(typeof(Gender)).Length);
		RefreshUI();
	}
	public void ChangePlayerSq()
	{
		if (newCamp.settings.gender == Gender.Femmina)
		{
			newCamp.settings.playerSqIndex = NextInArray(newCamp.settings.playerSqIndex, newCamp.settings.femaleSqs.Length);
		}
		else if (newCamp.settings.gender == Gender.Maschio)
		{
			newCamp.settings.playerSqIndex = NextInArray(newCamp.settings.playerSqIndex, newCamp.settings.maleSqs.Length);
		}
		RefreshUI();
		RefreshPlayerSq();
	}

	public void ChangeFemaleSqs(int index)
	{
		int c = Array.IndexOf(possibleFemaleSqs, newCamp.settings.femaleSqs[index]);
		for (int i = 0; i < possibleFemaleSqs.Length; i++)
		{
			if (!Array.Exists(newCamp.settings.femaleSqs, element => element == c))
			{
				newCamp.settings.femaleSqs[index] = c;
				RefreshUI();
				return;
			}
			c++;
			if (c > possibleFemaleSqs.Length - 1)
				c = 0;
		}
		RefreshPlayerSq();
	}
	public void ChangeMaleSqs(int index)
	{
		int c = Array.IndexOf(possibleMaleSqs, newCamp.settings.maleSqs[index]);
		for (int i = 0; i < possibleMaleSqs.Length; i++)
		{
			if (!Array.Exists(newCamp.settings.maleSqs, element => element == c))
			{
				newCamp.settings.maleSqs[index] = c;
				RefreshUI();
				return;
			}
			c++;
			if (c > possibleMaleSqs.Length - 1)
				c = 0;
		}
		RefreshPlayerSq();
	}

	#endregion

	string RefreshPlayerSq()
	{
		if (newCamp.settings.gender == Gender.Femmina)
		{
			return possibleFemaleSqs[newCamp.settings.femaleSqs[newCamp.settings.playerSqIndex]].name;
		}
		if (newCamp.settings.gender == Gender.Maschio)
		{
			return possibleMaleSqs[newCamp.settings.maleSqs[newCamp.settings.playerSqIndex]].name;
		}
		return null;
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
	public int[] maleSqs;
	public int[] femaleSqs;
	public SavingInterval savingInterval;
	public Gender gender;
	public Hair hair;
	public Difficulty difficulty;
	public Map map;
	public DaylightCycle dayCycle;
	public bool rain;

	public Settings Clone()
	{
		return new Settings
		{
			campName = this.campName,
			playerName = this.playerName,
			playerSqIndex = this.playerSqIndex,
			maleSqs = this.maleSqs,
			femaleSqs = this.femaleSqs,
			savingInterval = this.savingInterval,
			gender = this.gender,
			hair = this.hair,
			difficulty = this.difficulty,
			map = this.map,
			dayCycle = this.dayCycle,
			rain = this.rain,
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
