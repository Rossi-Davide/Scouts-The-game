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
	}
	#endregion


	public static System.Action ReadyToLoadData;


	private void Start()
	{
		InvokeRepeating("SaveAll", (int)CampManager.instance.newCamp.settings.savingInterval, 10);
	}



	#region GetInfo
	TimeAction[] GetTimeActions()
	{
		var timeActionArray = new TimeAction[ActionManager.instance.currentActions.Length];
		for (int i = 0; i < ActionManager.instance.currentActions.Length; i++)
			timeActionArray[i] = ActionManager.instance.currentActions[i];
		return timeActionArray;
	}

	CurrentAppSettings GetAppSettings()
	{
		var g = impostazioni.instance.generalVolume;
		var m = impostazioni.instance.musicVolume;
		var e = impostazioni.instance.effectsVolume;
		var ri = impostazioni.instance.resIndex;
		var qi = impostazioni.instance.qualityIndex;
		var f = impostazioni.instance.fullscreen;
		return new CurrentAppSettings(g, m, e, qi, ri, f);
	}

	CurrentPLayerValues GetPlayerValues()
	{
		var it = Shop.instance.shopPanel.GetComponentsInChildren<ShopItem>();
		var qt = QuestManager.instance.quests;
		var items = new Item[it.Length];
		var inventory = InventoryManager.instance.slots;
		var chest = ChestManager.instance.slots;
		var actions = QuestManager.instance.actionDatabase;
		var quests = new Quest[qt.Length];
		var materials = GameManager.instance.materialsValue;
		var maxMaterials = 1000;
		var energy = GameManager.instance.energyValue;
		var points = GameManager.instance.pointsValue;

		for (int i = 0; i < items.Length; i++)
		{
			items[i] = it[i].item;
		}
		for (int i = 0; i < quests.Length; i++)
		{
			quests[i] = qt[i].quest;
		}

		return new CurrentPLayerValues(items, inventory, chest, actions, quests, materials, maxMaterials, energy, points);
	}


	#endregion

	#region Save and load all
	public void SaveAll()
	{
		currentTimeActions = new CurrentTimeActions(GetTimeActions());
		jsonCurrentTimeActions = JsonUtility.ToJson(currentTimeActions);

		currentAppSettings = GetAppSettings();
		jsonCurrentAppSettings = JsonUtility.ToJson(currentAppSettings);

		currentSquadriglias = new CurrentSquadriglias(SquadrigliaManager.instance.GetInfo());
		jsonCurrentSquadriglias = JsonUtility.ToJson(currentSquadriglias);

		currentCamp = new CurrentCamp(CampManager.instance.newCamp);
		jsonCurrentCamp = JsonUtility.ToJson(currentCamp);

		currentPlayerValues = GetPlayerValues();
		jsonCurrentPlayerValues = JsonUtility.ToJson(currentPlayerValues);



	}

	public void LoadAll()
	{
		currentTimeActions = JsonUtility.FromJson<CurrentTimeActions>(jsonCurrentTimeActions);
		currentAppSettings = JsonUtility.FromJson<CurrentAppSettings>(jsonCurrentAppSettings);
		currentSquadriglias = JsonUtility.FromJson<CurrentSquadriglias>(jsonCurrentSquadriglias);
		currentCamp = JsonUtility.FromJson<CurrentCamp>(jsonCurrentCamp);
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
}
public class CurrentTimeActions
{
	public TimeAction[] actions;
	public CurrentTimeActions(TimeAction[] actions)
	{
		this.actions = actions;
	}
}
public class CurrentGameManagerValues
{
	public int currentDay;
	public int currentHour;
	public int currentMinute;
	
	//all items and all actions
}
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
public class CurrentObjects
{
	//specific things
	public Tent tent;
	public Portalegna portalegna;
	public Refettorio refettorio;
	public PianoBidoni pianoBidoni;
	public Stendipanni stendipanni;
	public Amaca amaca;

	public AngoloDiAltraSquadriglia[] angoli;

	public Lavaggi lavaggi;
	public Latrina latrina;
	public Campfire campfire;
	public Cambusa cambusa;
	public CassaDelFurfante cassaDelDurfante;
	public Alzabandiera alzabandiera;
	public Montana montana;

	public Decorations[] decorations;
}
public class CurrentPLayerValues
{
	public InventorySlot[] inventory;
	public InventorySlot[] chest;
	public Item[] items;
	public PlayerAction[] actions;
	public Quest[] quests;
	public int materials;
	public int maxMaterials;
	public int energy;
	public int points;

	public CurrentPLayerValues(Item[] items, InventorySlot[] inventory, InventorySlot[] chest, PlayerAction[] actions, Quest[] quests, int materials, int maxMaterials, int energy, int points)
	{
		this.items = items;
		this.inventory = inventory;
		this.chest = chest;
		this.actions = actions;
		this.quests = quests;
		this.materials = materials;
		this.maxMaterials = maxMaterials;
		this.energy = energy;
		this.points = points;
	}
}
public class CurrentAIs
{
	//capi e cambu
		//dialoguesDone
	//squadriglieri con info
}