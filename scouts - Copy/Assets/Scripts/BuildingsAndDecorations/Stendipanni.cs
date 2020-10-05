using System;

public class Stendipanni : PlayerBuildingBase
{
	protected override Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				ChangeCounter(1);
				return MettiAlSicuro;
			case 2:
				ChangeCounter(3);
				return Ripara;
			default:
				throw new NotImplementedException();
		}
	}
}
