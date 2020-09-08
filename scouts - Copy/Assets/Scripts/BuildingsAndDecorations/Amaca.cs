using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Amaca : BuildingsActionsAbstract
{
	[HideInInspector]
	public bool canSleepOnAmaca;



	protected override void Start()
	{
		base.Start();
		canSleepOnAmaca = true;
	}

	void StartSleep()
	{
		Player.instance.gameObject.SetActive(false);
		GetComponent<Animator>().Play("Amaca1");
	}
	void EndOfSleep()
	{
		Player.instance.gameObject.SetActive(true);
		GetComponent<Animator>().Play("Amaca2");
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, 20);
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
				return "Dormire";
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
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, EndOfSleep);
				StartSleep();
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
