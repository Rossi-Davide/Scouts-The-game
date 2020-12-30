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
	private const int maxChestItems = 28;
	public InventorySlot[] slots;
	public Joystick joy;
	public GameObject chestPanelParent, overlay;
	InventorySlot draggingSlot;
	public InventoryDragAndDrop clonePrefab;
	bool isOpen;
	[HideInInspector] [System.NonSerialized]
	public bool dragging;

	void Start()
	{
		if (slots.Length != maxChestItems)
			Debug.LogWarning("Chest contains a different number of slots from the required one.");
		if (fakeInventorySlots.Length != InventoryManager.instance.slots.Length)
			Debug.LogWarning("Chest fake inventory contains a different number of slots from the required one.");
		SetStatus(SaveSystem.instance.LoadData<Status>(SaveSystem.instance.chestManagerFileName, false));
	}

	public bool Contains(ObjectBase i)
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
		isOpen = !isOpen;
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(!isOpen ? "clickDepitched" : "click");
		overlay.SetActive(isOpen);
		chestPanelParent.SetActive(isOpen);
		PanZoom.instance.canDo = !isOpen;
		joy.canUseJoystick = !isOpen;

		if (isOpen)
		{
			for (int y = 0; y < fakeInventorySlots.Length; y++)
			{
				var s = InventoryManager.instance.slots[y];
				fakeInventorySlots[y].AddItemOrReset(s.item);
			}
		}
		else {
			for (int y = 0; y < fakeInventorySlots.Length; y++)
			{
				var s = InventoryManager.instance.slots[y];
				s.AddItemOrReset(fakeInventorySlots[y].item);
			}
		}
		foreach (InventorySlot s in slots)
		{
			s.RefreshInventoryAmount();
		}
	}

	void Update()
	{
		chestPanelParent.GetComponentInChildren<ScrollRect>().enabled = dragging;
		if (isOpen && Input.touchCount >= 1)
		{
			Touch t = Input.GetTouch(0);
			draggingSlot = InventoryManager.CheckIfNearASlot(t);
			if (t.phase == TouchPhase.Moved && !dragging && draggingSlot != null && draggingSlot.item != null)
			{
				draggingSlot.amountText.gameObject.SetActive(false);
				draggingSlot.GetComponent<Image>().enabled = false;
				var clone = Instantiate(clonePrefab, t.position, Quaternion.identity, InventoryManager.instance.ovCanvas.transform);
				clone.GetComponent<Image>().sprite = draggingSlot.item.icon;
				clone.parent = draggingSlot;
				dragging = true;
			}
		}
	}

	#region Status
	public Status SendStatus()
	{
		var it = new ObjectBase[slots.Length];
		for (int i = 0; i < it.Length; i++)
		{
			it[i] = slots[i].item;
		}
		return new Status
		{
			items = it,
		};
	}
	void SetStatus(Status status)
	{
		if (status != null)
		{
			for (int i = 0; i < status.items.Length; i++)
			{
				slots[i].AddItemOrReset(status.items[i]);
			}
		}
	}
	public class Status
	{
		public ObjectBase[] items;
	}
	#endregion
}
