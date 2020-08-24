using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class ShopItem : MonoBehaviour
{
	public Item item;
	bool showingInfo;

	TextMeshProUGUI itemName, type, amount, price;
	GameObject energyLogo, materialsLogo, pointsLogo, info, itemIcon;

	private void Awake()
	{
		InitializeVariables();
		info.GetComponent<TextMeshProUGUI>().text = item.description + " " + item.abilityDescription;
		itemName.text = item.name;
		type.text = item.type.ToString();
		price.text = item.price.ToString();
		itemIcon.GetComponent<Image>().sprite = item.icon;
		energyLogo.SetActive(item.priceType == GameManager.Counter.Energia);
		materialsLogo.SetActive(item.priceType == GameManager.Counter.Materiali);
		pointsLogo.SetActive(item.priceType == GameManager.Counter.Punti);
		RefreshInfo();
	}
	private void OnEnable()
	{
		RefreshInfo();
	}
	public void RefreshInfo()
	{
		amount.text = item.currentAmount + "/" + item.maxAmount;
		price.color = GameManager.instance.CheckCounterValue(item.priceType) >= item.price ? Color.white : Color.red;
		price.transform.parent.GetComponent<Animator>().Play(item.currentAmount < item.maxAmount ? "Enabled" : "Disabled");
	}

	private void InitializeVariables()
	{
		energyLogo = transform.Find("BuyButton/EnergyLogo").gameObject;
		materialsLogo = transform.Find("BuyButton/MaterialsLogo").gameObject;
		pointsLogo = transform.Find("BuyButton/PointsLogo").gameObject;
		info = transform.Find("Info").gameObject;
		itemIcon = transform.Find("ItemLogo").gameObject;
		itemName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
		type = transform.Find("Type").GetComponent<TextMeshProUGUI>();
		amount = transform.Find("Amount").GetComponent<TextMeshProUGUI>();
		price = transform.Find("BuyButton/Text").GetComponent<TextMeshProUGUI>();
	}

	public void EnableOrDisable()
	{
		gameObject.SetActive(Shop.instance.currentScreen == item.shopScreen);
	}

	public void ToggleInfo()
	{
		showingInfo = !showingInfo;
		info.SetActive(showingInfo);
		amount.gameObject.SetActive(!showingInfo);
		price.transform.parent.gameObject.SetActive(!showingInfo);
		itemIcon.SetActive(!showingInfo);
		transform.Find("InfoButton").gameObject.SetActive(!showingInfo);
	}

	public void OnClick()
	{
		if (showingInfo)
		{
			ToggleInfo();
		}
	}

	public void Select()
	{
		Shop.instance.DisplayItemInfo(item);
	}
}
