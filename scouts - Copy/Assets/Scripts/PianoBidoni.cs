using System;
using UnityEngine;

public class PianoBidoni : PlayerBuildingBase
{
	void CucinaStart()
	{
		GetComponent<Animator>().Play("PianoBidoni1");
	}
	void EndCucina()
	{
		GetComponent<Animator>().Play("PianoBidoni2");
		RefreshButtonsState();
	}
	protected override Action DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				CucinaStart();
				return EndCucina;
			case 2:
				return MettiAlSicuro;
			case 3:
				return Ripara;
			default:
				throw new NotImplementedException();
		}
	}
}
