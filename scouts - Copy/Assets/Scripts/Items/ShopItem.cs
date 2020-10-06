using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ShopItem : ShopObjectBase
{
	public Item item;

	TextMeshProUGUI amount;

	protected override void Awake()
	{
		base.Awake();
		infoText.GetComponent<TextMeshProUGUI>().text = item.description + " " + item.abilityDescription;
		objectName.text = item.name;
		type.text = type.ToString();
		price.text = item.price.ToString();
		icon.GetComponent<Image>().sprite = item.icon;
		energyLogo.SetActive(item.priceType == GameManager.Counter.Energia);
		materialsLogo.SetActive(item.priceType == GameManager.Counter.Materiali);
		pointsLogo.SetActive(item.priceType == GameManager.Counter.Punti);
		RefreshInfo();
	}

	public override void RefreshInfo()
	{
		amount.text = item.currentAmount + "/" + item.maxAmount;
		price.transform.parent.GetComponent<Animator>().Play(item.currentAmount < item.maxAmount ? "Enabled" : "Disabled");
		price.color = GameManager.instance.GetCounterValue(item.priceType) >= item.price ? Color.white : Color.red;
	}

	protected override void InitializeVariables()
	{
		amount = transform.Find("Amount").GetComponent<TextMeshProUGUI>();
		base.InitializeVariables();
	}

	public override void ToggleInfo()
	{
		amount.gameObject.SetActive(!showingInfo);
		base.ToggleInfo();
	}

	public override void Select()
	{
		Shop.instance.DisplayItemInfo(item);
	}
}
