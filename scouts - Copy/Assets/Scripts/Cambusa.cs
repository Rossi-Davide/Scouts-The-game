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
		GameManager.instance.ChangeCounter(GameManager.Counter.Materiali, 250);
		canAttack = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
	}

	private void OnWaitEnd()
	{
		canAttack = true;
	}

	protected override int GetTime(int buttonNum)
	{
		if (buttonNum == 1)
		{
			return 0;
		}
		else if (buttonNum == 2)
		{
			return 30;
		}
		else
			throw new System.NotImplementedException();
	}

	protected override string GetActionName(int buttonNum)
	{
		if (buttonNum == 1)
		{
			return "Negozio";
		}
		else if (buttonNum == 2)
		{
			return "Attacco alla cambusa";
		}
		else
			throw new System.NotImplementedException();
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
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
