using System;
public class Tent : PlayerBuildingBase
{
	void StartSleep()
	{
		Player.instance.gameObject.SetActive(false);
	}
	void EndOfSleep()
	{
		GameManager.instance.SkipNight();
		Player.instance.gameObject.SetActive(true);
		RefreshButtonsState();
	}
	protected override Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				StartSleep();
				return EndOfSleep;
			case 2:
				return MettiAlSicuro;
			case 3:
				return Ripara;
			default:
				throw new NotImplementedException();
		}
	}
}
