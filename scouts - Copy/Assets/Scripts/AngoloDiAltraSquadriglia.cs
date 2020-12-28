using UnityEngine;

public class AngoloDiAltraSquadriglia : InGameObject
{
	[HideInInspector]
	[System.NonSerialized]
	public Squadriglia squadriglia;

	protected override System.Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				SfidaManager.instance.ToggleChallengePanel();
				SfidaManager.instance.RefreshChallenge(squadriglia);
				return null;
			default:
				throw new System.NotImplementedException();
		}
	}
}
