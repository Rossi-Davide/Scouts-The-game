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
		DontDestroyOnLoad(this);
	}
	#endregion

	public event System.Action<LoadPriority> OnReadyToLoad;
	public object RequestData(DataCategory c, DataKey k) { return null; }
	public object RequestData(DataCategory c, DataKey k, DataParameter p, int i) { return null; }

	private void Start()
	{
		LoadAll();

		InvokeRepeating(nameof(GetSaveAll), 5, 20);
	}
	private void OnApplicationQuit()
	{
		GetSaveAll();
	}

	void SaveData(object o, string fileName)
	{
		string path = Application.persistentDataPath + $"/{fileName}.json";
		var json = JsonUtility.ToJson(o);
		Debug.Log($"SaveData: {json}");
		System.IO.File.WriteAllText(path, json);
	}
	T LoadData<T>(string fileName)
	{
		try
		{
			string path = Application.persistentDataPath + $"/{fileName}.json";
			if (!System.IO.File.Exists(path))
			{
				return default;
			}
			var d = JsonUtility.FromJson<T>(System.IO.File.ReadAllText(path));
			Debug.Log($"LoadData path: {path} Completed. The result is null: {d == null}. ");
			return d;
		}
		catch (System.Exception ex)
		{
			Debug.Log($"Error in LoadData path: {ex.Message}");
			return default;
		}
	}
	public void GetSaveAll()
	{
		if (CampManager.instance != null && CampManager.instance.camp != null) { SaveData(CampManager.instance.GetStatus(), "CampManager"); }
		Debug.Log("saved data");
	}
	public void LoadAll()
	{
		var campManager = CampManager.instance;
		if (campManager != null)
		{
			var data = LoadData<CampManager.Status>("CampManager");
			if (data != null)
			{
				campManager.SetStatus(data);
			}
		}
		Debug.Log("loaded data");
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