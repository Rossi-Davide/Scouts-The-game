using UnityEngine;

public class SaveSystem : MonoBehaviour
{
	public static SaveSystem instance;
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("Savesystem is not a singleton");
		instance = this;
	}

	public void SaveSettings()
	{
		
	}


	public void RefreshStuff()
	{

	}
}
