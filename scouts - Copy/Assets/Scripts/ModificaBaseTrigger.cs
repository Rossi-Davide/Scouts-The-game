using System.Collections;
using UnityEngine;

public class ModificaBaseTrigger : MonoBehaviour
{
	public GameObject[] objectsToToggle;
	MoveBuildings[] buildings;
	Camera cam;
	public Transform angolo;
	Transform player;
	bool isModifying;

	void Start()
	{
		cam = Camera.main;
		StartCoroutine(GetSq());
		player = Player.instance.transform;
	}
	IEnumerator GetSq()
	{
		var frame = new WaitForEndOfFrame();
		yield return frame;
		yield return frame;
		yield return frame;

		PlayerBuildingBase[] bArray = SquadrigliaManager.instance.GetPlayerSq().buildings;
		buildings = new MoveBuildings[bArray.Length];
		for (int b = 0; b < buildings.Length; b++)
		{
			buildings[b] = bArray[b].GetComponent<MoveBuildings>();
		}
	}

	public void ToggleModificaBase()
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

		isModifying = !isModifying;
		foreach (GameObject g in objectsToToggle)
		{
			g.SetActive(!isModifying);
		}
		foreach (var b in buildings)
		{
			if (b.gameObject.activeSelf)
			{
				b.enabled = isModifying;
				b.isEnabled = isModifying;
				modificaAngolo.instance.enabled = isModifying;
				b.GetComponent<SnapToGridSpostamentoCostruzioni>().enabled = true;
				b.GetComponent<ObjectWithActions>().enabled = !isModifying;
			}
		}
		cam.GetComponent<FollowPlayer>().SetTarget(isModifying ? angolo : player);
		cam.GetComponent<PanZoom>().enabled = !isModifying;
		cam.GetComponent<FollowPlayer>().EnableFollow();
		if (ActionButtons.instance.selected != null)
		{
			ActionButtons.instance.selected.Deselect();
		}
		GetComponent<Animator>().Play(isModifying ? "SalvaEdEsci" : "Modifica");
	}
}
