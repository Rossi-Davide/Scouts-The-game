using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	[HideInInspector] [System.NonSerialized]
	public Item item;
	public TextMeshProUGUI amountText;

	public void ResetSlot()
	{
		item = null;
		GetComponent<Image>().enabled = false;
	}
	public void AddItem(Item i)
	{
		if (i != null)
		{
			item = i;
			GetComponent<Image>().sprite = item.icon;
		}
		GetComponent<Image>().enabled = item != null;
		RefreshInventoryAmount();
	}

	public void OnClick()
	{
		InventoryManager.instance.SelectItem(this);
	}
	public void RefreshInventoryAmount()
	{
		if (item != null)
		{
			GetComponent<Image>().sprite = item.icon;
			amountText.text = item.currentAmount.ToString();
			amountText.gameObject.SetActive(item.currentAmount > 1);
		}
		GetComponent<Image>().enabled = item != null;
	}


	public InventoryDragAndDrop clone;
	[HideInInspector] [System.NonSerialized]
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
				ResetSlot();
			}
		}
		else
		{
			s.AddItem(item);
			ResetSlot();
		}
		InventoryManager.dragging = false;
		Destroy(c);
	}
}
