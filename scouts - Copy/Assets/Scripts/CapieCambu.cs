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

	protected override Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				DialogueManager.instance.currentObject = this;
				ForceTarget(Player.instance.transform.position, true, false);
				DialogueManager.instance.TogglePanel(dialoguesArray[nextDialogueIndex]);
				return null;
			default:
				throw new NotImplementedException();
		}
	}
}