using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;
using System;
using UnityEngine.Audio;

public class Campfire : ObjectWithActions
{

	[HideInInspector]
	public bool puoFareLegna;
	[HideInInspector]
	public AudioSource aud;

    void ChangeLight()
    {
        if (!GameManager.instance.isDay)
		{
			aud.Play();
            GetComponent<Light2D>().enabled = true;
		}
        else
		{
			aud.Stop();
            GetComponent<Light2D>().enabled = false;
		}
    }
	

	protected override void Start()
	{
		aud = GetComponent<AudioSource>();
		base.Start();
		puoFareLegna = true;
		GameManager.instance.OnDayEndOrStart += ChangeLight;
	}
	void FaiLegna()
	{
		GameManager.instance.ChangeCounter(GameManager.Counter.Punti, 10);
		GameManager.instance.ChangeCounter(GameManager.Counter.Materiali, -100);
		puoFareLegna = false;
		RefreshButtonsState();
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
	}

	private void OnWaitEnd()
	{
		puoFareLegna = true;
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
	protected override int GetTime(int buttonNum)
	{
		if (buttonNum == 1)
		{
			return 10;
		}
		else
			throw new System.NotImplementedException();
	}

	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionPuoFareLegnaPerFuoco: return puoFareLegna && GameManager.instance.materialsValue >= 100;
			default: return base.GetConditionValue(t);
		}
	}
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				loadingBar.GetComponent<TimeLeftBar>().InitializeValues(action, FaiLegna);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
