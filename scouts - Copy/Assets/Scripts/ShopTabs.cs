using TMPro;
using UnityEngine;

public class ShopTabs : MonoBehaviour
{
	[HideInInspector]
	public int selectedSpecificScreen, selectedMainScreen;
	public ShopTab[] specificTabs; //8 long
	public Animator[] mainTabs;

	#region Singleton
	public static ShopTabs instance;
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("ShopTabs non è un singleton!");
		instance = this;
	}
	#endregion

	public void OnEnable()
	{
		SetActiveTabs();
	}

	public void ChangeSpecificScreen(int tabNum)
	{
		Shop.instance.ChangeSpecificScreen(tabNum);
	}

	public void ChangeMainScreen(int tabNum)
	{
		Shop.instance.ChangeMainScreen(tabNum);
	}



	public void SetActiveTabs()
	{
		selectedSpecificScreen = (int)Shop.instance.currentSpecificScreen;
		selectedMainScreen = (int)Shop.instance.currentMainScreen;
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