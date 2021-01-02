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

	
}
