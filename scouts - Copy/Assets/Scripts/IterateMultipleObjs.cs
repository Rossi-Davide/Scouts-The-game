using System;
using UnityEngine;

public class IterateMultipleObjs : MonoBehaviour
{
	public ObjectBundle[] bundles;
	private void Start()
	{
		GameManager.instance.OnActionDo += RefreshActions;
		Debug.Log(bundles[0].nextAction);
	}

	void RefreshActions(PlayerAction a)
	{
		foreach (var i in bundles)
		{
			foreach (var o in i.objects)
			{
				if (o.obj.buttons[o.buttonNum - 1].generalAction == a)
				{
					i.nextAction = Array.IndexOf(i.objects, o);
				}
			}
		}
	}

	/// <summary>
	/// Returns true if can do action
	/// </summary>
	/// <param name="objNum">The num of the object that wants to do an action</param>
	/// <param name="bundleNum">The num of the bundle</param>
	public bool CheckAction(int objNum, int bundleNum)
	{
		var a = bundles[bundleNum - 1];
		if (a.nextAction == objNum - 1)
		{
			a.nextAction = objNum < a.objects.Length ? objNum : 0;
			return true;
		}
		else
		{
			return false;
		}
		throw new Exception("value not correct");
	}

}
