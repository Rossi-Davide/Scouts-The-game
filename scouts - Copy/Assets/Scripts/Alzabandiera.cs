using System.Collections;
using System;
using UnityEngine;

public class Alzabandiera : ObjectWithActions
{
	[HideInInspector]
	public bool puoFareAlzabandiera;

	protected override void Start()
	{
		base.Start();
		puoFareAlzabandiera = true;
	}
	void FareAlzabandiera()
	{
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
			case ConditionType.ConditionPuoFareAlzabandiera: return puoFareAlzabandiera;
			default: return base.GetConditionValue(t);
		}
	}
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, FareAlzabandiera);
				ChangeCounter(1);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
