using UnityEngine;
public class SaveSystem : MonoBehaviour
{
	#region Singleton
	public static SaveSystem instance;
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("Savesystem is not a singleton");
		instance = this;
		LoadAll();
	}
	#endregion


	private void Start()
	{
		InvokeRepeating(nameof(SaveAll), 15, (int)CampManager.instance.newCamp.settings.savingInterval);
	}
	private void OnApplicationQuit()
	{
		SaveAll();
	}
	private void OnApplicationPause(bool pause)
	{
		SaveAll();
	}
	private void OnApplicationFocus(bool focus)
	{
		LoadAll();
	}
	#region GetInfo
	CurrentTimeActions GetTimeActions()
	{
		var timeActionArray = new TimeAction[ActionManager.instance.currentActions.Length];
		for (int i = 0; i < ActionManager.instance.currentActions.Length; i++)
			timeActionArray[i] = ActionManager.instance.currentActions[i];
		var hiddeinActionArray = new TimeAction[ActionManager.instance.currentHiddenActions.Count];
		for (int i = 0; i < ActionManager.instance.currentHiddenActions.Count; i++)
			hiddeinActionArray[i] = ActionManager.instance.currentHiddenActions[i];
		return new CurrentTimeActions(timeActionArray, hiddeinActionArray);
	}

	CurrentAppSettings GetAppSettings()
	{
		return CampManager.instance.appSettings;
	}

	CurrentPLayerValues GetPlayerValues()
	{
		var pos = Player.instance.transform.position;
		var inventory = InventoryManager.instance.slots;
		var chest = ChestManager.instance.slots;
		var materials = GameManager.instance.materialsValue;
		var maxMaterials = 1000;
		var energy = GameManager.instance.energyValue;
		var points = GameManager.instance.pointsValue;
		return new CurrentPLayerValues(pos, inventory, chest, materials, maxMaterials, energy, points);
	}

	CurrentGameManagerValues GetGameManagerValues() {
		var items = Shop.instance.itemDatabase;
		var buildings = Shop.instance.buildingDatabase;
		var actions = QuestManager.instance.actionDatabase;
		var qt = QuestManager.instance.quests;
		var spawned = GameManager.instance.spawnedDecorations;
		var objects = GameManager.instance.InGameObjects;

		var quests = new Quest[qt.Length];
		for (int q = 0; q < qt.Length; q++)
		{
			quests[q] = qt[q].quest;
		}
		var plants = new Plant[spawned.Count];
		for (var p = 0; p < plants.Length; p++)
		{
			plants[p] = spawned[p].GetComponent<Plant>();
		}


		return new CurrentGameManagerValues(1, 1, 1, items, buildings, actions, quests, plants, objects);
	}

	CurrentAIs GetCurrentAIs()
	{
		var capiECambu = AIsManager.instance.allCapiECambu;
		var squadriglieri = AIsManager.instance.allSquadriglieri;

		return new CurrentAIs(capiECambu, squadriglieri);
	}

	#endregion

	#region Save and load all
	public void SaveAll()
	{
		currentTimeActions = GetTimeActions();
		jsonCurrentTimeActions = JsonUtility.ToJson(currentTimeActions);

		currentAppSettings = GetAppSettings();
		jsonCurrentAppSettings = JsonUtility.ToJson(currentAppSettings);

		currentSquadriglias = new CurrentSquadriglias(SquadrigliaManager.instance.GetInfo());
		jsonCurrentSquadriglias = JsonUtility.ToJson(currentSquadriglias);

		currentCamp = new CurrentCamp(CampManager.instance.newCamp);
		jsonCurrentCamp = JsonUtility.ToJson(currentCamp);

		currentPlayerValues = GetPlayerValues();
		jsonCurrentPlayerValues = JsonUtility.ToJson(currentPlayerValues);

		currentGameManagerValues = GetGameManagerValues();
		jsonCurrentGameManagerValues = JsonUtility.ToJson(currentGameManagerValues);

		currentAIs = GetCurrentAIs();
		jsonCurrentAIs = JsonUtility.ToJson(currentAIs);
	}

	public void LoadAll()
	{
		currentTimeActions = JsonUtility.FromJson<CurrentTimeActions>(jsonCurrentTimeActions);
		currentAppSettings = JsonUtility.FromJson<CurrentAppSettings>(jsonCurrentAppSettings);
		currentSquadriglias = JsonUtility.FromJson<CurrentSquadriglias>(jsonCurrentSquadriglias);
		currentCamp = JsonUtility.FromJson<CurrentCamp>(jsonCurrentCamp);
		currentGameManagerValues = JsonUtility.FromJson<CurrentGameManagerValues>(jsonCurrentGameManagerValues);
		currentAIs = JsonUtility.FromJson<CurrentAIs>(jsonCurrentAIs);
		UnityEngine.Debug.Log("loaded data");
	}

	#endregion

	public CurrentTimeActions currentTimeActions;
	public string jsonCurrentTimeActions;

	public CurrentAppSettings currentAppSettings;
	public string jsonCurrentAppSettings;

	public CurrentSquadriglias currentSquadriglias;
	public string jsonCurrentSquadriglias;

	public CurrentCamp currentCamp;
	public string jsonCurrentCamp;

	public CurrentPLayerValues currentPlayerValues;
	public string jsonCurrentPlayerValues;

	public CurrentGameManagerValues currentGameManagerValues;
	public string jsonCurrentGameManagerValues;

	public CurrentAIs currentAIs;
	public string jsonCurrentAIs;
}
public class CurrentTimeActions
{
	public TimeAction[] shownActions;
	public TimeAction[] hiddenActions;
	public CurrentTimeActions(TimeAction[] shownActions, TimeAction[] hiddenActions)
	{
		this.shownActions = shownActions;
		this.hiddenActions = hiddenActions;
	}
}
public class CurrentGameManagerValues
{
	public int currentDay;
	public int currentHour;
	public int currentMinute;

	public Item[] allItems;
	public PlayerBuilding[] allBuildings;
	public PlayerAction[] allActions;
	public Quest[] allQuests;

	public Plant[] spawnedPlants;
	public InGameObject[] allObjects;

	public CurrentGameManagerValues(int currentDay, int currentHour, int currentMinute, Item[] allItems, PlayerBuilding[] allBuildings, PlayerAction[] allActions, Quest[] allQuests, Plant[] spawnedPlants, InGameObject[] allObjects)
	{
		this.currentDay = currentDay;
		this.currentHour = currentHour;
		this.currentMinute = currentMinute;
		this.allItems = allItems;
		this.allBuildings = allBuildings;
		this.allActions = allActions;
		this.allQuests = allQuests;
		this.spawnedPlants = spawnedPlants;
		this.allObjects = allObjects;
	}

}
[System.Serializable]
public class CurrentAppSettings
{
	public float generalVolume;
	public float musicVolume;
	public float effectsVolume;
	public int qualityIndex;
	public int resIndex;
	public bool fullScreen;
	public CurrentAppSettings(float g, float m, float e, int qi, int ri, bool f)
	{
		generalVolume = g;
		musicVolume = m;
		effectsVolume = e;
		qualityIndex = qi;
		resIndex = ri;
		fullScreen = f;
	}
}
public class CurrentCamp
{
	public Camp camp;

	public CurrentCamp(Camp c)
	{
		camp = c;
	}
}
public class CurrentSquadriglias
{
	public ConcreteSquadriglia[] currentSquadriglias;

	public CurrentSquadriglias(ConcreteSquadriglia[] squadriglias)
	{
		currentSquadriglias = squadriglias;
	}
}
public class CurrentPLayerValues
{
	public Vector3 position;
	public InventorySlot[] inventory;
	public InventorySlot[] chest;
	public int materials;
	public int maxMaterials;
	public int energy;
	public int points;

	public CurrentPLayerValues(Vector3 position, InventorySlot[] inventory, InventorySlot[] chest, int materials, int maxMaterials, int energy, int points)
	{
		this.position = position;
		this.inventory = inventory;
		this.chest = chest;
		this.materials = materials;
		this.maxMaterials = maxMaterials;
		this.energy = energy;
		this.points = points;
	}
}
public class CurrentAIs
{
	public CapieCambu[] allCapiECambu;
	public Squadrigliere[] allSquadriglieri;

	public CurrentAIs(CapieCambu[] allCapiECambu, Squadrigliere[] allSquadriglieri)
	{
		this.allCapiECambu = allCapiECambu;
		this.allSquadriglieri = allSquadriglieri;
	}
}