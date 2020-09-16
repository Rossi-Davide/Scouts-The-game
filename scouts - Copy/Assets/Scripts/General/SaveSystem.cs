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
		
	}

	#region Save and load all
	public void SaveAll()
	{
		jsonCurrentTimeActions = JsonUtility.ToJson(currentTimeActions);
	}

	public void LoadAll()
	{
		currentTimeActions = JsonUtility.FromJson<CurrentTimeActions>(jsonCurrentTimeActions);
	}

	void GetInfo()
	{
		var timeActionArray = new TimeAction[ActionManager.instance.currentActions.Length];
		for (int i = 0; i < ActionManager.instance.currentActions.Length; i++)
			timeActionArray[i] = ActionManager.instance.currentActions[i];
		currentTimeActions = new CurrentTimeActions(timeActionArray);
	}

	#endregion
	public CurrentTimeActions currentTimeActions;
	public string jsonCurrentTimeActions;


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

}
public class CurrentAppSettings
{

}
public class CurrentCampSettings
{

}
public class CurrentSquadriglie
{

}
public class CurrentObjects
{
	//All InGameObjects scripts
}
public class CurrentPLayerValues
{
	//inventories
	//playerActions
	//quests
	//materials, points, energy
}