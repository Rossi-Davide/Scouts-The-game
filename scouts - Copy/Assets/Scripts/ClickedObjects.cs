using UnityEngine;
using UnityEngine.EventSystems;

public class ClickedObjects : MonoBehaviour
{

	#region Singleton
	public static ClickedObjects instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("ClickedObjects non è un singleton");
		}
		instance = this;
	}
	#endregion
	
}
