using System;
using UnityEngine;

public class modificaAngolo : MonoBehaviour
{
	Camera cam;
	Vector3 touchPosWorld;
	[HideInInspector] [System.NonSerialized]
	public Vector3 posizioneIniziale;
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
	[HideInInspector] [System.NonSerialized]
	public Rigidbody2D rb;
	[HideInInspector] [System.NonSerialized]
	public Transform oggetto;
	[HideInInspector] [System.NonSerialized]
	public bool firstIteraction;
	public string objectBought;
	public GameObject spawnPoints;

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
					posizioneIniziale = slot.buildingParent.transform.position;
					//slot.buildingParent.GetComponent<Collider2D>().enabled = true;
					//slot.buildingParent.GetComponent<SpriteRenderer>().enabled = true;
					slot.buildingParent.transform.position = cam.ScreenToWorldPoint(touch.position);
                    
					oggetto = slot.buildingParent.transform;
					UpdateRayCast(touch);
				}
				if (touch.phase == TouchPhase.Began)
				{
					ModificaBaseTrigger.instance.ToggleButtonComponent(true);
					ModificaBaseTrigger.instance.execTransition = false;

					try
					{
						oggetto = hitInformation.collider.transform;					}
					catch
					{
						Debug.LogError("errore dovuto al trascinamento di oggetto appena comprato");
					}
					if (!firstIteraction)
					{
						posizioneIniziale = oggetto.position;
					}
					else
					{
						//settare la posizione dei puntini
						posizioneIniziale = slot.buildingParent.GetComponent<MoveBuildings>().SearchForPos(objectBought);
						Debug.Log(posizioneIniziale);
						firstIteraction = false;
						SaveSystem.instance.SaveData(ModificaBaseTrigger.instance.SendStatus(), SaveSystem.instance.modificaBaseTriggerFileName, false);
						//Debug.Log("called modifica angolo");
						spawnPoints.SetActive(false);
					}
				}
				else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
				{
					Debug.Log("moved");
					rb = hitInformation.collider.attachedRigidbody;
					Vector2 posizioneDito = touch.position;
					Vector3 posizioneOggetto = cam.ScreenToWorldPoint(posizioneDito);
					rb.position =new Vector3(posizioneOggetto.x,posizioneOggetto.y,0);
					//oggetto.position = SnapToGrid(posizioneOggetto);
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


	//float grid = 0.8f;
	//Vector3 SnapToGrid(Vector3 pos)
	//{
	//	float reciprocalGrid = 1f / grid;
	//	float x = Mathf.Round(oggetto.position.x * reciprocalGrid) / reciprocalGrid;
	//	float y = Mathf.Round(oggetto.position.y * reciprocalGrid) / reciprocalGrid;
	//	return new Vector3(x, y, 0);
	//}


	void UpdateRayCast(Touch t)
	{
		touchPosWorld = Camera.main.ScreenToWorldPoint(t.position);
		touchPosWorld2D.x = touchPosWorld.x;
		touchPosWorld2D.y = touchPosWorld.y;
		hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
	}
}
