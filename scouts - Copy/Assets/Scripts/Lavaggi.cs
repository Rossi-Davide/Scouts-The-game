using System;

public class Lavaggi : InGameObject
{
	void LavaIPanni()
	{
		RefreshButtonsState();
	}
	void LavaIPiatti()
	{
		RefreshButtonsState();
	}

	void Pulisci()
	{
		RefreshButtonsState();
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
				return LavaIPanni;
			case 2:
				return LavaIPiatti;
			case 3:
				return Pulisci;
			default:
				throw new NotImplementedException();
		}
	}
}
