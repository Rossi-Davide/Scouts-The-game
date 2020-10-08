using UnityEngine;

public class modificaAngolo : MonoBehaviour
{
	Camera cam;
	Vector3 touchPosWorld;
	[HideInInspector]
	Vector3 posizioneIniziale;

	#region Singleton
	public static modificaAngolo instance;
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("Modifica Angolo non è un singleton");
		instance = this;
	}
	#endregion

	Transform oggetto;
	void Start()
	{
		cam = Camera.main;
	}

	void Update()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			touchPosWorld = Camera.main.ScreenToWorldPoint(touch.position);
			Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
			RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
			if (hitInformation.collider != null && hitInformation.collider.transform != null && (hitInformation.transform.tag == "oggSquadriglia1" && (oggetto == null || oggetto == hitInformation.collider.transform)))
			{
				if (touch.phase == TouchPhase.Began)
				{
					oggetto = hitInformation.collider.transform;
					posizioneIniziale = oggetto.position;
				}
				if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
				{
					Vector2 posizioneDito = touch.position;
					Vector3 posizioneOggetto = cam.ScreenToWorldPoint(posizioneDito);
					oggetto.position = new Vector3(posizioneOggetto.x, posizioneOggetto.y, 0);
				}
				if (touch.phase == TouchPhase.Ended)
				{
					oggetto.GetComponent<MoveBuildings>().OnEndDragging(posizioneIniziale);
					oggetto = null;
				}
				if (touch.phase == TouchPhase.Canceled)
				{
					oggetto.GetComponent<MoveBuildings>().ResetPos(posizioneIniziale);
					oggetto = null;
				}
			}
		}
	}
}
