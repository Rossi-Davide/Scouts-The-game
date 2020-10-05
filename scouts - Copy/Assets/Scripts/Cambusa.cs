using System;
public class Cambusa : ObjectWithActions
{
	void Attack()
	{
		RefreshButtonsState();
		StartWaitToUseAgain(buttons[1]);
	}
	protected override Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				ChangeCounter(2);
				return Attack;
			default:
				throw new NotImplementedException();
		}
	}
}
