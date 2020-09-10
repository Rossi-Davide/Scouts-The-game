using UnityEngine;

public class InventoryDragAndDrop : MonoBehaviour
{
	[HideInInspector]
	public InventorySlot parent;
	int radius = 50;
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
				var s = CheckIfNearASlot(t);
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

	InventorySlot CheckIfNearASlot(Touch t)
	{
		var slots = FindObjectsOfType<InventorySlot>();
		foreach (var s in slots)
		{
			if (s.gameObject.activeSelf && Vector2.Distance(t.position, s.transform.position) <= radius)
			{
				return s;
			}
		}
		return null;
	}
}
