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

	public override System.Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				return null;
			default:
				throw new System.NotImplementedException();
		}
	}
	protected override void DoActionOnStart(int buttonIndex) 
	{
		switch (buttonIndex + 1)
		{
			case 1: 
				SbloccaScreen();
				break;
		}
	}
}
