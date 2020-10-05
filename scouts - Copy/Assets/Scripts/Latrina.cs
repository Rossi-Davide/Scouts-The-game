using System;

public class Latrina : ObjectWithActions
{
	void Pulisci()
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
				return Pulisci;
			default:
				throw new NotImplementedException();
		}
	}
}
