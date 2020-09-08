using UnityEngine;

public class MoveBuildings : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "oggSquadriglia1")
		{
			modificaAngolo.instance.collisione = true;
			GameManager.instance.WarningMessage("Non puoi piazzare l'oggetto qui!");
		}
	}
	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "oggSquadriglia1")
		{
			modificaAngolo.instance.collisione = false;
			GameManager.instance.WarningMessage("");
		}
	}

	public void ResetPosition(Vector3 posizioneIniziale)
	{
		transform.position = posizioneIniziale;
	}


	public void MoveUI()
	{
		BuildingsActionsAbstract b = GetComponent<BuildingsActionsAbstract>();
		b.healthBar.transform.position = transform.position + b.healthBarOffset;
		b.loadingBar.transform.position = transform.position + b.loadingBarOffset;
		b.nameText.transform.position = transform.position + b.nameTextOffset;
		b.clickListener.transform.position = transform.position + b.clickListenerOffset;
	}
}
