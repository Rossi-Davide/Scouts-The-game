using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PianoBidoni : BuildingsActionsAbstract
{
	[HideInInspector]
	public bool isCooking, canEat, canCook;
	protected override void Start()
	{
		base.Start();
		canCook = true;
	}
	IEnumerator Cucina()
	{
		isCooking = true;
		RefreshButtonsState();
		loadingBar.GetComponent<TimeLeftBar>().totalTime = 20;
		loadingBar.SetActive(true);
		GetComponent<Animator>().Play("PianoBidoni1");
		yield return new WaitForSeconds(20);
		GetComponent<Animator>().Play("PianoBidoni2");
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -5);
		isCooking = false;
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
			case ConditionType.ConditionIsCooking: return isCooking;
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
				StartCoroutine(Cucina());
				break;
			case 2:
				StartCoroutine(MettiAlSicuro());
				break;
			case 3:
				StartCoroutine(Ripara());
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
