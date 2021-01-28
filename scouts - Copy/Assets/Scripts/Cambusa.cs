using System;
public class Cambusa : InGameObject
{
	void Attack()
	{
		RefreshButtonsState();
	}
	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				return Attack;
			default:
				throw new NotImplementedException();
		}
	}
	protected override void DoActionOnStart(int buttonIndex) { }
}
