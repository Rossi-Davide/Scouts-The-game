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
		GameManager.instance.ChangeCounter(GameManager.Counter.Punti, 5);
		puoFareAlzabandiera = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
	}

	private void OnWaitEnd()
	{
		puoFareAlzabandiera = true;
	}


	protected override int GetTime(int buttonNum)
	{
		if (buttonNum == 1)
		{
			return 10;
		}
		else
			throw new System.NotImplementedException();
	}

	protected override string GetActionName(int buttonNum)
	{
		if (buttonNum == 1)
		{
			return "Alzabandiera";
		}
		else
			throw new System.NotImplementedException();
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
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
