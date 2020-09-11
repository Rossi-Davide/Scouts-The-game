using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stendipanni : BuildingsActionsAbstract
{
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, MettiAlSicuro);
				ChangeCounter(1);
				break;
			case 2:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, Ripara);
				ChangeCounter(3);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
