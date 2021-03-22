using UnityEngine;
using System;
using Pathfinding;

public class Plant : InGameObject
{
	LayerMask costruzioni;

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
		Player.instance.GetComponent<Animator>().SetBool("alberi", true);

		GetComponent<ParticleSystem>().Play();
	}

	void EndPunch()
	{
		GetComponent<ParticleSystem>().Stop();
		Player.instance.enabled = true;


		Player.instance.GetComponent<Animator>().SetBool("alberi",false);




		Deselect();
		Destroy(gameObject);
		Destroy(clickListener.gameObject);
	}

	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				return EndPunch;
			default:
				throw new NotImplementedException();
		}
	}

	protected override void DoActionOnStart(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				PlayerHandPunch();
				break;
		}
	}

	protected override  void Start()
	{
		costruzioni = LayerMask.GetMask("costruzioni");
		base.Start();
		CheckColl();
		//UpdatePath();
		
	}
	void UpdatePath()
	{
		var graphToScan = AstarPath.active.data.gridGraph;
		AstarPath.active.Scan(graphToScan);
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


        foreach (Collider2D c in coll2)
        {
            if (c.gameObject.layer==costruzioni)
            {
				Destroy(gameObject);
				Destroy(clickListener.gameObject);
				GameManager.instance.BuildingChanged();
			}
        }
	}
}
