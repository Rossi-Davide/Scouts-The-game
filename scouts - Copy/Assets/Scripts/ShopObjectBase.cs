using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopObjectBase : MonoBehaviour
{
	public ObjectBase obj;
	protected bool showingInfo;
	protected TextMeshProUGUI objectName, typeText, price, description, level, levelHeader, amount, amountHeader, buyButtonText;
	protected GameObject energyLogo, materialsLogo, pointsLogo, icon, infoButton;

	protected virtual void Awake()
	{
		InitializeVariables();
		RefreshInfo();
	}

	public virtual void RefreshInfo()
	{
		bool canIncreaseLevel = !obj.usingLevel || (!obj.exists || obj.level < obj.maxLevel);
		bool canBuy = !obj.usingAmount || obj.currentAmount < obj.maxAmount;
		Counter pt;
		int pc, index;
		//if (o.exists)
		//{
		//	if (canIncreaseLevel) index = o.level;
		//	else index = o.level - 1;
		//}
		//else index = o.level;
		index = obj.exists ? (canIncreaseLevel ? obj.level + 1 : obj.level) : obj.level;

		pt = obj.shopInfos[index].priceCounter;
		pc = obj.shopInfos[index].Price;
		bool hasItems = GameManager.HasItemsToBuy(obj);
		buyButtonText.text = obj.usingLevel ? (obj.exists ? "Migliora" : "Costruisci") : "Compra";
		price.text = pc.ToString();
		energyLogo.SetActive(pt == Counter.Energia);
		materialsLogo.SetActive(pt == Counter.Materiali);
		pointsLogo.SetActive(pt == Counter.Punti);
		bool hasEnoughMoney = GameManager.instance.GetCounterValue(pt) >= pc;
		price.color = hasEnoughMoney ? UnityEngine.Color.white : UnityEngine.Color.red;
		price.transform.parent.GetComponent<Animator>().Play(canIncreaseLevel && hasItems && canBuy ? "Enabled" : "Disabled");

		amount.text = obj.currentAmount + "/" + obj.maxAmount;
		amount.gameObject.SetActive(obj.usingAmount);
		level.text = (obj.exists ? (obj.level + 1) : obj.level) + "/" + (obj.maxLevel + 1);
		level.gameObject.SetActive(obj.usingLevel);
	}
	protected virtual void InitializeVariables()
	{
		energyLogo = transform.Find("BuyButton/EnergyLogo").gameObject;
		materialsLogo = transform.Find("BuyButton/MaterialsLogo").gameObject;
		pointsLogo = transform.Find("BuyButton/PointsLogo").gameObject;
		description = transform.Find("Info").GetComponent<TextMeshProUGUI>();
		amount = transform.Find("Amount").GetComponent<TextMeshProUGUI>();
		amountHeader = transform.Find("Amount/Text").GetComponent<TextMeshProUGUI>();
		level = transform.Find("Level").GetComponent<TextMeshProUGUI>();
		levelHeader = transform.Find("Level/Text").GetComponent<TextMeshProUGUI>();
		icon = transform.Find("ItemLogo").gameObject;
		objectName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
		typeText = transform.Find("Type").GetComponent<TextMeshProUGUI>();
		price = transform.Find("BuyButton/Price").GetComponent<TextMeshProUGUI>();
		buyButtonText = transform.Find("BuyButton/Text").GetComponent<TextMeshProUGUI>();
		infoButton = transform.Find("InfoButton").gameObject;
		typeText.text = obj.type.ToString();
		description.text = obj.description;
		objectName.text = obj.name;
		icon.GetComponent<Image>().sprite = obj.icon;
		amount.gameObject.SetActive(obj.usingAmount);
		amountHeader.gameObject.SetActive(obj.usingAmount);
		level.gameObject.SetActive(obj.usingLevel);
		levelHeader.gameObject.SetActive(obj.usingLevel);
	}
	protected virtual void OnEnable()
	{
		RefreshInfo();
	}

	public void OnClick()
	{
		if (showingInfo)
		{
			ToggleInfo();
		}
	}

	public virtual void ToggleInfo()
	{
		showingInfo = !showingInfo;
		description.gameObject.SetActive(showingInfo);
		price.transform.parent.gameObject.SetActive(!showingInfo);
		icon.SetActive(!showingInfo);
		infoButton.SetActive(!showingInfo);
		amount.gameObject.SetActive(!showingInfo && obj.usingAmount);
		amountHeader.gameObject.SetActive(!showingInfo && obj.usingAmount);
		level.gameObject.SetActive(!showingInfo && obj.usingLevel);
		levelHeader.gameObject.SetActive(!showingInfo && obj.usingLevel);
		RefreshInfo();
	}

	public virtual void Select()
	{
		Shop.instance.DisplayInfo(obj);
	}
}