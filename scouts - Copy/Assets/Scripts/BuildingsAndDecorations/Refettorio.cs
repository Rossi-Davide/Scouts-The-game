using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Refettorio : PlayerBuildingBase
{

	void Eat()
	{
		FindObjectOfType<PianoBidoni>().canEat = false;
		FindObjectOfType<Lavaggi>().puoLavarePiatti = true;
		ChangeCounter(1);
		RefreshButtonsState();
	}


	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanEat: return FindObjectOfType<PianoBidoni>().canEat;
			default: return base.GetConditionValue(t);
		}
	}


	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, Eat);
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
