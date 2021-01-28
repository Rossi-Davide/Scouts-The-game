using System;
using UnityEngine.Timeline;

public class Refettorio : PlayerBuildingBase
{
	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
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

	protected override void DoActionOnStart(int buttonIndex) { }
}
