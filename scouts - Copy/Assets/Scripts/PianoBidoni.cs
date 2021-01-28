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
	public override Action GetOnEndAction(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				return EndCucina;
			case 2:
				return MettiAlSicuro;
			case 3:
				return Ripara;
			default:
				throw new NotImplementedException();
		}
	}
	protected override void DoActionOnStart(int buttonIndex)
	{
		switch (buttonIndex + 1)
		{
			case 1:
				CucinaStart();
				break;
		}
	}
}
