using UnityEngine;

public class modificaAngolo : MonoBehaviour
{
	Camera cam;
	Vector3 touchPosWorld;
	[HideInInspector]
	public bool collisione = false;
	Vector3 posizioneIniziale;

	public static modificaAngolo instance;
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("Modifica Angolo non è un singleton");
		instance = this;
	}


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
			if (hitInformation.collider != null && hitInformation.transform.tag == "oggSquadriglia1" && oggetto == hitInformation.transform || oggetto == null)
			{
				if (touch.phase == TouchPhase.Began)
				{
					oggetto = hitInformation.transform;
					posizioneIniziale = oggetto.position;
				}
				if (touch.phase == TouchPhase.Moved)
				{
					Vector2 posizioneDito = touch.position;
					Vector3 posizioneOggetto = cam.ScreenToWorldPoint(posizioneDito);
					oggetto.position = new Vector3(posizioneOggetto.x, posizioneOggetto.y, 0);
				}
				if (touch.phase == TouchPhase.Ended)
				{
					if (collisione)
					{
						oggetto.GetComponent<MoveBuildings>().ResetPosition(posizioneIniziale);
					}
					oggetto.GetComponent<MoveBuildings>().MoveUI();
					oggetto = null;
				}
			}
		}
	}
}
