using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	public GameObject itemInfoBox;
	TextMeshProUGUI itemName, description, type;
	GameObject useButton;
	public Image icon;
	bool selected;
	[HideInInspector]
	public Item item;
	public TextMeshProUGUI amountText;
	public int amount;

	private void Start()
	{
		itemName = itemInfoBox.transform.Find("Name").GetComponent<TextMeshProUGUI>();
		description = itemInfoBox.transform.Find("Description").GetComponent<TextMeshProUGUI>();
		type = itemInfoBox.transform.Find("Type").GetComponent<TextMeshProUGUI>();
		useButton = itemInfoBox.transform.Find("Button").gameObject;
	}

	public void AddItem(Item i)
	{
		amount++;
		GetComponent<Image>().enabled = true;
		if (item == null)
		{
			item = i;
			icon.sprite = item.icon;
		}
		RefreshInventoryAmount();
	}
	public void RemoveItem()
	{
		amount--;
		RefreshInventoryAmount();
		if (amount <= 0)
		{
			item = null;
			GetComponent<Image>().enabled = false;
		}
	}
	public void OnClick()
	{
		selected = !selected;
		itemInfoBox.SetActive(selected);
		if (item != null)
		{
			itemName.text = item.name;
			description.text = item.description + " " + item.abilityDescription;
			type.text = item.type.ToString();
			useButton.SetActive(item.type == Item.Type.Costruzione || item.periodicUseInterval == GameManager.PeriodicActionInterval.Once);
			useButton.GetComponentInChildren<TextMeshProUGUI>().text = item.type == Item.Type.Costruzione ? "Costruisci" : "Usa";
		}
	}
	public void Use()
	{
		RemoveItem();
		item.DoAction();
	}
	public void RefreshInventoryAmount()
	{
		amountText.text = amount.ToString();
		amountText.gameObject.SetActive(amount > 1);
	}
	int touchRadius = 40;
	public InventoryDragAndDrop clone;
	GameObject c;
	private void Update()
	{
		if (Input.touchCount >= 1)
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Moved && !InventoryManager.dragging && Vector2.Distance(t.position, transform.position) < touchRadius && item != null)
			{
				amountText.text = (amount- 1).ToString();
				amountText.gameObject.SetActive(amount - 1 > 1);
				icon.enabled = amount - 1 >= 1;
				c = Instantiate(clone.gameObject, t.position, Quaternion.identity, transform);
				c.GetComponent<Image>().sprite = item.icon;
				InventoryManager.dragging = true;
			}
		}
	}

	public void EndOfDrag()
	{
		RefreshInventoryAmount();
		InventoryManager.dragging = false;
		GetComponent<Image>().enabled = true;
		Destroy(c);
	}
	public void Drop(InventorySlot s)
	{
		if (s.item != null)
		{
			if (s.item != item)
			{
				EndOfDrag();
			}
			else
			{
				s.AddItem(item);
				RemoveItem();
			}
		}
		else
		{
			s.AddItem(item);
			RemoveItem();
		}
		InventoryManager.dragging = false;
		Destroy(c);
	}
}
