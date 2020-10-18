using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopObjectBase : MonoBehaviour
{
	public ObjectBase obj;
    protected bool showingInfo;
    protected TextMeshProUGUI objectName, typeText, price, description, level, levelHeader, amount, amountHeader;
    protected GameObject energyLogo, materialsLogo, pointsLogo, icon, infoButton;

	protected virtual void Awake()
	{
		InitializeVariables();
		RefreshInfo();
	}

    public virtual void RefreshInfo()
	{
		level.text = "Livello " + obj.currentLevel + "/" + obj.maxLevel;
		Counter pt = Counter.None;
		if (obj.currentLevel < obj.maxLevel)
		{
			if (GameManager.HasItemsToBuy(obj, obj.currentLevel))
			{
				price.transform.parent.GetComponent<Animator>().Play("Enabled");
				price.color = GameManager.instance.GetCounterValue(obj.shopInfos[obj.currentLevel].priceCounter) >= obj.shopInfos[obj.currentLevel].price ? Color.white : Color.red;
				pt = obj.shopInfos[obj.currentLevel].priceCounter;
			}
			else
			{
				price.transform.parent.GetComponent<Animator>().Play("Disabled");
				pt = obj.shopInfos[obj.currentLevel].priceCounter;
			}
		}
		else
		{
			price.transform.parent.GetComponent<Animator>().Play("Disabled");
			pt = obj.shopInfos[obj.currentLevel - 1].priceCounter;
		}
		levelHeader.text = obj.currentLevel > 0 ? "Migliora" : "Costruisci";
		amount.text = obj.currentAmount + "/" + obj.maxAmount;
		energyLogo.SetActive(pt == Counter.Energia);
		materialsLogo.SetActive(pt == Counter.Materiali);
		pointsLogo.SetActive(pt == Counter.Punti);
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
		infoButton = transform.Find("InfoButton").gameObject;
		typeText.text = obj.type.ToString();
		description.text = obj.description;
		objectName.text = obj.name;
		icon.GetComponent<Image>().sprite = obj.icon;
		amount.gameObject.SetActive(obj.usingAmount);
		amountHeader.gameObject.SetActive(obj.usingAmount);
		level.gameObject.SetActive(obj.showLevel);
		levelHeader.gameObject.SetActive(obj.showLevel);
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
		amount.gameObject.SetActive(!showingInfo);
		amountHeader.gameObject.SetActive(!showingInfo);
		level.gameObject.SetActive(!showingInfo);
		levelHeader.gameObject.SetActive(!showingInfo);
	}

	public virtual void Select() 
	{
		Shop.instance.DisplayInfo(obj);
	}
}