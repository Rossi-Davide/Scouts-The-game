using UnityEngine;
using System;

public class Montana : ObjectWithActions
{
	[HideInInspector]
	public bool isDancing, canDance;

	protected override void Start()
	{
		base.Start();
		canDance = true;
	}
	void Dance()
	{
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -5);
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


	protected override int GetTime(int buttonNum)
	{
		if (buttonNum == 1)
		{
			return 20;
		}
		else
			throw new System.NotImplementedException();
	}

	protected override string GetActionName(int buttonNum)
	{
		if (buttonNum == 1)
		{
			return "Discotenda";
		}
		else
			throw new System.NotImplementedException();
	}



	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, Dance);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
