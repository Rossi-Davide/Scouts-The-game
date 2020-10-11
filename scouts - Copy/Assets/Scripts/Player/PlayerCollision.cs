using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
	private GameObject waterParticle;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Collider2D>().name == "Water")
		{
			waterParticle = transform.Find("WaterParticle").gameObject;
			waterParticle.SetActive(true);
		}
	}
	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<Collider2D>().name == "Water")
		{
			GameObject waterParticle = transform.Find("WaterParticle").gameObject;
			waterParticle.SetActive(false);
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("entrato");
	}
}
