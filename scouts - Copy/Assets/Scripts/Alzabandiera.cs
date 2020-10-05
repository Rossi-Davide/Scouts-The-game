using System;

public class Alzabandiera : ObjectWithActions
{
	void FareAlzabandiera()
	{
		StartWaitToUseAgain(buttons[0]);
	}
	protected override Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				ChangeCounter(1);
				return FareAlzabandiera;
			default:
				throw new NotImplementedException();
		}
	}
}
