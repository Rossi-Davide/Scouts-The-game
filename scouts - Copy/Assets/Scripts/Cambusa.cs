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

	void EntraNelloShop()
	{
		Shop.instance.ToggleShop();
	}
	
	void Attack()
	{
		canAttack = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[1], OnWaitEnd));
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
			case 1:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, null);
				EntraNelloShop();
				break;
			case 2:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, Attack);
				ChangeCounter(2);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
