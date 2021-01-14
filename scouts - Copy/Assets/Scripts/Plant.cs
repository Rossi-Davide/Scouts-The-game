using UnityEngine;
using System;

public class Plant : InGameObject
{
	void PlayerHandPunch()
	{
		Player.instance.enabled = false;
		if (Player.instance.transform.position.x >= transform.position.x)
		{
			Player.instance.GetComponent<Animator>().Play("leftHandPunch");
		}
		else
		{
			Player.instance.GetComponent<Animator>().Play("rightHandPunch");
		}
		GetComponent<ParticleSystem>().Play();
	}

	void EndPunch()
	{
		GetComponent<ParticleSystem>().Stop();
		Player.instance.GetComponent<Animator>().Play("idle");
		Player.instance.enabled = true;
		Deselect();
		Destroy(gameObject);
		Destroy(clickListener.gameObject);
	}

	protected override Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				PlayerHandPunch();
				return EndPunch;
			default:
				throw new NotImplementedException();
		}
	}

	protected override  void Start()
	{
		base.Start();
		CheckColl();
		
	}


	void CheckColl()
    {
		Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, 10f);
		foreach (Collider2D c in coll)
		{
			if (c.name == "Player")
			{
				Destroy(gameObject);
				Destroy(clickListener.gameObject);
				GameManager.instance.BuildingChanged();
			}
		}


		Collider2D[] coll2 = Physics2D.OverlapCircleAll(transform.position, 1f);

        if (coll2.Length > 0)
        {
			Destroy(gameObject);
			Destroy(clickListener.gameObject);
			GameManager.instance.BuildingChanged();
		}
	}
}
