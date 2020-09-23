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




	#endregion

	#region Save and load all
	public void SaveAll()
	{
		currentTimeActions = new CurrentTimeActions(GetTimeActions());
		jsonCurrentTimeActions = JsonUtility.ToJson(currentTimeActions);

		currentAppSettings = GetAppSettings();
		jsonCurrentAppSettings = JsonUtility.ToJson(currentAppSettings);
	}

	public void LoadAll()
	{
		currentTimeActions = JsonUtility.FromJson<CurrentTimeActions>(jsonCurrentTimeActions);
		currentAppSettings = JsonUtility.FromJson<CurrentAppSettings>(jsonCurrentAppSettings);
	}

	#endregion
	public CurrentTimeActions currentTimeActions;
	public string jsonCurrentTimeActions;

	public CurrentAppSettings currentAppSettings;
	public string jsonCurrentAppSettings;


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
public class CurrentCampSettings
{
	public Camp camp;
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
	//actions
	//materials, points, energy
}
public class CurrentAIs
{
	//capi e cambu
		//dialoguesDone
	//squadriglieri con info
}