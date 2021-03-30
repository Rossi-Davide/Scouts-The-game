using System;
using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveSystem : MonoBehaviour
{
	#region Singleton
	public static SaveSystem instance;
	private void Awake()
	{
		DontDestroyOnLoad(this);
		if (instance != null)
			Destroy(instance.gameObject);	
		instance = this;
	}
	#endregion

	string gameDataDirectoryName = "/GameData";
	string persistentDataDirectoryName = "/PersistentData";
	private void Start()
	{
		string p = Application.persistentDataPath;
		if (!System.IO.Directory.Exists(p + $"{gameDataDirectoryName}"))
		{
			System.IO.Directory.CreateDirectory(p + $"{gameDataDirectoryName}");
		}
		if (!System.IO.Directory.Exists(p + $"{persistentDataDirectoryName}"))
		{
			System.IO.Directory.CreateDirectory(p + $"{persistentDataDirectoryName}");
		}
		InvokeRepeating(nameof(GetSaveAll), 20, 20);
	}
	private void OnApplicationQuit()
	{
		GetSaveAll();
	}

	public void DeleteGameFiles()
	{
		Array.ForEach(System.IO.Directory.GetFiles(Application.persistentDataPath + $"{gameDataDirectoryName}"), System.IO.File.Delete);
	}

	public void SaveData(object o, string fileName, bool isPersistent)
	{
		string p = Application.persistentDataPath;
		string path = p + (isPersistent ? $"{persistentDataDirectoryName}" : $"{gameDataDirectoryName}") + $"/{fileName}.json";
		var json = JsonUtility.ToJson(o);
		System.IO.File.WriteAllText(path, json);
		//Debug.Log($"SaveData: {json}");
	}
	public T LoadData<T>(string fileName, bool isPersistent)
	{
		try
		{
			string path = Application.persistentDataPath + (isPersistent ? $"{persistentDataDirectoryName}" : $"{gameDataDirectoryName}") + $"/{fileName}.json";
			if (!System.IO.File.Exists(path))
			{
				return default;
			}
			var d = JsonUtility.FromJson<T>(System.IO.File.ReadAllText(path));
			//Debug.Log($"LoadData path: {path} Completed. The result is null: {d == null}. ");
			return d;
		}
		catch (Exception ex)
		{
			Debug.Log($"Error in LoadData path: {ex.Message}");
			return default;
		}
	}
	public event Action OnReadyToSaveData;
	public void GetSaveAll()
	{
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
			if (SquadrigliaManager.instance != null) { SaveData(SquadrigliaManager.instance.SendStatus(), squadrigliaManagerFileName, false);}
			if (GameManager.instance != null) { SaveData(GameManager.instance.SendStatus(), gameManagerFileName, false); }
			if (Shop.instance != null) { SaveData(Shop.instance.SendStatus(), shopFileName, false); }
			if (ActionManager.instance != null) { SaveData(ActionManager.instance.SendStatus(), actionManagerFileName, false); }
			if (AIsManager.instance != null) { SaveData(AIsManager.instance.SendStatus(), aisManagerFileName, false); }
			if (ChestManager.instance != null) { SaveData(ChestManager.instance.SendStatus(), chestManagerFileName, false); }
			if (InventoryManager.instance != null) { SaveData(InventoryManager.instance.SendStatus(), inventoryManagerFileName, false); }
			if (Player.instance != null) { SaveData(Player.instance.SendStatus(), plFileName, false); }
			if (QuestManager.instance != null) { SaveData(QuestManager.instance.SendStatus(), questManagerFileName, false); }
			if (ModificaBaseTrigger.instance != null) { SaveData(ModificaBaseTrigger.instance.SendStatus(), modificaBaseTriggerFileName, false); }
			OnReadyToSaveData?.Invoke();
			Debug.Log("saved data");
		}

		if (impostazioni.instance != null) { SaveData(impostazioni.instance.SendStatus(), impostazioniFileName, true); }
		if (CampManager.instance != null && CampManager.instance.camp != null) { SaveData(CampManager.instance.SendStatus(), campManagerFileName, false); }


	}

	[HideInInspector] [System.NonSerialized]
	public string campManagerFileName = "CampManager";
	[HideInInspector]
	[System.NonSerialized]
	public string actionButtonsFileName = "ActionButtons";
	[HideInInspector]
	[System.NonSerialized]
	public string actionManagerFileName = "ActionManager";
	[HideInInspector]
	[System.NonSerialized]
	public string aisManagerFileName = "AIsManager";
	[HideInInspector]
	[System.NonSerialized]
	public string chestManagerFileName = "ChestManager";
	[HideInInspector]
	[System.NonSerialized]
	public string inventoryManagerFileName = "InventoryManager";
	[HideInInspector]
	[System.NonSerialized]
	public string plFileName = "Player";
	[HideInInspector]
	[System.NonSerialized]
	public string questManagerFileName = "QuestManager";
	[HideInInspector]
	[System.NonSerialized]
	public string squadrigliaManagerFileName = "SquadrigliaManager";
	[HideInInspector]
	[System.NonSerialized]
	public string shopFileName = "Shop";
	[HideInInspector]
	[System.NonSerialized]
	public string gameManagerFileName = "GameManager";
	[HideInInspector]
	[System.NonSerialized]
	public string impostazioniFileName = "Impostazioni";
	[HideInInspector]
	[System.NonSerialized]
	public string modificaBaseTriggerFileName = "ModificaBaseTrigger";

	[HideInInspector]
	[System.NonSerialized]
	public string iterateMultipleObjsFileName = "IterateMultipleObjs";
	[HideInInspector]
	[System.NonSerialized]
	public string playerBuildingBaseFileName = "PlayerBuildingBase"; 
}