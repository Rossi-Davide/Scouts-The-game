using UnityEngine;

public class ModificaBaseTrigger : MonoBehaviour
{
	public GameObject[] objectsToToggle;
	SnapToGridSpostamentoCostruzioni[] buildings;
	Camera cam;
	public Transform angolo;
	Transform player;
	bool isModifying = false;

	void Start()
	{
		cam = Camera.main;
		buildings = FindObjectsOfType<SnapToGridSpostamentoCostruzioni>();
		player = Player.instance.transform;
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
			b.enabled = isModifying;
			b.GetComponent<MoveBuildings>().enabled = isModifying;
			modificaAngolo.instance.enabled = isModifying;
			b.GetComponent<ObjectWithActions>().enabled = !isModifying;
		}
		cam.GetComponent<FollowPlayer>().SetTarget(isModifying ? angolo : player);
		cam.GetComponent<PanZoom>().enabled = !isModifying;
		cam.GetComponent<FollowPlayer>().EnableFollow();
		GetComponent<Animator>().Play(isModifying ? "SalvaEdEsci" : "Modifica");
	}
}
