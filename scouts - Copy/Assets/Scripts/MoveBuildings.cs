using UnityEngine;

public class MoveBuildings : MonoBehaviour
{
	[HideInInspector] [System.NonSerialized]
	public bool isTouching;
	[HideInInspector] [System.NonSerialized]
	public bool componentEnabled, isBeingBuilt;
	LayerMask punti;

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

	Vector3 posIniziale;

	public Vector3 SearchForPos(string oggetto)
    {
		punti = LayerMask.GetMask("posizioneOggetti");
		bool objectFound = false;
		posIniziale.z = 0;
		oggetto = oggetto.ToLower();
		Debug.Log(oggetto);
		GameObject posAngoloPlayer = SquadrigliaManager.instance.GetPlayerSq().angolo.gameObject;

		Vector2 posV2;
		posV2.x = posAngoloPlayer.transform.position.x;
		posV2.y = posAngoloPlayer.transform.position.y;
		Collider2D[] array = Physics2D.OverlapCircleAll(posV2, 20f,punti);

		foreach(Collider2D c in array)
        {
			Debug.Log(c.name);
            if (c.name == oggetto)
            {
				posIniziale.x = c.transform.position.x;
				posIniziale.y = c.transform.position.y;
				objectFound = true;
				break;
			}
		}

		posV2.x = posIniziale.x;
		posV2.y = posIniziale.y;

		punti = LayerMask.GetMask("Default");
		Collider2D[] arrayCheckColl= Physics2D.OverlapCircleAll(posV2, 1f,punti);

        if (arrayCheckColl.Length > 0)
        {
			objectFound = false;
        }
		Debug.Log(objectFound);

        if (!objectFound)
		{
			bool ok;
			do
			{
				ok = true;

				posIniziale.x = Random.Range(posAngoloPlayer.transform.position.x - 7, posAngoloPlayer.transform.position.x + 8);
				posIniziale.y = Random.Range(posAngoloPlayer.transform.position.y - 7, posAngoloPlayer.transform.position.y + 8);

				Vector2 ricerca;
				ricerca.x = posIniziale.x;
				ricerca.y = posIniziale.y;


				punti = LayerMask.GetMask("Default");
				Collider2D[] ar = Physics2D.OverlapCircleAll(ricerca, 1f, punti);

				if (ar.Length > 0)
				{
					objectFound = false;
				}
			} while (!ok);
			
		}
		return posIniziale;
    }
}
