using System;
public class Tent : PlayerBuildingBase
{
	void StartSleep()
	{
		Player.instance.gameObject.SetActive(false);
		Joystick.instance.enabled = false;
	}
	void EndOfSleep()
	{
		GameManager.instance.SkipNight();
		Player.instance.gameObject.SetActive(true);
		Joystick.instance.enabled = true;
		RefreshButtonsState();
	}
	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				
				return EndOfSleep;
			case 2:
				return MettiAlSicuro;
			case 3:
				return Ripara;
			default:
				throw new NotImplementedException();
		}
	}

	protected override void DoActionOnStart(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				StartSleep();
				break;
		}
	}
}
