using UnityEngine;
using System;

public class Montana : ObjectWithActions
{
	[HideInInspector]
	public bool canDance;

	protected override void Start()
	{
		base.Start();
		canDance = true;
	}
	void Dance()
	{
		canDance = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
	}

	private void OnWaitEnd()
	{
		canDance = true;
	}

	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanDance: return canDance;
			default: return base.GetConditionValue(t);
		}
	}


	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, Dance);
				ChangeCounter(1);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
