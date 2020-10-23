using UnityEngine;

public class MoveBuildings : MonoBehaviour
{
	bool isTouching;
	[HideInInspector]
	public bool componentEnabled, isBeingBuilt;
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "oggSquadriglia1" && componentEnabled)
		{
			isTouching = true;
			GameManager.instance.WarningOrMessage("Non puoi piazzare l'oggetto qui!", true);
		}
	}
	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "oggSquadriglia1" && componentEnabled)
		{
			isTouching = false;
			GameManager.instance.CleanWarningOrMessage();
		}
	}
	public void ResetPos(Vector3 startPos)
	{
		transform.position = startPos;
		if (isBeingBuilt)
		{
			gameObject.GetComponent<Collider2D>().enabled = false;
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			ModificaBaseTrigger.instance.buildingSlot.gameObject.SetActive(true);
		}
	}
	public void OnEndDragging(Vector3 startPos)
	{
		if (isTouching)
		{
			ResetPos(startPos);
		}
		GetComponent<InGameObject>().MoveUI();
	}
}
