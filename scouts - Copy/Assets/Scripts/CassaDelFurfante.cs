using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassaDelFurfante : InGameObject
{
	public Vector3[] possiblePositions;
	bool canUnlock;
	protected override void Start()
	{
		base.Start();
		canUnlock = true;
		transform.position = possiblePositions[Random.Range(0, possiblePositions.Length - 1)];
	}

	void SbloccaScreen()
	{
		Shop.instance.negozioIllegaleUnlocked = true;
		canUnlock = false;
	}


	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanUnlockNegozioDelFurfante: return canUnlock;
			default: return base.GetConditionValue(t);
		}
	}

	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				SbloccaScreen();
				break;
			default:
				throw new System.NotImplementedException();
		}
	}
}
