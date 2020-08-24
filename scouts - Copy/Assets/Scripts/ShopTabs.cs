using TMPro;
using UnityEngine;

public class ShopTabs : MonoBehaviour
{
	[HideInInspector]
	public int selectedScreen;
	Animator[] animators;
	ShopItem[] items;


	private void OnEnable()
	{
		selectedScreen = (int)Shop.instance.currentScreen;
		animators = transform.GetComponentsInChildren<Animator>();
		for (int i = 1; i <= animators.Length; i++)
		{
			var a = animators[i - 1];
			if (i == selectedScreen)
				a.Play("Open");
			a.transform.Find("Screen").GetComponent<TextMeshProUGUI>().text = GameManager.ChangeToFriendlyString(((GameManager.ShopScreen)i).ToString());
		}
		items = transform.parent.Find("Mask/Items").GetComponentsInChildren<ShopItem>(true);
		RefreshItems(items);
	}

	public void OnClick(int tabNum)
	{
		selectedScreen = tabNum;
		foreach (var a in animators)
		{
			if (a.gameObject.name == "Tab" + selectedScreen)
			{
				a.Play("Open");
			}
			else
			{
				a.Play("Close");
			}
		}
		Shop.instance.ChangePanel(selectedScreen);
		RefreshItems(items);
		
	}

	void RefreshItems(ShopItem[] items)
	{
		foreach (var i in items)
		{
			i.EnableOrDisable();
		}
	}
}
