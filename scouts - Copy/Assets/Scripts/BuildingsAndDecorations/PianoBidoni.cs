using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PianoBidoni : PlayerBuildingBase
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
		ChangeCounter(1);
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
