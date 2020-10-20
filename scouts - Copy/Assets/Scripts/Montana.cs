using System;

public class Montana : InGameObject
{
	void Dance()
	{
		RefreshButtonsState();
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
