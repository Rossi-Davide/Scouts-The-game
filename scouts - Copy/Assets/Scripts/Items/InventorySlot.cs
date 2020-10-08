using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	public Image icon;
	[HideInInspector]
	public Item item;
	public TextMeshProUGUI amountText;
	public int amount;

	public void ResetSlot()
	{
		amount = 0;
		item = null;
		GetComponent<Image>().enabled = false;
	}

	public void SetAllValues(int a, Item i)
	{
		amount = a;
		item = i;
		GetComponent<Image>().enabled = amount > 0;
		if (item != null)
		{
			icon.sprite = item.icon;
		}
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
		InventoryManager.instance.SelectItem(this);
	}
	public void RefreshInventoryAmount()
	{
		amountText.text = amount.ToString();
		amountText.gameObject.SetActive(amount > 1);
	}


	public InventoryDragAndDrop clone;
	[HideInInspector]
	public GameObject c;
	

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
