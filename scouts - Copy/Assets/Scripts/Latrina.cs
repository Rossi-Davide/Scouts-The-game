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
		GameManager.instance.ChangeCounter(GameManager.Counter.Punti, 5);
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -5);
		canCleanLatrina = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
	}

	private void OnWaitEnd()
	{
		canCleanLatrina = true;
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
			return "Pulizia latrina";
		}
		else
			throw new System.NotImplementedException();
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
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
