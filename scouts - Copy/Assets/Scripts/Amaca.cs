using System;
using UnityEngine;

public class Amaca : PlayerBuildingBase
{
	void StartSleep()
	{
		Player.instance.GetComponent<Animator>().Play("amacaDormireLv" + (building.level + 1));
		GetComponent<SpriteRenderer>().enabled = false;
	}
	void EndOfSleep()
	{
		Player.instance.GetComponent<Animator>().Play("Idle");
		GetComponent<SpriteRenderer>().enabled = true;
		GameManager.instance.ChangeCounter(Counter.Energia, 20);
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
