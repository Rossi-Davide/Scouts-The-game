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
		infoText.GetComponent<TextMeshProUGUI>().text = building.description;
		objectName.text = building.name;
		type.text = type.ToString();
		price.text = building.prices[building.currentLevel].ToString();
		icon.GetComponent<Image>().sprite = building.icon;
		RefreshInfo();
	}
	protected override void OnEnable()
	{
		base.OnEnable();
		energyLogo.SetActive(building.priceTypes[building.currentLevel] == GameManager.Counter.Energia);
		materialsLogo.SetActive(building.priceTypes[building.currentLevel] == GameManager.Counter.Materiali);
		pointsLogo.SetActive(building.priceTypes[building.currentLevel] == GameManager.Counter.Punti);
	}
	public override void RefreshInfo()
	{
		level.text = "Level " + building.currentLevel + "/" + building.maxLevel;
		price.transform.parent.GetComponent<Animator>().Play(building.currentLevel < building.maxLevel ? "Enabled" : "Disabled");
		price.color = GameManager.instance.GetCounterValue(building.priceTypes[building.currentLevel]) >= building.prices[building.currentLevel] ? Color.white : Color.red;
		text.text = building.currentLevel > 0 ? "Migliora" : "Costruisci";
	}

	protected override void InitializeVariables()
	{
		level = transform.Find("Level").GetComponent<TextMeshProUGUI>();
		text = transform.Find("BuyButton/Text").GetComponent<TextMeshProUGUI>();
		base.InitializeVariables();
	}

	public override void ToggleInfo()
	{
		level.gameObject.SetActive(!showingInfo);
		text.gameObject.SetActive(!showingInfo);
		base.ToggleInfo();
	}

	public override void Select()
	{
		//Shop.instance.DisplayItemInfo(building);
	}
}
