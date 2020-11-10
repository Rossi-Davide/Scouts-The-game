using UnityEngine;
using UnityEngine.UI;

public class ChestManager : MonoBehaviour
{
	#region Singleton
	public static ChestManager instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("Chest manager is not a singleton");
		}
		instance = this;
	}
	#endregion

	public InventorySlot[] fakeInventorySlots;
	private const int maxItemsPerChest = 28;
	public InventorySlot[] slots;

	public GameObject chestPanelParent, overlay;
	bool isOpen;

	public bool Contains(Item i)
	{
		foreach (var s in slots)
		{
			if (s.item == i)
			{
				return true;
			}
		}
		return false;
	}


	public void ToggleChestPanel()
	{
		if (!isOpen)
		{

			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

			overlay.SetActive(true);
			chestPanelParent.SetActive(true);
			isOpen = true;
			PanZoom.instance.canDo = false;


			for (int y = 0; y < fakeInventorySlots.Length; y++)
			{
				var s = InventoryManager.instance.slots[y];
				fakeInventorySlots[y].ResetSlot();
				fakeInventorySlots[y].AddItem(s.item);
				fakeInventorySlots[y].RefreshInventoryAmount();
			}
		}
		else
		{
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");

			isOpen = false;
			chestPanelParent.SetActive(false);
			overlay.SetActive(false);
			PanZoom.instance.canDo = true;

			for (int y = 0; y < fakeInventorySlots.Length; y++)
			{
				var s = InventoryManager.instance.slots[y];
				s.AddItem(fakeInventorySlots[y].item);
				s.RefreshInventoryAmount();
			}
		}
		foreach (InventorySlot s in slots)
		{
			s.RefreshInventoryAmount();
		}
	}

	void Update()
	{
		if (InventoryManager.dragging)
		{
			chestPanelParent.GetComponentInChildren<ScrollRect>().enabled = false;
		}
		else
		{
			chestPanelParent.GetComponentInChildren<ScrollRect>().enabled = true;
		}
	}


	void Start()
	{
		if (slots.Length != maxItemsPerChest)
			Debug.LogWarning("Chest contains a different number of slots from the required one.");
		if (fakeInventorySlots.Length != InventoryManager.instance.slots.Length)
			Debug.LogWarning("Chest fake inventory contains a different number of slots from the required one.");
		SaveSystem.instance.OnReadyToLoad += ReceiveSavedData;
	}
	void ReceiveSavedData(LoadPriority p)
	{
		if (p == LoadPriority.Low)
		{
			for (int i = 0; i < slots.Length; i++)
			{
				slots[i].item = (Item)SaveSystem.instance.RequestData(DataCategory.ChestManager, DataKey.slots, DataParameter.item, i);
			}
		}
	}
}
