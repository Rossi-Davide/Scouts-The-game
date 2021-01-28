using System;

public class Montana : InGameObject
{
	void Dance()
	{
		RefreshButtonsState();
	}

	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				return Dance;
			default:
				throw new NotImplementedException();
		}
	}

	protected override void DoActionOnStart(int buttonIndex) { }
}
