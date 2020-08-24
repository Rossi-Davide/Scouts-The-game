using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Cambusa : ObjectWithActions
{
	[HideInInspector]
	public bool canAttack;
	protected override void Start()
	{
		base.Start();
		canAttack = true;
	}

	IEnumerator EntraNelloShop()
	{
		Shop.instance.ToggleShop();
		yield return new WaitForEndOfFrame();
	}
	
	IEnumerator Attack()
	{
		RefreshButtonsState();
		loadingBar.GetComponent<TimeLeftBar>().totalTime = 10;
		loadingBar.SetActive(true);
		yield return new WaitForSeconds(10);
		GameManager.instance.ChangeCounter(GameManager.Counter.Materiali, 250);
		canAttack = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
	}

	private void OnWaitEnd()
	{
		canAttack = true;
	}
	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionPuoAttaccareLaCambusa: return canAttack;
			default: return base.GetConditionValue(t);
		}
	}
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 0:
				StartCoroutine(EntraNelloShop());
				break;
			case 1:
				StartCoroutine(Attack());
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
