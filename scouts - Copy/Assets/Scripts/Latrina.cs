using System;

public class Latrina : InGameObject
{
	void Pulisci()
	{
		RefreshButtonsState();
	}
	protected override Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				return Pulisci;
			default:
				throw new NotImplementedException();
		}
	}
}
