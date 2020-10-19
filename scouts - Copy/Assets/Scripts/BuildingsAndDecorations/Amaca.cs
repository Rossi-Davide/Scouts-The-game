using System;
using UnityEngine;

public class Amaca : PlayerBuildingBase
{
	void StartSleep()
	{
		Player.instance.gameObject.SetActive(false);
		GetComponent<Animator>().Play("Amaca1");
	}
	void EndOfSleep()
	{
		Player.instance.gameObject.SetActive(true);
		GetComponent<Animator>().Play("Amaca2");
		GameManager.instance.ChangeCounter(Counter.Energia, 20);
		RefreshButtonsState();
		StartWaitToUseAgain(buttons[0]);
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
