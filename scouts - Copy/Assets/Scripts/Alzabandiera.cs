using System;

public class Alzabandiera : InGameObject
{
	void FareAlzabandiera()
	{
		
	}
	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				return FareAlzabandiera;
			default:
				throw new NotImplementedException();
		}
	}

	protected override void DoActionOnStart(int buttonIndex) { }
}
