using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tent : BuildingsActionsAbstract
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

	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, EndOfSleep);
				StartSleep();
				break;
			case 2:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, MettiAlSicuro);
				ChangeCounter(2);
				break;
			case 3:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, Ripara);
				ChangeCounter(3);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
