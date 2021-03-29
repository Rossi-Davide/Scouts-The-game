using UnityEngine;

using System;
public class AngoloDiAltraSquadriglia : InGameObject
{
	[HideInInspector]
	[System.NonSerialized]
	public Squadriglia squadriglia;

	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				return null;
			default:
				throw new System.NotImplementedException();
		}
	}

	protected override void DoActionOnStart(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				SfidaManager.instance.ToggleChallengePanel();
				SfidaManager.instance.RefreshChallenge(squadriglia, this);
				break;
		}
	}
}
