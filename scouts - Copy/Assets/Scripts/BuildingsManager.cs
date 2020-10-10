using UnityEngine;
using TMPro;

public class BuildingsManager : MonoBehaviour
{
	ConcreteSquadriglia playerSq;


	#region Singleton
	public static BuildingsManager instance;
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("BuildingsManager non è un singleton");
		instance = this;
	}
	#endregion


	

}
