using System.Collections;
using UnityEngine;
using System;

public class Latrina : ObjectWithActions
{
	[HideInInspector]
	public bool isCleaningLatrina, canCleanLatrina;

	protected override void Start()
	{
		base.Start();
		canCleanLatrina = true;
	}
	IEnumerator Pulisci()
	{
		isCleaningLatrina = true;
		RefreshButtonsState();
		loadingBar.GetComponent<TimeLeftBar>().totalTime = 10;
		loadingBar.SetActive(true);
		yield return new WaitForSeconds(10);
		GameManager.instance.ChangeCounter(GameManager.Counter.Punti, 5);
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -5);
		isCleaningLatrina = false;
		canCleanLatrina = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
	}

	private void OnWaitEnd()
	{
		canCleanLatrina = true;
	}

	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionIsCleaningLatrina: return isCleaningLatrina;
			case ConditionType.ConditionCanCleanLatrina: return canCleanLatrina;
			default: return base.GetConditionValue(t);
		}
	}
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 0:
				StartCoroutine(Pulisci());
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
