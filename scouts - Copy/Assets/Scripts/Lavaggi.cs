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
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -5);
		puoLavarePanni = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnLavarePanniWaitEnd));
	}
	void LavaIPiatti()
	{
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -5);
		puoLavarePiatti = false;
		FindObjectOfType<PianoBidoni>().canCook = true;
		RefreshButtonsState();
	}

	void Pulisci()
	{
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -5);
		GameManager.instance.ChangeCounter(GameManager.Counter.Punti, 6);
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

	protected override int GetTime(int buttonNum)
	{
		switch (buttonNum)
		{
			case 1:
				return 20;
			case 2:
				return 45;
			case 3:
				return 30;
			default: throw new System.NotImplementedException();
		}
	}

	protected override string GetActionName(int buttonNum)
	{
		switch (buttonNum)
		{
			case 1:
				return "Lavare i panni";
			case 2:
				return "Lavare i piatti";
			case 3:
				return "Pulizia lavaggi";
			default: throw new System.NotImplementedException();
		}
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
				break;
			case 2:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, LavaIPiatti);
				break;
			case 3:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, Pulisci);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
