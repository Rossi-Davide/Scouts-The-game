using UnityEngine;

public class MoveBuildings : MonoBehaviour
{
	bool isTouching;
	[HideInInspector] [System.NonSerialized]
	public bool componentEnabled, isBeingBuilt;

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "oggSquadriglia1" )
		{
			isTouching = true;
			GameManager.instance.WarningOrMessage("Non puoi piazzare l'oggetto qui!", true);
		}
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
		if (collision.tag == "oggSquadriglia1")
		{
			isTouching = false;
			GameManager.instance.ClearWarningOrMessage();
		}
	}
	public void ResetPos(Vector3 startPos)
	{
		Debug.LogError(gameObject.name);
		transform.position = startPos;
		//gameObject.GetComponent<accroccoCorreggiZ>().Aggiustino();
		/*if (isBeingBuilt)
		{
			gameObject.GetComponent<Collider2D>().enabled = false;
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			ModificaBaseTrigger.instance.buildingSlot.gameObject.SetActive(true);
		}*/
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
