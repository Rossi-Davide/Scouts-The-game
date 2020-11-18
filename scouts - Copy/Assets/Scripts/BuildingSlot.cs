using UnityEngine;

public class BuildingSlot : MonoBehaviour
{
	[HideInInspector] [System.NonSerialized]
    public PlayerBuilding building;
	[HideInInspector] [System.NonSerialized]
    public PlayerBuildingBase buildingParent;
	int touchRadius = 80;

	public bool CheckIfNearTouch(Touch t)
	{
		return Vector2.Distance(t.position, transform.position) <= touchRadius && gameObject.activeSelf;
	}

}
