using System;

public class Lavaggi : ObjectWithActions
{
	void LavaIPanni()
	{
		RefreshButtonsState();
		StartWaitToUseAgain(buttons[0]);
	}
	void LavaIPiatti()
	{
		RefreshButtonsState();
		StartWaitToUseAgain(buttons[1]);
	}

	void Pulisci()
	{
		RefreshButtonsState();
		StartWaitToUseAgain(buttons[2]);
	}

	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCookBundle: return GetComponent<IterateMultipleObjs>().CheckAction(3, 1);
			default: return base.GetConditionValue(t);
		}
	}

	protected override Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				ChangeCounter(1);
				return LavaIPanni;
			case 2:
				ChangeCounter(2);
				return LavaIPiatti;
			case 3:
				ChangeCounter(3);
				return Pulisci;
			default:
				throw new NotImplementedException();
		}
	}
}
