using UnityEngine;

public class AngoloDiAltraSquadriglia : ObjectWithActions
{
	bool puoSfidare;
	[HideInInspector]
	public Squadriglia squadriglia;
	protected override void Start()
	{
		base.Start();
		puoSfidare = true;
	}

	void Sfida()
	{
		puoSfidare = false;
	}

	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionPuoSfidareSquadriglia: return puoSfidare;
			default: return base.GetConditionValue(t);
		}
	}

	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				SfidaManager.instance.ToggleChallengePanel();
				SfidaManager.instance.RefreshChallenge(squadriglia);
				break;
			default:
				throw new System.NotImplementedException();
		}
	}
}
