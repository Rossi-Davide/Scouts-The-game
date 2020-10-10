using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuilding : ShopObjectBase
{
    public PlayerBuilding building;

    TextMeshProUGUI level, text;

	protected override void Awake()
	{
		base.Awake();
		type = ShopObjectType.Costruzione;
		typeText.text = type.ToString();
		infoText.GetComponent<TextMeshProUGUI>().text = building.description;
		objectName.text = building.name;
		if (building.currentLevel < building.maxLevel)
			price.text = building.prices[building.currentLevel].ToString();
		else
			price.text = building.prices[building.currentLevel - 1].ToString();
		icon.GetComponent<Image>().sprite = building.icon;
		RefreshInfo();
	}
	protected override void OnEnable()
	{
		base.OnEnable();
	}
	public override void RefreshInfo()
	{
		level.text = "Livello " + building.currentLevel + "/" + building.maxLevel;
		GameManager.Counter pt = GameManager.Counter.None;
		if (building.currentLevel < building.maxLevel)
		{
			if (GameManager.HasItemsToBuild(building, building.currentLevel))
			{
				price.transform.parent.GetComponent<Animator>().Play("Enabled");
				price.color = GameManager.instance.GetCounterValue(building.priceTypes[building.currentLevel]) >= building.prices[building.currentLevel] ? Color.white : Color.red;
				pt = building.priceTypes[building.currentLevel];
			}
			else
			{
				price.transform.parent.GetComponent<Animator>().Play("Disabled");
				pt = building.priceTypes[building.currentLevel];
			}
		}
		else
		{
			price.transform.parent.GetComponent<Animator>().Play("Disabled");
			pt = building.priceTypes[building.currentLevel - 1];
		}
		text.text = building.currentLevel > 0 ? "Migliora" : "Costruisci";
		energyLogo.SetActive(pt == GameManager.Counter.Energia);
		materialsLogo.SetActive(pt == GameManager.Counter.Materiali);
		pointsLogo.SetActive(pt == GameManager.Counter.Punti);
	}

	protected override void InitializeVariables()
	{
		level = transform.Find("Level").GetComponent<TextMeshProUGUI>();
		text = transform.Find("BuyButton/Text").GetComponent<TextMeshProUGUI>();
		base.InitializeVariables();
	}

	public override void ToggleInfo()
	{
		base.ToggleInfo();
		level.gameObject.SetActive(!showingInfo);
		text.gameObject.SetActive(!showingInfo);
	}

	public override void Select()
	{
		Shop.instance.DisplayBuildingInfo(building);
	}
}
