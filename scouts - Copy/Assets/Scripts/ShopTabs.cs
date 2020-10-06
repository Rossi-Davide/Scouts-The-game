using TMPro;
using UnityEngine;

public class ShopTabs : MonoBehaviour
{
	[HideInInspector]
	public int selectedSpecificScreen, selectedMainScreen;
	public ShopTab[] specificTabs; //8 long
	public Animator[] mainTabs;


	public void OnEnable()
	{
		selectedSpecificScreen = (int)Shop.instance.currentSpecificScreen;
		selectedMainScreen = (int)Shop.instance.currentMainScreen;
		SetActiveTabs();
	}

	public void ChangeSpecificScreen(int tabNum)
	{
		if (Shop.instance.ChangeSpecificScreen(tabNum))
		{
			selectedSpecificScreen = tabNum;
			SetActiveTabs();
		}
	}

	public void ChangeMainScreen(int tabNum)
	{
		selectedMainScreen = tabNum;
		Shop.instance.ChangeMainScreen(selectedMainScreen);
		SetActiveTabs();
	}



	public void SetActiveTabs()
	{
		for (int t = 0; t < mainTabs.Length; t++)
		{
			if (t == selectedMainScreen)
				mainTabs[t].Play("Green_Enabled");
			else
				mainTabs[t].Play("Gray_Disabled");
		}
		foreach (var a in specificTabs)
		{
			a.animator.gameObject.SetActive((int)a.mainScreen == selectedMainScreen);
			if (a.animator.gameObject.activeSelf) { a.animator.Play((int)a.specificScreen == selectedSpecificScreen ? "Enabled" : "Disabled"); }
			a.animator.transform.Find("Screen").GetComponent<TextMeshProUGUI>().text = GameManager.ChangeToFriendlyString(a.specificScreen.ToString());
		}
	}
}