using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Decorations : ObjectWithActions
{
	public Button button;
	Button btn;
	protected override void Start()
	{
		base.Start();
		btn = Instantiate(button, transform.position, Quaternion.identity, wpCanvas.transform);
		btn.onClick.AddListener(OnClick);
	}

	void PlayerHandPunch()
	{
		Player.instance.enabled = false;
		if (Player.instance.transform.position.x >= transform.position.x)
		{
			Player.instance.GetComponent<Animator>().Play("leftHandPunch");
		}
		else
		{
			Player.instance.GetComponent<Animator>().Play("rightHandPunch");
		}
		GetComponent<ParticleSystem>().Play();
	}

	void EndPunch()
	{
		GetComponent<ParticleSystem>().Stop();
		Player.instance.GetComponent<Animator>().Play("idle");
		Player.instance.enabled = true;
		GameManager.instance.ChangeCounter(GameManager.Counter.Materiali, 15);
		GameManager.instance.spawnedDecorations.Remove(gameObject);
		Deselect();
		Destroy(gameObject);
		Destroy(btn.gameObject);
	}

	protected override int GetTime(int buttonNum)
	{
		if (buttonNum == 1)
		{
			return 3;
		}
		else
			throw new System.NotImplementedException();
	}

	protected override string GetActionName(int buttonNum)
	{
		if (buttonNum == 1)
		{
			return "Fare legna";
		}
		else
			throw new System.NotImplementedException();
	}

	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			default: return base.GetConditionValue(t);
		}
	}
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, EndPunch);
				PlayerHandPunch();
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
