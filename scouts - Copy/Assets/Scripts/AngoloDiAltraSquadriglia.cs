using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngoloDiAltraSquadriglia : InGameObject
{
	bool puoSfidare;
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
				Sfida();
				break;
			default:
				throw new System.NotImplementedException();
		}
	}
}
