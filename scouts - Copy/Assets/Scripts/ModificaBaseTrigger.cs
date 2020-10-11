using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ModificaBaseTrigger : MonoBehaviour
{
	public BuildingSlot buildingSlot;
	public GameObject[] objectsToToggle;
	MoveBuildings[] buildings;
	Camera cam;
	public Transform angolo;
	Transform player;
	bool isModifying;

	#region Singleton
	public static ModificaBaseTrigger instance;
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("ModificaBaseTrigger non è un singleton");
		instance = this;
	}
	#endregion




	void Start()
	{
		cam = Camera.main;
		StartCoroutine(GetSq());
		player = Player.instance.transform;
	}
	public void SetBuildingSlotInfo(PlayerBuilding building)
	{
		buildingSlot.GetComponent<Image>().sprite = building.icon;
		buildingSlot.building = building;
		foreach (var b in buildings)
		{
			var bg = b.GetComponent<PlayerBuildingBase>();
			if (bg.building == buildingSlot.building)
			{
				buildingSlot.buildingParent = bg;
			}
			b.GetComponent<MoveBuildings>().isBeingBuilt = false;
		}
		buildingSlot.buildingParent.GetComponent<MoveBuildings>().isBeingBuilt = true;
		ToggleModificaBase(true);
	}
	IEnumerator GetSq()
	{
		var frame = new WaitForEndOfFrame();
		yield return frame;
		yield return frame;
		yield return frame;

		SpriteRenderer[] bArray = SquadrigliaManager.instance.GetPlayerSq().buildings;
		buildings = new MoveBuildings[bArray.Length];
		for (int b = 0; b < buildings.Length; b++)
		{
			buildings[b] = bArray[b].GetComponent<MoveBuildings>();
		}
	}

	public void ToggleModificaBase(bool isBuilding)
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

		isModifying = !isModifying;
		buildingSlot.gameObject.SetActive(isBuilding);
		
		if (!isBuilding && buildingSlot.buildingParent != null)
		{
			buildingSlot.buildingParent.GetComponent<MoveBuildings>().isBeingBuilt = false;
			buildingSlot.buildingParent = null;
		}

		foreach (GameObject g in objectsToToggle)
		{
			g.SetActive(!isModifying);
		}
		foreach (var b in buildings)
		{
			if (buildingSlot.buildingParent != null && b.GetComponent<PlayerBuildingBase>() == buildingSlot.buildingParent)
			{
				b.componentEnabled = isModifying;
			}
			else if (b.gameObject.activeSelf)
			{
				b.MoveUI();
				b.componentEnabled = isModifying;
			}
		}
		modificaAngolo.instance.enabled = isModifying;
		cam.GetComponent<FollowPlayer>().SetTarget(isModifying ? angolo : player);
		cam.GetComponent<PanZoom>().canDo = !isModifying;
		cam.GetComponent<FollowPlayer>().EnableFollow();
		if (ActionButtons.instance.selected != null)
		{
			ActionButtons.instance.selected.Deselect();
		}
		GetComponent<Animator>().Play(isModifying ? "SalvaEdEsci" : "Modifica");
	}
}
