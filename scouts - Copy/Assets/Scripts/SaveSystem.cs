using System;
using System.Collections;
using System.Data;
using UnityEngine;
public class SaveSystem : MonoBehaviour
{
	#region Singleton
	public static SaveSystem instance;
	private void Awake()
	{
		DontDestroyOnLoad(this);
		if (instance == null)
			instance = this;
	}
	#endregion

	private void Start()
	{
		InvokeRepeating(nameof(GetSaveAll), 5, 20);
	}
	private void OnApplicationQuit()
	{
		GetSaveAll();
	}

	public void DeleteAllFiles()
	{
		Array.ForEach(System.IO.Directory.GetFiles(Application.persistentDataPath), System.IO.File.Delete);
	}

	public void SaveData(object o, string fileName)
	{
		string path = Application.persistentDataPath + $"/{fileName}.json";
		var json = JsonUtility.ToJson(o);
		//Debug.Log($"SaveData: {json}");
		System.IO.File.WriteAllText(path, json);
	}
	public T LoadData<T>(string fileName)
	{
		try
		{
			string path = Application.persistentDataPath + $"/{fileName}.json";
			if (!System.IO.File.Exists(path))
			{
				return default;
			}
			var d = JsonUtility.FromJson<T>(System.IO.File.ReadAllText(path));
			//Debug.Log($"LoadData path: {path} Completed. The result is null: {d == null}. ");
			return d;
		}
		catch (System.Exception ex)
		{
			Debug.Log($"Error in LoadData path: {ex.Message}");
			return default;
		}
	}
	public event Action OnReadyToSaveData;
	public void GetSaveAll()
	{
		if (CampManager.instance != null && CampManager.instance.camp != null) { SaveData(CampManager.instance.SendStatus(), campManagerFileName); }
		if (SquadrigliaManager.instance != null) { SaveData(SquadrigliaManager.instance.SendStatus(), squadrigliaManagerFileName); }
		if (GameManager.instance != null) { SaveData(GameManager.instance.SendStatus(), gameManagerFileName); }
		if (Shop.instance != null) { SaveData(Shop.instance.SendStatus(), shopFileName); }
		if (ActionManager.instance != null) { SaveData(ActionManager.instance.SendStatus(), actionManagerFileName); }
		if (AIsManager.instance != null) { SaveData(AIsManager.instance.SendStatus(), aisManagerFileName); }
		if (ChestManager.instance != null) { SaveData(ChestManager.instance.SendStatus(), chestManagerFileName); }
		if (InventoryManager.instance != null) { SaveData(InventoryManager.instance.SendStatus(), inventoryManagerFileName); }
		if (Player.instance != null) { SaveData(Player.instance.SendStatus(), plFileName); }
		if (QuestManager.instance != null) { SaveData(QuestManager.instance.SendStatus(), questManagerFileName); }
		OnReadyToSaveData?.Invoke();
		Debug.Log("saved data");
	}


	public string campManagerFileName = "CampManager";
	public string actionButtonsFileName = "ActionButtons";
	public string actionManagerFileName = "ActionManager";
	public string aisManagerFileName = "AIsManager";
	public string chestManagerFileName = "ChestManager";
	public string inventoryManagerFileName = "InventoryManager";
	public string plFileName = "Player";
	public string questManagerFileName = "QuestManager";
	public string squadrigliaManagerFileName = "SquadrigliaManager";
	public string shopFileName = "Shop";
	public string gameManagerFileName = "GameManager";

	public string iterateMultipleObjsFileName = "IterateMultipleObjs";
	public string playerBuildingBaseFileName = "PlayerBuildingBase"; 
}