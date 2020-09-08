using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PianoBidoni : BuildingsActionsAbstract
{
	[HideInInspector]
	public bool canEat, canCook;
	protected override void Start()
	{
		base.Start();
		canCook = true;
	}
	void CucinaStart()
	{
		GetComponent<Animator>().Play("PianoBidoni1");
	}
	void EndCucina()
	{
		GetComponent<Animator>().Play("PianoBidoni2");
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -5);
		canCook = false;
		canEat = true;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
	}

	void OnWaitEnd()
	{
		canCook = true;
	}

	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanEat: return canEat;
			case ConditionType.ConditionPuoLavarePiatti: return FindObjectOfType<Lavaggi>().puoLavarePiatti;
			case ConditionType.ConditionCanCook: return canCook;
			default: return base.GetConditionValue(t);
		}
	}



	protected override int GetTime(int buttonNum)
	{
		switch (buttonNum)
		{
			case 1:
				return 20;
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
				return "Cucinare";
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
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, EndCucina);
				CucinaStart();
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
