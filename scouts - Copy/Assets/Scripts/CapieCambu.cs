using System;
using UnityEngine;

public class CapieCambu : BaseAI
{
	[HideInInspector] [System.NonSerialized]
	public int nextDialogueIndex;

	public Dialogue[] dialoguesArray;

	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionHasAnythingToSayAI: return nextDialogueIndex < dialoguesArray.Length;
			default: return base.GetConditionValue(t);
		}
	}

	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				return null;
			default:
				throw new NotImplementedException();
		}
	}

	protected override void DoActionOnStart(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				DialogueManager.instance.currentObject = this;
				StartCoroutine(ForceTarget(Player.instance.transform.position, true, false, false));
				DialogueManager.instance.TogglePanel(dialoguesArray[nextDialogueIndex]);
				break;
		}
	}
}