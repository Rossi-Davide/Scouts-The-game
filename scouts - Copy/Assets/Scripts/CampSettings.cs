using UnityEngine.UI;
using UnityEngine;

public class CampSettings : MonoBehaviour
{
	#region Singleton
	public static CampSettings instance;
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("CampSettings non è un singleton!");
		instance = this;
	}
	#endregion

	bool isCreating;
	[Header("UI")]
	public GameObject panel, noCampPanel;
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


	void RefreshUI()
	{

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
	public string playerName;
	public Squadriglia playerSq;
	public Squadriglia[] inGameSq;
	
}
