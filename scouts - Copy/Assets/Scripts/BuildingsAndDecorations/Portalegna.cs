using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Portalegna : BuildingsActionsAbstract
{
	protected override int GetTime(int buttonNum)
	{
		switch (buttonNum)
		{
			case 1:
				return 5;
			case 2:
				return 60;
			default: throw new System.NotImplementedException();
		}
	}

	protected override string GetActionName(int buttonNum)
	{
		switch (buttonNum)
		{
			case 1:
				return "Imbragare";
			case 2:
				return "Riparare";
			default: throw new System.NotImplementedException();
		}
	}


	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, MettiAlSicuro);
				break;
			case 2:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, Ripara);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
