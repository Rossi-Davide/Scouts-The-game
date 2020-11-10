using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	[HideInInspector]
	public Image icon;
	[HideInInspector]
	public Item item;
	public TextMeshProUGUI amountText;

	private void Awake()
	{
		icon = GetComponent<Image>();
	}

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
			icon.sprite = item.icon;
		}
		icon.enabled = item != null;
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
			icon.sprite = item.icon;
			amountText.text = item.currentAmount.ToString();
			amountText.gameObject.SetActive(item.currentAmount > 1);
		}
		icon.enabled = item != null;
	}


	public InventoryDragAndDrop clone;
	[HideInInspector]
	public GameObject c;


	public void EndOfDrag()
	{
		RefreshInventoryAmount();
		InventoryManager.dragging = false;
		icon.enabled = true;
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
