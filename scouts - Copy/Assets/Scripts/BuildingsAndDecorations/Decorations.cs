using System.Collections;
using UnityEngine;
using System;

public class Decorations : ObjectWithActions
{
	private bool staFacendoLegna;
	public IEnumerator PlayerHandPunch()
	{
		staFacendoLegna = true;
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
		loadingBar.GetComponent<TimeLeftBar>().totalTime = 3;
		loadingBar.SetActive(true);
		yield return new WaitForSeconds(3f);
		GetComponent<ParticleSystem>().Stop();
		Player.instance.GetComponent<Animator>().Play("idle");
		Player.instance.enabled = true;
		GameManager.instance.ChangeCounter(GameManager.Counter.Materiali, 15);
		GameManager.instance.spawnedDecorations.Remove(gameObject);
		Deselect();
		Destroy(gameObject);
		staFacendoLegna = false;
	}

	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionStaFacendoLegna: return staFacendoLegna;
			default: return base.GetConditionValue(t);
		}
	}
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				StartCoroutine(PlayerHandPunch());
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
