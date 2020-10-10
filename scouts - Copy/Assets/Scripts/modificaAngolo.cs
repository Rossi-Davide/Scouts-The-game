using UnityEngine;

public class modificaAngolo : MonoBehaviour
{
	Camera cam;
	Vector3 touchPosWorld;
	Vector3 posizioneIniziale;
	Vector2 touchPosWorld2D;
	RaycastHit2D hitInformation;


	#region Singleton
	public static modificaAngolo instance;
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("Modifica Angolo non è un singleton");
		instance = this;
	}
	#endregion
	[HideInInspector]
	public Transform oggetto;
	void Start()
	{
		cam = Camera.main;
	}

	void Update()
	{
		var slot = ModificaBaseTrigger.instance.buildingSlot;
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			UpdateRayCast(touch);
			if ((hitInformation.collider != null && hitInformation.collider.transform != null && hitInformation.transform.tag == "oggSquadriglia1" && (oggetto == null || oggetto == hitInformation.collider.transform)) || slot.CheckIfNearTouch(touch))
			{
				if (slot.CheckIfNearTouch(touch))
				{
					slot.gameObject.SetActive(false);
					slot.buildingParent.gameObject.SetActive(true);
					slot.buildingParent.GetComponent<Collider2D>().enabled = true;
					slot.buildingParent.GetComponent<SpriteRenderer>().enabled = true;
					slot.buildingParent.transform.position = cam.ScreenToWorldPoint(touch.position);
					posizioneIniziale = slot.buildingParent.transform.position;
					oggetto = slot.buildingParent.transform;
					UpdateRayCast(touch);
				}
				else if (touch.phase == TouchPhase.Began)
				{
					
					oggetto = hitInformation.collider.transform;
					posizioneIniziale = oggetto.position;
				}
				if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
				{
					oggetto = hitInformation.collider.transform;
					Vector2 posizioneDito = touch.position;
					Vector3 posizioneOggetto = cam.ScreenToWorldPoint(posizioneDito);
					oggetto.position = new Vector3(posizioneOggetto.x, posizioneOggetto.y, 0);
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					oggetto.GetComponent<MoveBuildings>().OnEndDragging(posizioneIniziale);
					oggetto = null;
				}
				else if (touch.phase == TouchPhase.Canceled)
				{
					oggetto.GetComponent<MoveBuildings>().ResetPos(posizioneIniziale);
					oggetto = null;
				}
			}
		}
	}
	


	void UpdateRayCast(Touch t)
	{
		touchPosWorld = Camera.main.ScreenToWorldPoint(t.position);
		touchPosWorld2D.x = touchPosWorld.x;
		touchPosWorld2D.y = touchPosWorld.y;
		hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
	}
}
