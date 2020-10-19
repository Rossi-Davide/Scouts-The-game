using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;

public class Campfire : InGameObject
{
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
		GameManager.instance.OnDayEndOrStart += ChangeLight;
	}
	void FaiLegna()
	{
		RefreshButtonsState();
		StartWaitToUseAgain(buttons[0]);
	}
	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionHasEnoughMaterials: return GameManager.instance.materialsValue >= Mathf.Abs(buttons[0].generalAction.materialsGiven);
			default: return base.GetConditionValue(t);
		}
	}
	protected override Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				return FaiLegna;
			default:
				throw new NotImplementedException();
		}
	}
}
