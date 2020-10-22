using UnityEngine;

public class AIsManager : MonoBehaviour
{
	public int maxActiveAIs;
	
	public CapieCambu[] allCapiECambu;
	[HideInInspector]
	public Squadrigliere[] allSquadriglieri;
	public GameObject AIContainer;

	public AIEvent[] events;

	#region Singleton
	public static AIsManager instance;
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("AIsManager non è un singleton!");
		instance = this;
	}
	#endregion

	void SetActiveOrInactiveAI()
	{
		foreach (var sq in allSquadriglieri)
		{
			if (sq.sq != Player.instance.squadriglia && GetProbability(40))
			{
				sq.ForceTarget("Tenda"); //set inactive
			}
		}
	}

	bool GetProbability(int percentage)
	{
		return Random.Range(1, 101) <= percentage;
	}
}