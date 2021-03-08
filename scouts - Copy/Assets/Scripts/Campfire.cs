using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;
using System.Collections;

public class Campfire : InGameObject
{
	[HideInInspector] [System.NonSerialized]
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
		GameManager.instance.OnSunsetOrSunrise += ChangeLight;
		StartCoroutine(CallChangeLight());
	}
	IEnumerator CallChangeLight()
	{
		yield return new WaitForSeconds(GameManager.minuteDuration + 0.05f);
		ChangeLight(GameManager.instance.isDay);
	}

	void FaiLegna()
	{
		RefreshButtonsState();
	}
	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionHasEnoughMaterials: return buttons[0].generalAction.EditableMaterialsGiven < 0 && GameManager.instance.GetCounterValue(Counter.Materiali) >= Mathf.Abs(buttons[0].generalAction.EditableMaterialsGiven);
			default: return base.GetConditionValue(t);
		}
	}
	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				return FaiLegna;
			default:
				throw new NotImplementedException();
		}
	}

	protected override void DoActionOnStart(int buttonIndex) { }
}
