using System.Collections;
using System;
using UnityEngine;

public class Lavaggi : ObjectWithActions
{
	[HideInInspector]
	public bool canClean, puoLavarePanni, puoLavarePiatti;


	protected override void Start()
	{
		base.Start();
		canClean = true;
		puoLavarePanni = true;
	}
	void LavaIPanni()
	{
		puoLavarePanni = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnLavarePanniWaitEnd));
	}
	void LavaIPiatti()
	{
		puoLavarePiatti = false;
		FindObjectOfType<PianoBidoni>().canCook = true;
		RefreshButtonsState();
	}

	void Pulisci()
	{
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
			case 1:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, LavaIPanni);
				ChangeCounter(1);
				break;
			case 2:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, LavaIPiatti);
				ChangeCounter(2);
				break;
			case 3:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, Pulisci);
				ChangeCounter(3);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
