using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;

public class Campfire : InGameObject
{
	[HideInInspector]
	public AudioSource aud;

    void ChangeLight(bool day)
    {
        if (!day)
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
		ChangeLight(GameManager.instance.isDay);
		GameManager.instance.OnSunsetOrSunrise += ChangeLight;
	}
	void FaiLegna()
	{
		RefreshButtonsState();
	}
	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionHasEnoughMaterials: return buttons[0].generalAction.materialsGiven < 0 && GameManager.instance.materialsValue >= Mathf.Abs(buttons[0].generalAction.materialsGiven);
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
