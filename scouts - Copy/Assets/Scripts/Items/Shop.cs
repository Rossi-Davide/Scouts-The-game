using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Shop : MonoBehaviour
{
	public Item[] itemDatabase;
	public PlayerBuilding[] buildingDatabase;
	[HideInInspector]
	public GameManager.SpecificShopScreen currentSpecificScreen;
	[HideInInspector]
	public GameManager.MainShopScreen currentMainScreen;
	public GameObject shopPanel, pioneristica, cucina, infermieristica, topografia, espressione, negozioIllegale, costruzioni, decorazioni;
	bool hasEnoughMoney, canIncreaseLevel, hasItems, canBuy;
	[HideInInspector]
	public bool negozioIllegaleUnlocked;


	TextMeshProUGUI objName, description, price, amount, amountHeader, level, levelHeader, itemsNeeded;
	public GameObject infoPanel;
	GameObject materialsLogo, pointsLogo, energyLogo, icon, buyButton;

	ObjectBase selected;

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
		objName = infoPanel.transform.Find("Name").GetComponent<TextMeshProUGUI>();
		description = infoPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>();
		itemsNeeded = infoPanel.transform.Find("ItemsNeeded").GetComponent<TextMeshProUGUI>();
		price = buyButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
		amount = infoPanel.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
		amountHeader = infoPanel.transform.Find("Amount/Text").GetComponent<TextMeshProUGUI>();
		level = infoPanel.transform.Find("Level").GetComponent<TextMeshProUGUI>();
		levelHeader = infoPanel.transform.Find("Level/Text").GetComponent<TextMeshProUGUI>();
		buyButton.GetComponent<Button>().onClick.AddListener(Buy);
		OrganizeObjects(pioneristica, GameManager.SpecificShopScreen.Pioneristica);
		OrganizeObjects(cucina, GameManager.SpecificShopScreen.Cucina);
		OrganizeObjects(infermieristica, GameManager.SpecificShopScreen.Infermieristica);
		OrganizeObjects(negozioIllegale, GameManager.SpecificShopScreen.NegozioIllegale);
		OrganizeObjects(costruzioni, GameManager.SpecificShopScreen.Costruzioni);
		OrganizeObjects(decorazioni, GameManager.SpecificShopScreen.Decorazioni);
		OrganizeObjects(topografia, GameManager.SpecificShopScreen.Topografia);
	}

	void OrganizeObjects(GameObject panel, GameManager.SpecificShopScreen screen)
	{
		int counter = 0;
		var slots = panel.GetComponentsInChildren<ShopObjectBase>();
		foreach (var o in itemDatabase)
		{
			if (o.shopScreen == screen)
			{
				slots[counter].obj = o;
				counter++;
			}
		}
		foreach (var o in buildingDatabase)
		{
			if (o.shopScreen == screen)
			{
				slots[counter].obj = o;
				counter++;
			}
		}
	}



	void Buy()
	{
		var nextIndex = selected.exists ? selected.level + 1 : selected.level;
		if (selected.type == ObjectType.Item && InventoryManager.instance.IsInventoryFull())
		{
			GameManager.instance.WarningMessage("L'inventario è pieno!");
			return;
		}
		else if (!canIncreaseLevel)
		{
			GameManager.instance.WarningMessage("Questo oggetto ha già raggiunto il livello massimo!");
			return;
		}
		else if (!hasEnoughMoney)
		{
			GameManager.instance.WarningMessage($"Non hai abbastanza {selected.shopInfos[nextIndex].priceCounter} per comprare {selected.name}");
			return;
		}
		else if (!hasItems)
		{
			GameManager.instance.WarningMessage("Non hai tutti gli item richiesti!");
			return;
		}
		else if (!canBuy)
		{
			GameManager.instance.WarningMessage("Hai già acquistato il numero massimo di questi oggetti!");
			return;
		}
		
		GameManager.instance.ChangeCounter(selected.shopInfos[nextIndex].priceCounter, -selected.shopInfos[nextIndex].price);
		GameManager.instance.ChangeCounter(selected.shopInfos[nextIndex].rewardCounter, selected.shopInfos[nextIndex].reward);

		if (selected.usingAmount)
			selected.currentAmount++;
		if (selected.usingLevel)
		{
			if (selected.exists)
				selected.level++;
			else
				selected.exists = true;
		}

		GameManager.DestroyItems(selected);

		if (selected.type == ObjectType.Item)
		{
			InventoryManager.instance.Add(selected.ToItem());
		}
		else if (selected.type == ObjectType.Costruzione)
		{
			ToggleShop();
			ModificaBaseTrigger.instance.SetBuildingSlotInfo(selected.ToPlayerBuilding());
			GameManager.instance.Built(selected);
		}
		foreach (var o in GetComponentsInChildren<ShopObjectBase>())
		{
			o.RefreshInfo();
		}
		CloseInfoPanel();
	}

	public void DisplayInfo(ObjectBase o)
	{
		selected = o;
		objName.text = o.name;
		description.text = o.description;
		icon.GetComponent<Image>().sprite = o.icon;
		infoPanel.SetActive(true);
		canIncreaseLevel = !o.usingLevel || o.level < o.maxLevel;
		canBuy = !o.usingAmount || o.currentAmount < o.maxAmount;

		Counter pt;
		int pc, index;

		if (o.exists)
		{
			if (canIncreaseLevel)
				index = o.level;
			else
				index = o.level - 1;
		}
		else
			index = o.level;

		pt = o.shopInfos[index].priceCounter;
		pc = o.shopInfos[index].price;
		var itNeeded = (o.exists && o.itemsNeededs.Length > o.level + 1) || (!o.exists && o.itemsNeededs.Length > o.level) ? o.itemsNeededs[index] : null;

		hasItems = GameManager.HasItemsToBuy(o);
		if (selected.type == ObjectType.Item)
			price.text = "Compra: " + pc.ToString();
		else if (selected.type == ObjectType.Costruzione)
			price.text = (o.exists ? "Migliora: " : "Costruisci: ") + pc.ToString();
		energyLogo.SetActive(pt == Counter.Energia);
		materialsLogo.SetActive(pt == Counter.Materiali);
		pointsLogo.SetActive(pt == Counter.Punti);
		hasEnoughMoney = GameManager.instance.GetCounterValue(pt) >= pc;
		buyButton.GetComponentInChildren<TextMeshProUGUI>().color = hasEnoughMoney ? Color.white : Color.red;
		buyButton.GetComponent<Animator>().Play(canIncreaseLevel && hasItems && canBuy ? "Enabled" : "Disabled");

		amount.text = o.currentAmount + "/" + o.maxAmount;
		level.text = (o.exists ? (o.level + 1) : o.level) + "/" + (o.maxLevel + 1);
		amountHeader.text = "Al momento hai:";
		levelHeader.text = "Livello:";
		level.gameObject.SetActive(o.usingAmount);
		levelHeader.gameObject.SetActive(o.usingAmount);
		amount.gameObject.SetActive(o.usingAmount);
		amountHeader.gameObject.SetActive(o.usingAmount);
		if (itNeeded != null)
		{
			string s = "Item richiesti: ";
			if (itNeeded.items.Length == 0)
			{
				s += "nessuno";
			}
			else
			{
				for (int i = 0; i < itNeeded.items.Length - 1; i++)
				{
					s += itNeeded.items[i].item.name + ", ";
				}
				s += itNeeded.items[itNeeded.items.Length - 1].item.name + ".";
			}
			itemsNeeded.text = s;
			itemsNeeded.gameObject.SetActive(true);
		}
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
		Camera.main.GetComponent<PanZoom>().enabled = !shopPanel.activeSelf;
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
