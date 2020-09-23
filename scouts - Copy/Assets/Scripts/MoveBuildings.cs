using UnityEngine;

public class MoveBuildings : MonoBehaviour
{
	bool isTouching;
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "oggSquadriglia1")
		{
			isTouching = true;
			GameManager.instance.WarningMessage("Non puoi piazzare l'oggetto qui!");
		}
	}
	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "oggSquadriglia1")
		{
			isTouching = false;
			GameManager.instance.WarningMessage("");
		}
	}

	public void OnEndDragging(Vector3 startPos)
	{
		if (isTouching)
		{
			transform.position = startPos;
		}
		MoveUI();
	}
	void MoveUI()
	{
		BuildingsActionsAbstract b = GetComponent<BuildingsActionsAbstract>();
		b.healthBar.transform.position = transform.position + b.healthBarOffset;
		b.loadingBar.transform.position = transform.position + b.loadingBarOffset;
		b.nameText.transform.position = transform.position + b.nameTextOffset;
		b.instanceOfListener.transform.position = transform.position + b.clickListenerOffset;
	}
}
