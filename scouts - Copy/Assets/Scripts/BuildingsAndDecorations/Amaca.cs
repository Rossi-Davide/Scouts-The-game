using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Amaca : BuildingsActionsAbstract
{
	[HideInInspector]
	public bool isSleeping, canSleepOnAmaca;



	protected override void Start()
	{
		base.Start();
		canSleepOnAmaca = true;
	}


	IEnumerator Sleep()
	{
		isSleeping = true;
		RefreshButtonsState();
		loadingBar.GetComponent<TimeLeftBar>().totalTime = 10;
		loadingBar.SetActive(true);
		Player.instance.gameObject.SetActive(false);
		GetComponent<Animator>().Play("Amaca1");
		yield return new WaitForSeconds(10);
		Player.instance.gameObject.SetActive(true);
		GetComponent<Animator>().Play("Amaca2");
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, 20);
		isSleeping = false;
		canSleepOnAmaca = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
	}

	private void OnWaitEnd()
	{
		canSleepOnAmaca = true;
	}


	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanSleepOnAmaca: return canSleepOnAmaca;
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
