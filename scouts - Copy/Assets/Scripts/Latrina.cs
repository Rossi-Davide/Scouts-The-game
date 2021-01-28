using System;

public class Latrina : InGameObject
{
	void Pulisci()
	{
		RefreshButtonsState();
	}
	void Usa()
	{
		Player.instance.gameObject.SetActive(false);
	}
	void EndUsa()
	{
		Player.instance.gameObject.SetActive(true);
	}
	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				return Pulisci;
			case 2:
				return EndUsa;
			default:
				throw new NotImplementedException();
		}
	}

	protected override void DoActionOnStart(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1: break;
			case 2:
				Usa();
				break;
		}
	}
}
