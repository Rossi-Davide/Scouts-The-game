using System;
using System.Collections;
using UnityEngine;

public class IterateMultipleObjs : MonoBehaviour
{
	public ObjectBundle[] bundles;
	SaveSystem saveSystem;
	private void Start()
	{
		GameManager.instance.OnActionDo += RefreshActions;
		GameManager.instance.OnObjectArrayUpdate += FindObjectReferences;
		StartCoroutine(GetBuild());
		saveSystem = SaveSystem.instance;
		saveSystem.onReadyToLoad += ReceiveSavedData;
	}
	void ReceiveSavedData()
	{
		for (int i = 0; i < bundles.Length; i++)
		{
			bundles[i].nextAction = (int)saveSystem.RequestData(DataCategory.IterateMultipleObjs, DataKey.bundles, DataParameter.nextAction, i);
		}
	}

	IEnumerator GetBuild()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (Array.Exists(SquadrigliaManager.instance.GetPlayerSq().buildings, el => el == GetComponent<PlayerBuildingBase>()))
		{
			bundles[0].objects[0].obj = GetComponent<PlayerBuildingBase>();
		}
		FindObjectReferences();
	}



	void FindObjectReferences()
	{
		foreach (var i in bundles)
		{
			foreach (var o in i.objects)
			{
				foreach (var b in GameManager.instance.InGameObjects)
				{
					if (o.objectName == b.objectName)
						o.obj = b;
				}
				foreach (var b in SquadrigliaManager.instance.GetPlayerSq().buildings)
				{
					if (o.objectName == b.GetComponent<PlayerBuildingBase>().building.name)
						o.obj = b.GetComponent<PlayerBuildingBase>();
				}
			}
		}
	}

	void RefreshActions(PlayerAction a)
	{
		foreach (var i in bundles)
		{
			foreach (var o in i.objects)
			{
				if (o.obj.buttons[o.buttonNum - 1 ].generalAction == a)
				{
					var num = Array.IndexOf(i.objects, o) + 1;
					i.nextAction = num < i.objects.Length ? num : 0;
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
			return true;
		}
		else
		{
			return false;
		}
		throw new Exception("value not correct");
	}

}
