using System;

public class Portalegna : PlayerBuildingBase
{
	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				return MettiAlSicuro;
			case 2:
				return Ripara;
			default:
				throw new NotImplementedException();
		}
	}

	protected override void DoActionOnStart(int buttonIndex) { }
}
