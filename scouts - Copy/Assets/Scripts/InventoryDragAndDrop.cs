using UnityEngine;

public class InventoryDragAndDrop : MonoBehaviour
{
	[HideInInspector] [System.NonSerialized]
	public InventorySlot parent;


	void FixedUpdate()
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
				var s = InventoryManager.CheckIfNearASlot(t);
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
