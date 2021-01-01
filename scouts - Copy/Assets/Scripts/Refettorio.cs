using System;
using UnityEngine.Timeline;

public class Refettorio : PlayerBuildingBase
{
	protected override System.Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				return null;
			case 2:
				return MettiAlSicuro;
			case 3:
				return Ripara;
			default:
				throw new NotImplementedException();
		}
	}

	private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
	{
		UnityEngine.Debug.Log("collision enter");
	}
}
