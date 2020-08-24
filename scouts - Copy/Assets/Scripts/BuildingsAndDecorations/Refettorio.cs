using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Refettorio : BuildingsActionsAbstract
{
	[HideInInspector]
	public bool isEating;

	IEnumerator Eat()
	{
		isEating = true;
		RefreshButtonsState();
		loadingBar.GetComponent<TimeLeftBar>().totalTime = 5;
		loadingBar.SetActive(true);
		yield return new WaitForSeconds(5);
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, 20);
		isEating = false;
		FindObjectOfType<PianoBidoni>().canEat = false;
		FindObjectOfType<Lavaggi>().puoLavarePiatti = true;
		RefreshButtonsState();
	}


	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionIsEating: return isEating;
			case ConditionType.ConditionCanEat: return FindObjectOfType<PianoBidoni>().canEat;
			default: return base.GetConditionValue(t);
		}
	}
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				StartCoroutine(Eat());
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
