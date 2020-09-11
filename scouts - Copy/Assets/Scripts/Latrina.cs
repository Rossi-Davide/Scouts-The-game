using System.Collections;
using UnityEngine;
using System;

public class Latrina : ObjectWithActions
{
	[HideInInspector]
	public bool canCleanLatrina;

	protected override void Start()
	{
		base.Start();
		canCleanLatrina = true;
	}
	void Pulisci()
	{
		canCleanLatrina = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
	}

	private void OnWaitEnd()
	{
		canCleanLatrina = true;
	}

	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanCleanLatrina: return canCleanLatrina;
			default: return base.GetConditionValue(t);
		}
	}
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, Pulisci);
				ChangeCounter(1);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
