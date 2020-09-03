using System.Collections;
using UnityEngine;
using System;

public class Montana : ObjectWithActions
{
	[HideInInspector]
	public bool isDancing, canDance;

	protected override void Start()
	{
		base.Start();
		canDance = true;
	}
	IEnumerator Dance()
	{
		isDancing = true;
		RefreshButtonsState();
		loadingBar.GetComponent<TimeLeftBar>().totalTime = 20;
		loadingBar.SetActive(true);
		yield return new WaitForSeconds(20);
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -5);
		isDancing = false;
		canDance = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
	}

	private void OnWaitEnd()
	{
		canDance = true;
	}


	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanDance: return canDance;
			default: return base.GetConditionValue(t);
		}
	}
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 0:
				StartCoroutine(Dance());
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
