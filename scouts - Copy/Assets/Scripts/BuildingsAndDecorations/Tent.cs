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
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, 20);
		RefreshButtonsState();
		GameManager.Wait(.1f, SkipNight);
	}
	void SkipNight()
	{
		GameManager.instance.hasSkippedNight = false;
	}

	protected override int GetTime(int buttonNum)
	{
		switch (buttonNum)
		{
			case 1:
				return 10;
			case 2:
				return 5;
			case 3:
				return 60;
			default: throw new System.NotImplementedException();
		}
	}

	protected override string GetActionName(int buttonNum)
	{
		switch (buttonNum)
		{
			case 1:
				return "Dormire";
			case 2:
				return "Imbragare";
			case 3:
				return "Riparare";
			default: throw new System.NotImplementedException();
		}
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
				break;
			case 3:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, Ripara);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
