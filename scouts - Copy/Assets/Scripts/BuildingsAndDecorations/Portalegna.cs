using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Portalegna : PlayerBuildingBase
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
				ChangeCounter(2);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
