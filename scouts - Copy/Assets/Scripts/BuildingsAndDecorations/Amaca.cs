using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Amaca : PlayerBuildingBase
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
		ChangeCounter(1);
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
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, EndOfSleep);
				StartSleep();
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
