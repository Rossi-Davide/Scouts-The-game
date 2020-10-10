using UnityEngine;

public class BuildingSlot : MonoBehaviour
{
	[HideInInspector]
    public PlayerBuilding building;
	[HideInInspector]
    public PlayerBuildingBase buildingParent;
	int touchRadius = 80;

	public bool CheckIfNearTouch(Touch t)
	{
		return Vector2.Distance(t.position, transform.position) <= touchRadius && gameObject.activeSelf;
	}

}
