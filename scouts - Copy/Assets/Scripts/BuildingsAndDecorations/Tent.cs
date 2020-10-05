using System;
public class Tent : PlayerBuildingBase
{
	void StartSleep()
	{
		Player.instance.gameObject.SetActive(false);
	}
	void EndOfSleep()
	{
		GameManager.instance.hasSkippedNight = true;
		Player.instance.gameObject.SetActive(true);
		ChangeCounter(1);
		RefreshButtonsState();
		GameManager.Wait(.1f, SkipNight);
	}
	void SkipNight()
	{
		GameManager.instance.hasSkippedNight = false;
	}

	protected override Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				StartSleep();
				return EndOfSleep;
			case 2:
				ChangeCounter(2);
				return MettiAlSicuro;
			case 3:
				ChangeCounter(3);
				return Ripara;
			default:
				throw new NotImplementedException();
		}
	}
}
