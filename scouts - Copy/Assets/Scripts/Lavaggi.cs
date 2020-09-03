using System.Collections;
using System;
using UnityEngine;

public class Lavaggi : ObjectWithActions
{
	[HideInInspector]
	public bool isCleaning, canClean, staLavandoPanni, puoLavarePanni, staLavandoPiatti, puoLavarePiatti;


	protected override void Start()
	{
		base.Start();
		canClean = true;
		puoLavarePanni = true;
	}
	IEnumerator LavaIPanni()
	{
		staLavandoPanni = true;
		RefreshButtonsState();
		loadingBar.GetComponent<TimeLeftBar>().totalTime = 45;
		loadingBar.SetActive(true);
		yield return new WaitForSeconds(45);
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -5);
		staLavandoPanni = false;
		puoLavarePanni = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnLavarePanniWaitEnd));
	}
	IEnumerator LavaIPiatti()
	{
		staLavandoPiatti = true;
		RefreshButtonsState();
		loadingBar.GetComponent<TimeLeftBar>().totalTime = 30;
		loadingBar.SetActive(true);
		yield return new WaitForSeconds(30);
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -5);
		staLavandoPiatti = false;
		puoLavarePiatti = false;
		FindObjectOfType<PianoBidoni>().canCook = true;
		RefreshButtonsState();
	}

	IEnumerator Pulisci()
	{
		isCleaning = true;
		RefreshButtonsState();
		loadingBar.GetComponent<TimeLeftBar>().totalTime = 20;
		loadingBar.SetActive(true);
		yield return new WaitForSeconds(20);
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -5);
		GameManager.instance.ChangeCounter(GameManager.Counter.Punti, 3);
		isCleaning = false;
		canClean = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[2], OnCleanWaitEnd));
	}

	private void OnCleanWaitEnd()
	{
		canClean = true;
	}
	private void OnLavarePanniWaitEnd()
	{
		puoLavarePanni = true;
	}

	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanCleanLavaggi: return canClean;
			case ConditionType.ConditionPuoLavarePanni: return puoLavarePanni;
			case ConditionType.ConditionPuoLavarePiatti: return puoLavarePiatti;
			default: return base.GetConditionValue(t);
		}
	}
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 0:
				StartCoroutine(LavaIPanni());
				break;
			case 1:
				StartCoroutine(LavaIPiatti());
				break;
			case 2:
				StartCoroutine(Pulisci());
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
