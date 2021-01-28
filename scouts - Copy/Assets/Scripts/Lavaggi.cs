using System;

public class Lavaggi : InGameObject
{
	void LavaIPanni()
	{
		RefreshButtonsState();
	}
	void LavaIPiatti()
	{
		RefreshButtonsState();
	}

	void Pulisci()
	{
		RefreshButtonsState();
	}

	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				return LavaIPanni;
			case 2:
				return LavaIPiatti;
			case 3:
				return Pulisci;
			default:
				throw new NotImplementedException();
		}
	}

	protected override void DoActionOnStart(int buttonIndex) { }
}
