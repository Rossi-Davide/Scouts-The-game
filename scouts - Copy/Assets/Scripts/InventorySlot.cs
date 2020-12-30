using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	[HideInInspector] [System.NonSerialized]
	public ObjectBase item;
	public TextMeshProUGUI amountText;

	public void ResetSlot()
	{
		item = null;
		GetComponent<Image>().enabled = false;
	}
	public void AddItemOrReset(ObjectBase i)
	{
		if (i != null)
		{
			item = i;
			GetComponent<Image>().sprite = item.icon;
			GetComponent<Image>().enabled = true;
		}
		else
		{
			ResetSlot();
		}
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

	void CancelDrag()
	{
		RefreshInventoryAmount();
		GetComponent<Image>().enabled = true;
	}
	public void Drop(InventorySlot s)
	{
		if (s != null)
		{
			if (s.item != null)
			{
				if (s.item != item)
				{
					CancelDrag();
				}
				else
				{
					s.AddItemOrReset(item);
					ResetSlot();
				}
			}
			else
			{
				s.AddItemOrReset(item);
				ResetSlot();
			}
		}
		else
		{
			CancelDrag();
		}
		InventoryManager.instance.dragging = false;
		ChestManager.instance.dragging = false;
	}
}
