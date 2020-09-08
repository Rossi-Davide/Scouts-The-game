using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Refettorio : BuildingsActionsAbstract
{

	void Eat()
	{
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, 20);
		FindObjectOfType<PianoBidoni>().canEat = false;
		FindObjectOfType<Lavaggi>().puoLavarePiatti = true;
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
				return "Mangiare";
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
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, Eat);
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
