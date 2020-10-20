using UnityEngine;

public class CassaDelFurfante : InGameObject
{
	void SbloccaScreen()
	{
		Shop.instance.negozioIllegaleUnlocked = true;
	}


	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanUnlockNegozioDelFurfante: return !Shop.instance.negozioIllegaleUnlocked;
			default: return base.GetConditionValue(t);
		}
	}

	protected override System.Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				SbloccaScreen();
				return null;
			default:
				throw new System.NotImplementedException();
		}
	}
}
