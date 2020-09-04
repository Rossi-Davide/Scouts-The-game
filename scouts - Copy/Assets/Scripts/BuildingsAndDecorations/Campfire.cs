using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;
using System;

public class Campfire : ObjectWithActions
{

	[HideInInspector]
	public bool puoFareLegna;


    void ChangeLight()
    {
        if (!GameManager.instance.isDay)
		{
            GetComponent<Light2D>().enabled = true;
		}
        else
		{
            GetComponent<Light2D>().enabled = false;
		}
    }
	

	protected override void Start()
	{
		base.Start();
		puoFareLegna = true;
		GameManager.instance.OnDayEndOrStart += ChangeLight;
	}
	IEnumerator FaiLegna()
	{
		RefreshButtonsState();
		GameManager.instance.ChangeCounter(GameManager.Counter.Punti, 10);
		GameManager.instance.ChangeCounter(GameManager.Counter.Materiali, -100);
		puoFareLegna = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
		yield return new WaitForEndOfFrame();
	}

	private void OnWaitEnd()
	{
		puoFareLegna = true;
	}


	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionPuoFareLegnaPerFuoco: return puoFareLegna;
			default: return base.GetConditionValue(t);
		}
	}
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 0:
				StartCoroutine(FaiLegna());
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
