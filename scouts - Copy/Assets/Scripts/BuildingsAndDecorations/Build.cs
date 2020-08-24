using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
	#region Singleton 
	public static Build instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("Build non è un singleton: doppia instance");
		}
		instance = this;
	}
	#endregion
	private Item currentBuilding;
	private int time, points;
	public Camera mainCam, buildCam;
	public Vector2[] startAngoloSq, endAngoloSq;
	private Dictionary<Item, BuildingInfo> builtBuildings = new Dictionary<Item, BuildingInfo>();
	public void EnterBuildMode(Item building, int time, int points)
	{
		if (currentBuilding != null)
		{
			return;
		}
		currentBuilding = building;
		this.time = time;
		this.points = points;
		mainCam.enabled = false;
		buildCam.enabled = true;
	}
	void Save()
	{
		if (currentBuilding != null)
		{
			builtBuildings.Add(currentBuilding, new BuildingInfo { position = new Vector2(1, 0), timeRequired = time, pointsGiven = points });
			mainCam.enabled = true;
			buildCam.enabled = false;
		}
	}
	public bool HasBuilt(Item building) => builtBuildings.ContainsKey(building);
	public enum Buildings
	{
		tenda, 
		refettorio, 
		stendipanni, 
		portalegna, 
		pianoBidoni, 
		amaca, 
		cassaDiPioneristica, 
		cassaDiInfermieristica, 
		cassaDiCucina, 
		cassaDiTopografia, 
		cassaDiEspressione, 
		cassaDelFurfante
	}
}
public class BuildingInfo
{
	public int timeRequired;
	public int pointsGiven;
	public Vector2 position;
}
