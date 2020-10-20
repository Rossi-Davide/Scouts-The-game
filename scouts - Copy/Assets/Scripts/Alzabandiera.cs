using System;

public class Alzabandiera : InGameObject
{
	void FareAlzabandiera()
	{
		
	}
	protected override Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				return FareAlzabandiera;
			default:
				throw new NotImplementedException();
		}
	}
}
