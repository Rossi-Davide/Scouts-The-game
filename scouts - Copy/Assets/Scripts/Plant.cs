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
		GameManager.instance.spawnedDecorations.Remove(gameObject);
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
}
