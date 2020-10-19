using System;

public class Portalegna : PlayerBuildingBase
{
	protected override System.Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				return MettiAlSicuro;
			case 2:
				return Ripara;
			default:
				throw new NotImplementedException();
		}
	}
}
