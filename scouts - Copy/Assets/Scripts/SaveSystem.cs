using System.Collections;
using System.Data;
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

	public event System.Action<LoadPriority> OnReadyToLoad;

	private void Start()
	{
		InvokeRepeating(nameof(SaveAll), 15, (int)CampManager.instance.camp.settings.savingInterval);
		if (CampManager.instance.camp != null)
		{
			LoadAll();
		}
	}
	private void OnApplicationQuit()
	{
		SaveAll();
	}
	private void OnApplicationPause(bool pause)
	{
		SaveAll();
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

		var quests = new Quest[qt.Length];
		for (int q = 0; q < qt.Length; q++)
		{
			quests[q] = qt[q].quest;
		}
		return new CurrentGameManagerValues(1, 1, 1, items, buildings, actions, quests);
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

		currentCamp = new CurrentCamp(CampManager.instance.camp);
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
		StartCoroutine(LoadWithPrioritization());
	}

	IEnumerator LoadWithPrioritization()
	{
		var f = new WaitForEndOfFrame();
		OnReadyToLoad?.Invoke(LoadPriority.Highest);
		yield return f;
		OnReadyToLoad?.Invoke(LoadPriority.High);
		yield return f;
		OnReadyToLoad?.Invoke(LoadPriority.Normal);
		yield return f;
		OnReadyToLoad?.Invoke(LoadPriority.Low);
	}

	public object RequestData(DataCategory category, DataKey key)
	{
		switch (category)
		{
			//ActionButtons - selected
			//
		}
		throw new System.NotImplementedException("Non esiste");
	}
	public object RequestData(DataCategory category, DataKey key, DataParameter param, int index)
	{
		switch (category)
		{
			//ActionButtons - selected
			//
		}
		throw new System.NotImplementedException("Non esiste");
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

	public CurrentGameManagerValues(int currentDay, int currentHour, int currentMinute, Item[] allItems, PlayerBuilding[] allBuildings, PlayerAction[] allActions, Quest[] allQuests)
	{
		this.currentDay = currentDay;
		this.currentHour = currentHour;
		this.currentMinute = currentMinute;
		this.allItems = allItems;
		this.allBuildings = allBuildings;
		this.allActions = allActions;
		this.allQuests = allQuests;
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

public enum DataCategory
{
	ActionButtons,
	ActionManager,
	AIsManager,
	CampManager,
	ChestManager,
	InventoryManager,
	IterateMultipleObjs,
	Player,
	QuestManager,
	SquadrigliaManager,
	Shop,
	InGameObject,
	PlayerBuildingBase,
	GameManager,
}
public enum DataKey
{
	selected, //ActionButtons
	currentActions, //ActionManager
	currentHiddenActions, //ActionManager
	allSquadriglieri, //AIsManager
	events, //AIsManager
	allCapiECambu, //AIsManager
	camp, //CampManager
	slots, //InventoryManager, ChestManager
	bundles, //IterateMultipleObjs
	objects, //IterateMultipleObjs
	position, //Player, InGameObject
	active, //InGameObject
	quests, //QuestManager
	squadriglieInGioco, //SquadrigliaManager
	sq, //SquadrigliaManager
	itemDatabase, //Shop
	buildingDatabase, //Shop
	objectName, //InGameObject
	objectSubName, //InGameObject
	buttons, //InGameObject
	health, //PlaterBuildingBase
	isDestroyed, //PlaterBuildingBase
	isSafe, //PlaterBuildingBase
	energyValue, //GameManager
	materialsValue, //GameManager
	pointsValue, //GameManager
	energyMaxValue, //GameManager
	materialsMaxValue, //GameManager
	pointsMaxValue, //GameManager
	currentDay, //GameManager
	currentHour, //GameManager
	currentMinute, //GameManager
	isRaining, //GameManager
	rainTimeLeft, //GameManager
	rainWaitTimeLeft, //GameManager
}
public enum DataParameter
{
	nextAction, //IterateMultipleObjs
	obj, //IterateMultipleObjs
	prizeTaken, //QuestManager
	timesDone, //QuestManager
	baseSq, //SquadrigliaManager
	angolo, //SquadrigliaManager
	buildings, //SquadrigliaManager
	ruoli, //SquadrigliaManager
	nomi, //SquadrigliaManager
	materials, //SquadrigliaManager
	points, //SquadrigliaManager
	AIPrefabTypes, //SquadrigliaManager
	item, //Shop, InventoryManager, ChestManager
	building, //Shop
	canDo, //InGameObject
	isWaiting, //InGameObject
	timeLeft, //InGameObject, AIsManager
	nextDialogueIndex, //AIsManager
	running, //AIsManager
}

public enum LoadPriority
{
	Highest,
	High,
	Normal,
	Low,
}