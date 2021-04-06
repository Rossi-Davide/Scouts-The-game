using System;
using UnityEngine.Timeline;

public class Refettorio : PlayerBuildingBase
{
	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				return EndEat;
			case 2:
				return MettiAlSicuro;
			case 3:
				return Ripara;
			default:
				throw new NotImplementedException();
		}
	}

	void StartEat()
	{
		Player.instance.gameObject.SetActive(false);
		Joystick.instance.enabled = false;
	}
	void EndEat()
	{
		Player.instance.gameObject.SetActive(true);
		Joystick.instance.enabled = true;
	}

	protected override void DoActionOnStart(int buttonIndex) 
	{
		switch (buttonIndex + 1)
		{
			case 1:
				StartEat();
				break;
		}
	}
}
