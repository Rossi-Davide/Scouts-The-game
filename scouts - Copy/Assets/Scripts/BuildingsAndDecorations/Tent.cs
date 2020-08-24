using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tent : BuildingsActionsAbstract
{
	[HideInInspector]
	public bool isSleeping;

	IEnumerator Sleep()
	{
		isSleeping = true;
		RefreshButtonsState();
		loadingBar.GetComponent<TimeLeftBar>().totalTime = 10;
		loadingBar.SetActive(true);
		Player.instance.gameObject.SetActive(false);
		yield return new WaitForSeconds(10);
		Player.instance.gameObject.SetActive(true);
		GameManager.instance.hasSkippedNight = true;
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, 20);
		yield return new WaitForSeconds(.1f);
		GameManager.instance.hasSkippedNight = false;
		isSleeping = false;
		RefreshButtonsState();
	}



	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionIsSleeping: return isSleeping;
			default: return base.GetConditionValue(t);
		}
	}
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				StartCoroutine(Sleep());
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
