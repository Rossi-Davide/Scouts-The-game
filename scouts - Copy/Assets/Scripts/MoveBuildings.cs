using UnityEngine;

public class MoveBuildings : MonoBehaviour
{
	[HideInInspector] [System.NonSerialized]
	public bool isTouching,insideBorders;
	[HideInInspector] [System.NonSerialized]
	public bool componentEnabled, isBeingBuilt;
	LayerMask punti;
	bool switchV=false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "oggSquadriglia1" )
		{
			isTouching = true;
			GameManager.instance.WarningOrMessage("Non puoi piazzare l'oggetto qui!", true);
		}

		if (collision.name == "con")
		{
			insideBorders = true;
			Debug.LogError(insideBorders);
		}
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
		if (collision.tag == "oggSquadriglia1")
		{
			isTouching = false;
			GameManager.instance.ClearWarningOrMessage();
		}



        if (collision.name == "con")
        {
            if (switchV)
            {
				switchV = !switchV;
            }
            else
            {
				insideBorders = false;
				switchV = !switchV;
				Debug.LogError(insideBorders);
				GameManager.instance.WarningOrMessage("Sei fuori dai bordi dell'angolo", true);
			}
			
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
		if (isTouching||!insideBorders)
		{
			ResetPos(startPos);
			insideBorders = true;
			switchV = false;
		}
		GetComponent<InGameObject>().MoveUI();
	}

	Vector3 posIniziale;

	public Vector3 SearchForPos(string oggetto)
    {
		//ricerca della propria base
		punti = LayerMask.GetMask("posizioneOggetti");
		bool objectFound = false;
		posIniziale.z = 0;
		oggetto = oggetto.ToLower();
        if (oggetto == "tent")
        {
			oggetto = "tenda";
        }
		//Debug.Log(oggetto);
		GameObject posAngoloPlayer = SquadrigliaManager.instance.GetPlayerSq().angolo.gameObject;

		Vector2 posV2;
		posV2.x = posAngoloPlayer.transform.position.x;
		posV2.y = posAngoloPlayer.transform.position.y;
		Collider2D[] array = Physics2D.OverlapCircleAll(posV2, 10f,punti);

		foreach(Collider2D c in array)
        {
			//Debug.Log(c.name);
            if (c.name == oggetto)
            {
				posIniziale.x = c.transform.position.x;
				posIniziale.y = c.transform.position.y;
				objectFound = true;
				break;
			}
		}

		//Debug.Log(objectFound);
		

		//controllo che la base non sia occupata con esclusione oggetto corrente
		posV2.x = posIniziale.x;
		posV2.y = posIniziale.y;

		punti = LayerMask.GetMask("Default");
		Collider2D[] arrayCheckColl= Physics2D.OverlapCircleAll(posV2, 1f,punti);

		string nomeOggettoAttuale = oggetto + "(clone)";
		bool checkObjColl = false;

		foreach (Collider2D c in arrayCheckColl)
        {
			string name = c.name.ToLower();
            if (name != nomeOggettoAttuale)
            {
				checkObjColl = true;
            }
		}
        if (arrayCheckColl.Length > 0&&checkObjColl)
        {
			//Debug.Log("entered");
			objectFound = false;
        }


		//ricerca di una nuova base 
        if (!objectFound)
		{
			bool ok;
			do
			{
				ok = true;

				posIniziale.x = Random.Range(posAngoloPlayer.transform.position.x - 10, posAngoloPlayer.transform.position.x + 10);
				posIniziale.y = Random.Range(posAngoloPlayer.transform.position.y - 7, posAngoloPlayer.transform.position.y + 6);


				//controllo che non sia occupata con esclusione oggetto corrente 
				Vector2 ricerca;
				ricerca.x = posIniziale.x;
				ricerca.y = posIniziale.y;


				punti = LayerMask.GetMask("Default");
				Collider2D[] ar = Physics2D.OverlapCircleAll(ricerca, 1f, punti);
				checkObjColl = false;
				foreach (Collider2D c in ar)
				{
					string name = c.name.ToLower();
					if (name != nomeOggettoAttuale)
					{
						checkObjColl = true;
					}
				}
				if (ar.Length > 0&&checkObjColl)
				{
					ok = false;
				}
			} while (!ok);
			
		}
		return posIniziale;
    }
}
