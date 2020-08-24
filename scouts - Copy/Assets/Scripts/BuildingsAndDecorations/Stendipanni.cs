using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stendipanni : BuildingsActionsAbstract
{ 
	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				StartCoroutine(MettiAlSicuro());
				break;
			case 2:
				StartCoroutine(Ripara());
				break;
			default:
				throw new NotImplementedException();
		}
	}
}
