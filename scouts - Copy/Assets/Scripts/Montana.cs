using System;

public class Montana : InGameObject
{
	void Dance()
	{
		RefreshButtonsState();
		StartWaitToUseAgain(buttons[0]);
	}

	protected override Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				return Dance;
			default:
				throw new NotImplementedException();
		}
	}
}
