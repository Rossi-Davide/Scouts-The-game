using System;
using UnityEngine.Timeline;

public class Refettorio : PlayerBuildingBase
{
	void Eat()
	{
		ChangeCounter(1);
		RefreshButtonsState();
	}

	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCookBundle: return GetComponent<IterateMultipleObjs>().CheckAction(2, 1);
			default: return base.GetConditionValue(t);
		}
	}

	protected override System.Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				return Eat;
			case 2:
				ChangeCounter(2);
				return MettiAlSicuro;
			case 3:
				ChangeCounter(3);
				return Ripara;
			default:
				throw new NotImplementedException();
		}
	}
}
