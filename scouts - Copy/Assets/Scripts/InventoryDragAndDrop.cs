using UnityEngine;

public class InventoryDragAndDrop : MonoBehaviour
{
	[HideInInspector] [System.NonSerialized]
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
				parent.Drop(InventoryManager.CheckIfNearASlot(t));
				Destroy(gameObject);
			}
			else if (t.phase == TouchPhase.Canceled)
			{
				parent.Drop(null);
				Destroy(gameObject);
			}
		}
	}
}
