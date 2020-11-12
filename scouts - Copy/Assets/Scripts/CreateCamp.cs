using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateCamp : MonoBehaviour
{
	bool isCreating;
	[Header("UI")]
	public GameObject createCampPanel;
	public GameObject existingCampPanel;
	public GameObject confirmationWindow;
	public GameObject noCampPanel;
	public GameObject[] settingsPanels;

	Camp camp;
	int currentPanelIndex;
	CampManager campManager;

	Button campName, playerName, playerSq, gender, hair, difficulty, savingInterval;
	TextMeshProUGUI exCampName, exPlayerName, exPlayerSq, exGender, exHair, exDifficulty, exSavingInterval, exSqs; //"ex" stands for "existing"
	Button[] femaleSqs, maleSqs;

	public void BackToMenu()
	{
		SceneLoader.instance.LoadMainMenuScene();
	}

	private void Start()
	{
		createCampPanel.transform.parent.Find("General/Home").GetComponent<Button>().onClick.AddListener(SceneLoader.instance.LoadMainMenuScene);
		campName = createCampPanel.transform.Find("Base/NomeCampo/Button").GetComponent<Button>();
		playerName = createCampPanel.transform.Find("Base/NomePlayer/Button").GetComponent<Button>();
		playerSq = createCampPanel.transform.Find("Base/Squadriglia/Button").GetComponent<Button>();
		gender = createCampPanel.transform.Find("Base/Genere/Button").GetComponent<Button>();
		hair = createCampPanel.transform.Find("Base/Aspetto/Button").GetComponent<Button>();
		difficulty = createCampPanel.transform.Find("Base/Difficoltà/Button").GetComponent<Button>();
		femaleSqs = createCampPanel.transform.Find("Advanced/Squadriglie/Femminili").GetComponentsInChildren<Button>();
		maleSqs = createCampPanel.transform.Find("Advanced/Squadriglie/Maschili").GetComponentsInChildren<Button>();
		savingInterval = createCampPanel.transform.Find("Advanced/Salvataggio/Button").GetComponent<Button>();

		exCampName = existingCampPanel.transform.Find("Info/Campo").GetComponent<TextMeshProUGUI>();
		exPlayerName = existingCampPanel.transform.Find("Info/Player").GetComponent<TextMeshProUGUI>();
		exPlayerSq = existingCampPanel.transform.Find("Info/Squadriglia").GetComponent<TextMeshProUGUI>();
		exGender = existingCampPanel.transform.Find("Info/Genere").GetComponent<TextMeshProUGUI>();
		exHair = existingCampPanel.transform.Find("Info/Aspetto").GetComponent<TextMeshProUGUI>();
		exSqs = existingCampPanel.transform.Find("Info/Squadriglie").GetComponent<TextMeshProUGUI>();
		exDifficulty = existingCampPanel.transform.Find("Info/Difficoltà").GetComponent<TextMeshProUGUI>();
		exSavingInterval = existingCampPanel.transform.Find("Info/Salvataggio").GetComponent<TextMeshProUGUI>();

		campManager = CampManager.instance;
		camp = campManager.camp;
		if (camp != null)
		{
			existingCampPanel.SetActive(true);
			noCampPanel.SetActive(false);
			RefreshExistingCampUI();
		}
		else
		{
			existingCampPanel.SetActive(false);
			noCampPanel.SetActive(camp == null);
		}
	}

	public void PartialDeleteCamp()
	{
		confirmationWindow.SetActive(true);
	}
	public void CancelDelete()
	{
		confirmationWindow.SetActive(false);
	}
	public void DeleteCamp()
	{
		campManager.camp = null;
		confirmationWindow.SetActive(false);
		existingCampPanel.SetActive(false);
		noCampPanel.SetActive(true);
	}

	void RefreshExistingCampUI()
	{
		exCampName.text = "Nome del campo: " + camp.settings.campName;
		exPlayerName.text = "Nome del player: " + camp.settings.playerName;
		exPlayerSq.text = "Squadriglia del player: " + RefreshPlayerSq();
		exSqs.text = "Squadriglie: ";
		exGender.text = "Genere: " + camp.settings.gender;
		exHair.text = "Aspetto: " + camp.settings.hair;
		exDifficulty.text = "Difficoltà: " + camp.settings.difficulty;
		exSavingInterval.text = "Salvataggio: " + camp.settings.savingInterval;

		for (int s = 0; s < femaleSqs.Length; s++)
		{
			exSqs.text += campManager.possibleFemaleSqs[campManager.camp.settings.femaleSqs[s]].name + "; ";
		}
		for (int s = 0; s < maleSqs.Length; s++)
		{
			exSqs.text += campManager.possibleMaleSqs[campManager.camp.settings.maleSqs[s]].name + "; ";
		}
	}

	void RefreshUI()
	{
		//base settings
		campName.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = camp.settings.campName;
		playerName.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = camp.settings.playerName;
		playerSq.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = RefreshPlayerSq();
		gender.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = camp.settings.gender.ToString();
		hair.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = camp.settings.hair.ToString();
		difficulty.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = camp.settings.difficulty.ToString();
		//advanced settings 1
		for (int sq = 0; sq < femaleSqs.Length; sq++)
		{
			femaleSqs[sq].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = campManager.possibleFemaleSqs[camp.settings.femaleSqs[sq]].name;
		}
		for (int sq = 0; sq < maleSqs.Length; sq++)
		{
			maleSqs[sq].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = campManager.possibleMaleSqs[camp.settings.maleSqs[sq]].name;
		}
		savingInterval.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = camp.settings.savingInterval.ToString();
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
				camp.settings.campName = keyboard.text;
				editingCampName = false;
			}
			else if (editingPlayerName)
			{
				camp.settings.playerName = keyboard.text;
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
	public void SwitchSavingInterval()
	{
		camp.settings.savingInterval = (SavingInterval)NextInArray((int)camp.settings.savingInterval, Enum.GetNames(typeof(SavingInterval)).Length);
		RefreshUI();
	}
	public void SwitchDifficulty()
	{
		camp.settings.difficulty = (Difficulty)NextInArray((int)camp.settings.difficulty, Enum.GetNames(typeof(Difficulty)).Length);
		RefreshUI();
	}
	public void SwitchHair()
	{
		camp.settings.hair = (Hair)NextInArray((int)camp.settings.hair, Enum.GetNames(typeof(Hair)).Length);
		RefreshUI();
	}
	public void SwitchGender()
	{
		camp.settings.gender = (Gender)NextInArray((int)camp.settings.gender, Enum.GetNames(typeof(Gender)).Length);
		RefreshUI();
	}
	public void ChangePlayerSq()
	{
		if (camp.settings.gender == Gender.Femmina)
		{
			camp.settings.playerSqIndex = NextInArray(camp.settings.playerSqIndex, camp.settings.femaleSqs.Length);
		}
		else if (camp.settings.gender == Gender.Maschio)
		{
			camp.settings.playerSqIndex = NextInArray(camp.settings.playerSqIndex, camp.settings.maleSqs.Length);
		}
		RefreshUI();
		RefreshPlayerSq();
	}

	public void ChangeFemaleSqs(int index)
	{
		int c = Array.IndexOf(campManager.possibleFemaleSqs, campManager.possibleFemaleSqs[camp.settings.femaleSqs[index]]);
		for (int i = 0; i < campManager.possibleFemaleSqs.Length; i++)
		{
			if (!Array.Exists(camp.settings.femaleSqs, element => element == c))
			{
				camp.settings.femaleSqs[index] = c;
				RefreshUI();
				return;
			}
			c++;
			if (c > campManager.possibleFemaleSqs.Length - 1)
				c = 0;
		}
		RefreshPlayerSq();
	}
	public void ChangeMaleSqs(int index)
	{
		int c = Array.IndexOf(campManager.possibleMaleSqs, campManager.possibleMaleSqs[camp.settings.maleSqs[index]]);
		for (int i = 0; i < campManager.possibleMaleSqs.Length; i++)
		{
			if (!Array.Exists(camp.settings.maleSqs, element => element == c))
			{
				camp.settings.maleSqs[index] = c;
				RefreshUI();
				return;
			}
			c++;
			if (c > campManager.possibleMaleSqs.Length - 1)
				c = 0;
		}
		RefreshPlayerSq();
	}

	public void Create()
	{
		campManager.CreateCamp(camp);
	}

	#endregion

	#region Create camp general

	public void ToggleCampo()
	{
		isCreating = !isCreating;
		createCampPanel.SetActive(isCreating);
		settingsPanels[currentPanelIndex].SetActive(isCreating);
		noCampPanel.SetActive(!isCreating);
		camp = campManager.camp;
		if (camp == null)
			camp = new Camp(campManager.standardSettings);
		RefreshUI();
	}
	public void SwitchPanel()
	{
		settingsPanels[currentPanelIndex].SetActive(false);
		currentPanelIndex = currentPanelIndex == settingsPanels.Length - 1 ? 0 : currentPanelIndex + 1;
		settingsPanels[currentPanelIndex].SetActive(true);
	}

	public void ResetSettings()
	{
		camp.settings = campManager.standardSettings.Clone();
		RefreshUI();
	}
	string RefreshPlayerSq()
	{
		if (camp.settings.gender == Gender.Femmina)
		{
			return campManager.possibleFemaleSqs[camp.settings.femaleSqs[camp.settings.playerSqIndex]].name;
		}
		if (camp.settings.gender == Gender.Maschio)
		{
			return campManager.possibleMaleSqs[camp.settings.maleSqs[camp.settings.playerSqIndex]].name;
		}
		return null;
	}

	#endregion
}
