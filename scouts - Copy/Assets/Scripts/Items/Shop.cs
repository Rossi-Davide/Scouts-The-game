using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	[HideInInspector]
	public GameManager.SpecificShopScreen currentSpecificScreen;
	[HideInInspector]
	public GameManager.MainShopScreen currentMainScreen;
	public GameObject shopPanel, pioneristica, cucina, infermieristica, topografia, espressione, negozioIllegale, costruzioni, decorazioni;
	bool hasEnoughMoney, canBuy, hasItems;
	[HideInInspector]
	public bool negozioIllegaleUnlocked;


	TextMeshProUGUI itemName, description, price, amount, amountText, itemsNeeded;
	public GameObject infoPanel;
	GameObject materialsLogo, pointsLogo, energyLogo, icon, buyButton;



	Item selectedItem;
	PlayerBuilding selectedBuilding;
	#region Singleton
	public static Shop instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("singleton già creato");
		}
		instance = this;
	}
	#endregion



	private void Start()
	{
		currentMainScreen = GameManager.MainShopScreen.Costruzioni;
		currentSpecificScreen = GameManager.SpecificShopScreen.Pioneristica;
		buyButton = infoPanel.transform.Find("BuyButton").gameObject;
		materialsLogo = buyButton.transform.Find("MaterialsLogo").gameObject;
		energyLogo = buyButton.transform.Find("EnergyLogo").gameObject;
		pointsLogo = buyButton.transform.Find("PointsLogo").gameObject;
		icon = infoPanel.transform.Find("Icon").gameObject;
		itemName = infoPanel.transform.Find("Name").GetComponent<TextMeshProUGUI>();
		description = infoPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>();
		itemsNeeded = infoPanel.transform.Find("ItemsNeeded").GetComponent<TextMeshProUGUI>();
		price = buyButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
		amount = infoPanel.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
		amountText = infoPanel.transform.Find("Amount/Text").GetComponent<TextMeshProUGUI>();
		buyButton.GetComponent<Button>().onClick.AddListener(SelectBuyMethod);
	}

	void SelectBuyMethod()
	{
		if (selectedBuilding != null)
			BuyBuilding();
		else if (selectedItem != null)
			BuyItem();
	}



	public void BuyItem()
	{
		if (InventoryManager.instance.IsInventoryFull())
		{
			GameManager.instance.WarningMessage("L'inventario è pieno!");
			return;
		}
		if (!canBuy)
		{
			GameManager.instance.WarningMessage("Hai già acquistato il numero massimo di questi item");
			return;
		}
		if (!hasEnoughMoney)
		{
			GameManager.instance.WarningMessage($"Non hai abbastanza {selectedItem.priceType} per comprare {selectedItem.name}");
			return;
		}
		selectedItem.currentAmount++;
		GameManager.instance.ChangeCounter(selectedItem.priceType, -selectedItem.price);
		InventoryManager.instance.Add(selectedItem);
		var items = shopPanel.GetComponentsInChildren<ShopItem>();
		foreach (var i in items)
		{
			i.RefreshInfo();
		}
		CloseInfoPanel();
	}

	public void BuyBuilding()
	{
		if (!canBuy)
		{
			GameManager.instance.WarningMessage("Questa costruzione ha già raggiunto il livello massimo!");
			return;
		}
		else if (!hasEnoughMoney)
		{
			GameManager.instance.WarningMessage($"Non hai abbastanza {selectedBuilding.priceTypes[selectedBuilding.currentLevel]} per comprare {selectedBuilding.name}");
			return;
		}
		else if (!hasItems)
		{
			GameManager.instance.WarningMessage("Non hai tutti gli item richiesti!");
			return;
		}
		BuildingsManager.instance.SetBuildingActive(selectedBuilding);
		GameManager.instance.ChangeCounter(selectedBuilding.priceTypes[selectedBuilding.currentLevel], -selectedBuilding.prices[selectedBuilding.currentLevel]);
		selectedBuilding.currentLevel++;
		var buildings = shopPanel.GetComponentsInChildren<ShopBuilding>();
		foreach (var b in buildings)
		{
			b.RefreshInfo();
		}
		CloseInfoPanel();
	}





	public void DisplayItemInfo(Item item)
	{
		itemName.text = item.name;
		description.text = item.description + " " + item.abilityDescription;
		price.text = item.price.ToString();
		amount.text = item.currentAmount + "/" + item.maxAmount;
		icon.GetComponent<Image>().sprite = item.icon;
		amountText.text = "Al momento hai:";
		infoPanel.SetActive(true);
		energyLogo.SetActive(item.priceType == GameManager.Counter.Energia);
		materialsLogo.SetActive(item.priceType == GameManager.Counter.Materiali);
		pointsLogo.SetActive(item.priceType == GameManager.Counter.Punti);
		selectedItem = item;
		selectedBuilding = null;
		hasEnoughMoney = GameManager.instance.GetCounterValue(item.priceType) >= item.price;
		canBuy = item.currentAmount < item.maxAmount;
		buyButton.GetComponentInChildren<TextMeshProUGUI>().color = hasEnoughMoney ? Color.white : Color.red;
		buyButton.GetComponent<Animator>().Play(canBuy ? "Enabled" : "Disabled");
		itemsNeeded.gameObject.SetActive(false);
	}

	public void DisplayBuildingInfo(PlayerBuilding b)
	{
		itemName.text = b.name;
		description.text = b.description;
		amount.text = b.currentLevel + "/" + b.maxLevel;
		icon.GetComponent<Image>().sprite = b.icon;
		amountText.text = "Livello:";
		infoPanel.SetActive(true);
		selectedBuilding = b;
		selectedItem = null;
		canBuy = b.currentLevel < b.maxLevel;

		GameManager.Counter pt;
		int pc, index;
		ItemsNeeded itNeeded;
		if (canBuy)
		{
			index = b.currentLevel;
			pt = b.priceTypes[index];
			pc = b.prices[index];
			itNeeded = b.itemsNeeded[index];
		}
		else
		{
			index = b.currentLevel - 1;
			pt = b.priceTypes[index];
			pc = b.prices[index];
			itNeeded = b.itemsNeeded[index];
		}
		hasItems = GameManager.HasItemsToBuild(b, index);
		price.text = (b.currentLevel > 0 ? "Migliora: " : "Costruisci: ") + pc.ToString();
		energyLogo.SetActive(pt == GameManager.Counter.Energia);
		materialsLogo.SetActive(pt == GameManager.Counter.Materiali);
		pointsLogo.SetActive(pt == GameManager.Counter.Punti);
		hasEnoughMoney = GameManager.instance.GetCounterValue(pt) >= pc;
		buyButton.GetComponentInChildren<TextMeshProUGUI>().color = hasEnoughMoney ? Color.white : Color.red;
		buyButton.GetComponent<Animator>().Play(canBuy && hasItems ? "Enabled" : "Disabled");
		string s = "Item richiesti: ";
		if (itNeeded.items.Length == 0)
		{
			s += "nessuno";
		}
		else
		{
			for (int i = 0; i < itNeeded.items.Length - 1; i++)
			{
				s += itNeeded.items[i].name + ", ";
			}
			s += itNeeded.items[itNeeded.items.Length - 1].name + ".";
		}
		itemsNeeded.text = s;
		itemsNeeded.gameObject.SetActive(true);
	}


	public void CloseInfoPanel()
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");

		infoPanel.SetActive(false);
	}

	public void ToggleShop()
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");
		currentMainScreen = GameManager.MainShopScreen.Costruzioni;
		currentSpecificScreen = GameManager.SpecificShopScreen.Costruzioni;
		shopPanel.SetActive(!shopPanel.activeSelf);
		Camera.main.GetComponent<PanZoom>().enabled = shopPanel.activeSelf;
		SetActivePanels();
	}

	public void ChangeSpecificScreen(int n)
	{
		if (!negozioIllegaleUnlocked && (GameManager.SpecificShopScreen)n == GameManager.SpecificShopScreen.NegozioIllegale)
		{
			GameManager.instance.WarningMessage("Per sbloccare il negozio illegale devi prima trovare la cassa del furfante!");
			return;
		}
		currentSpecificScreen = (GameManager.SpecificShopScreen)n;
		ShopTabs.instance.SetActiveTabs();
		SetActivePanels();
	}

	public void ChangeMainScreen(int n)
	{
		currentMainScreen = (GameManager.MainShopScreen)n;
		if (currentMainScreen == GameManager.MainShopScreen.Costruzioni)
		{
			currentSpecificScreen = GameManager.SpecificShopScreen.Costruzioni;
		}
		else if (currentMainScreen == GameManager.MainShopScreen.Item)
		{
			currentSpecificScreen = GameManager.SpecificShopScreen.Pioneristica;
		}
		ShopTabs.instance.SetActiveTabs();
		SetActivePanels();
	}


	void SetActivePanels()
	{
		pioneristica.SetActive(currentSpecificScreen == GameManager.SpecificShopScreen.Pioneristica);
		cucina.SetActive(currentSpecificScreen == GameManager.SpecificShopScreen.Cucina);
		topografia.SetActive(currentSpecificScreen == GameManager.SpecificShopScreen.Topografia);
		infermieristica.SetActive(currentSpecificScreen == GameManager.SpecificShopScreen.Infermieristica);
		espressione.SetActive(currentSpecificScreen == GameManager.SpecificShopScreen.Espressione);
		negozioIllegale.SetActive(currentSpecificScreen == GameManager.SpecificShopScreen.NegozioIllegale);
		costruzioni.SetActive(currentSpecificScreen == GameManager.SpecificShopScreen.Costruzioni);
		decorazioni.SetActive(currentSpecificScreen == GameManager.SpecificShopScreen.Decorazioni);
	}


}
