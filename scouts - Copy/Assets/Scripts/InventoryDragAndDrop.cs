using UnityEngine;

public class InventoryDragAndDrop : MonoBehaviour
{
	[HideInInspector]
	public InventorySlot parent;


	void Update()
	{
		if (Input.touchCount >= 1)
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Moved)
			{
				transform.position = t.position;
			}
			else if (t.phase == TouchPhase.Ended)
			{
				var s = InventoryManager.instance.CheckIfNearASlot(t);
				if (s != null)
				{
					parent.Drop(s);
				}
				else
				{
					parent.EndOfDrag();
				}
			}
		}
	}
}
