using UnityEngine.UI;
using TMPro;
using UnityEngine;

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
	public Camp standardCamp;
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
		if (isCreating)
		{
			newCamp = new Camp(standardCamp.settings);
			RefreshUI();
		}
	}
	public void SwitchPanel()
	{
		settingsPanels[currentPanelIndex].SetActive(false);
		currentPanelIndex = currentPanelIndex == settingsPanels.Length - 1 ? 0 : currentPanelIndex + 1;
		settingsPanels[currentPanelIndex].SetActive(true);
	}
	public void ResetSettings()
	{
		newCamp = standardCamp;
		RefreshUI();
	}
	public void CreateCamp()
	{
		Debug.Log("created");
	}

	GameObject FindInTransform(string path)
	{
		return panel.transform.Find(path).gameObject;
	}

	Button campName, playerName, playerSq, gender, hair, difficulty, rain, dayCycle, map, savingInterval;
	Button[] femaleSqs, maleSqs;
	private void Start()
	{
		campName = FindInTransform("Base/NomeCampo/Button").GetComponent<Button>();
		playerName = FindInTransform("Base/NomePlayer/Button").GetComponent<Button>();
		playerSq = FindInTransform("Base/Squadriglia/Button").GetComponent<Button>();
		gender = FindInTransform("Base/Genere/Button").GetComponent<Button>();
		hair = FindInTransform("Base/Aspetto/Button").GetComponent<Button>();
		difficulty = FindInTransform("Base/Difficoltà/Button").GetComponent<Button>();
		femaleSqs = FindInTransform("Advanced1/Squadriglie/Femminili").GetComponentsInChildren<Button>();
		maleSqs = FindInTransform("Advanced1/Squadriglie/Maschili").GetComponentsInChildren<Button>();
		savingInterval = FindInTransform("Advanced1/NomeCampo/Button").GetComponent<Button>();
		map = FindInTransform("Advanced1/NomeCampo/Button").GetComponent<Button>();
		dayCycle = FindInTransform("Advanced1/NomeCampo/Button").GetComponent<Button>();
		rain = FindInTransform("Advanced2/Pioggia/Button").GetComponent<Button>();
	}
	void RefreshUI()
	{
		//base settings
		
		campName.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.campName;
		playerName.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.playerName;
		playerSq.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.playerSq.name;
		gender.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.gender.ToString();
		hair.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.hair.ToString();
		difficulty.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.difficulty.ToString();
		//advanced settings 1
		for (int sq = 0; sq < femaleSqs.Length; sq++)
		{
			femaleSqs[sq].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.femaleSqs[sq].name;
		}
		for (int sq = 0; sq < maleSqs.Length; sq++)
		{
			maleSqs[sq].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.maleSqs[sq].name;
		}
		savingInterval.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.savingInterval.ToString();
		map.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.map.ToString();
		dayCycle.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.dayCycle.ToString();
		//advanced settings 2
		rain.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = newCamp.settings.rain.ToString();
	}
}

[System.Serializable]
public class Camp
{
	public Settings settings;
	public Camp(Settings settings)
	{
		this.settings = settings;
	}
}

[System.Serializable]
public class Settings
{
	public string campName;
	public string playerName;
	public Squadriglia playerSq;
	public Squadriglia[] maleSqs;
	public Squadriglia[] femaleSqs;
	public SavingInterval savingInterval;
	public Gender gender;
	public Hair hair;
	public Difficulty difficulty;
	public Map map;
	public DaylightCycle dayCycle;
	public bool rain;
}
public enum Gender
{
	Female,
	Male,
}
public enum DaylightCycle
{
	Normal,
	NightOnly,
	DayOnly,
}

public enum Map
{
	Hill,
	Magma,
	Snowy,
}
public enum Difficulty
{
	Easy,
	Medium,
	Hard,
	Impossible,
}
public enum Hair
{
	blonde,
	brown,
}
public enum SavingInterval
{
	Slow = 600,
	Medium = 300,
	Fast = 60,
	VeryFast = 30,
	Fastest = 10,
}
