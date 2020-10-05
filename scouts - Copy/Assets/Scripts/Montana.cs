using System;

public class Montana : ObjectWithActions
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
				ChangeCounter(1);
				return Dance;
			default:
				throw new NotImplementedException();
		}
	}
}
