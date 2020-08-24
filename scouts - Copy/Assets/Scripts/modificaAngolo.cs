using UnityEngine;

public class modificaAngolo : MonoBehaviour
{
	Camera cam;
	Vector3 touchPosWorld;
	bool collisione = false;
	public GameObject alert;
	Vector3 posizioneIniziale;

	void Start()
	{
		cam = Camera.main;
		posizioneIniziale = transform.position;
	}

	void Update()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			touchPosWorld = Camera.main.ScreenToWorldPoint(touch.position);
			Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
			RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
			if (hitInformation.collider != null && hitInformation.collider == GetComponent<BoxCollider2D>())
			{
				if (touch.phase == TouchPhase.Moved)
				{
					Vector2 posizioneDito = touch.position;
					Vector3 posizioneOggetto = cam.ScreenToWorldPoint(posizioneDito);
					transform.position = new Vector3(posizioneOggetto.x, posizioneOggetto.y, 0);
				}
				if (touch.phase == TouchPhase.Ended)
				{
					if (collisione)
						ResetPosition();
					else
						posizioneIniziale = transform.position;
				}
			}
		}
	}


	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "oggSquadriglia1")
		{
			collisione = true;
			GameManager.instance.WarningMessage("Non puoi piazzare l'oggetto qui!");
		}
	}
	public void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "oggSquadriglia1")
		{
			collisione = false;
			GameManager.instance.WarningMessage("");
		}
	}

	public void ResetPosition()
	{
		if (collisione == true)
		{
			transform.position = posizioneIniziale;
		}
	}
}
