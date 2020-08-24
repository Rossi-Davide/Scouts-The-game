using System.Collections;
using System;
using UnityEngine;

public class Alzabandiera : ObjectWithActions
{
	[HideInInspector]
	public bool staFacendoAlzabandiera, puoFareAlzabandiera;

	protected override void Start()
	{
		base.Start();
		puoFareAlzabandiera = true;
	}
	IEnumerator FareAlzabandiera()
	{
		staFacendoAlzabandiera = true;
		RefreshButtonsState();
		loadingBar.GetComponent<TimeLeftBar>().totalTime = 10;
		loadingBar.SetActive(true);
		yield return new WaitForSeconds(10);
		GameManager.instance.ChangeCounter(GameManager.Counter.Punti, 5);
		staFacendoAlzabandiera = false;
		puoFareAlzabandiera = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
	}

	private void OnWaitEnd()
	{
		puoFareAlzabandiera = true;
	}


	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionStaFacendoAlzabandiera: return staFacendoAlzabandiera;
			case ConditionType.ConditionPuoFareAlzabandiera: return puoFareAlzabandiera;
			default: return base.GetConditionValue(t);
		}
	}
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 0:
				StartCoroutine(FareAlzabandiera());
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
