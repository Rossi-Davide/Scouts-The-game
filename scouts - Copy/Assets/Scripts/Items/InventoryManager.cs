using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	private const int maxInventoryItems = 8;
	public static bool dragging;
	public Item[] allItems; //the item database
	public InventorySlot[] slots;

	#region Singleton
	public static InventoryManager instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("Inventory manager is not a singleton");
		}
		instance = this;
	}
	#endregion
	#region Basic Methods
	public void Add(Item item)
	{
		foreach (InventorySlot s in slots)
		{
			var i = s.item;
			if (i == item && i.currentAmount < i.maxAmount)
			{
				s.AddItem(item);
				return;
			}
		}
		foreach (var s in slots)
		{
			if (s.item == null)
			{
				s.AddItem(item);
				break;
			}
		}
	}

	public bool IsInventoryFull()
	{
		foreach (InventorySlot s in slots)
		{
			if (s.item == null || s.item.currentAmount < s.item.maxAmount)
				return false;
		}
		return true;
	} // shop checks if inventory is full before calling the method "Add"

	#endregion
	#region InGameMethods
	public GameObject inventoryPanelParent, inventoryPanel, overlay;
	bool isOpen;
	public void ToggleInventoryPanel()
	{
		if (!isOpen)
		{

			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

			overlay.SetActive(true);
			inventoryPanelParent.SetActive(true);
			isOpen = true;
		}
		else
		{
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");

			isOpen = false;
			inventoryPanelParent.SetActive(false);
			overlay.SetActive(false);
		}
		foreach (InventorySlot s in slots)
		{
			s.RefreshInventoryAmount();
		}
	}
	
	#endregion
	void Start()
	{
		if (slots.Length != maxInventoryItems)
			Debug.LogWarning("Inventory contains a different number of slots from the required one.");
	}
}
