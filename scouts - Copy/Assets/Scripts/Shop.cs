using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Shop : MonoBehaviour
{
	public Item[] itemDatabase;
	public PlayerBuilding[] buildingDatabase;
	[HideInInspector] [System.NonSerialized]
	public SpecificShopScreen currentSpecificScreen;
	[HideInInspector] [System.NonSerialized]
	public MainShopScreen currentMainScreen;
	public GameObject shopPanel, pioneristica, cucina, infermieristica, topografia, espressione, negozioIllegale, costruzioni, decorazioni;
	bool hasEnoughMoney, canIncreaseLevel, hasItems, canBuy;
	[HideInInspector] [System.NonSerialized]
	public bool negozioIllegaleUnlocked;
	public Joystick joy;

	TextMeshProUGUI objName, description, price, amount, amountHeader, level, levelHeader, itemsNeeded;
	public GameObject infoPanel;
	GameObject materialsLogo, pointsLogo, energyLogo, icon, buyButton;

	object selected;

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
		currentMainScreen = MainShopScreen.Costruzioni;
		currentSpecificScreen = SpecificShopScreen.Pioneristica;
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
		OrganizeObjects(pioneristica, SpecificShopScreen.Pioneristica);
		OrganizeObjects(cucina, SpecificShopScreen.Cucina);
		OrganizeObjects(infermieristica, SpecificShopScreen.Infermieristica);
		OrganizeObjects(negozioIllegale, SpecificShopScreen.NegozioIllegale);
		OrganizeObjects(costruzioni, SpecificShopScreen.Costruzioni);
		OrganizeObjects(decorazioni, SpecificShopScreen.Decorazioni);
		OrganizeObjects(topografia, SpecificShopScreen.Topografia);

		SetStatus(SaveSystem.instance.LoadData<Status>(SaveSystem.instance.shopFileName, false));
	}

	public Status SendStatus()
	{
		var items = new ObjectBase.Status[itemDatabase.Length];
		for (int i = 0; i < items.Length; i++)
			items[i] = itemDatabase[i].SendStatus();
		var buildings = new ObjectBase.Status[buildingDatabase.Length];
		for (int i = 0; i < buildings.Length; i++)
			buildings[i] = buildingDatabase[i].SendStatus();
		return new Status
		{
			itemDatabase = items,
			buildingDatabase = buildings
		};
	}
	void SetStatus(Status status)
	{
		if (status != null)
		{
			for (int i = 0; i < itemDatabase.Length; i++)
			{
				itemDatabase[i].SetStatus(status.itemDatabase[i]);
			}
			for (int i = 0; i < buildingDatabase.Length; i++)
			{
				buildingDatabase[i].SetStatus(status.buildingDatabase[i]);
			}
		}
	}
	public class Status
	{
		public ObjectBase.Status[] itemDatabase;
		public ObjectBase.Status[] buildingDatabase;
	}

	void OrganizeObjects(GameObject panel, SpecificShopScreen screen)
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
		var index = ((ObjectBase)selected).exists ? ((ObjectBase)selected).level + 1 : ((ObjectBase)selected).level;
		if (((ObjectBase)selected).type == ObjectType.Item && InventoryManager.instance.IsInventoryFull())
		{
			GameManager.instance.WarningOrMessage("L'inventario è pieno!", true);
			return;
		}
		else if (!canIncreaseLevel)
		{
			GameManager.instance.WarningOrMessage("Questo oggetto ha già raggiunto il livello massimo!", true);
			return;
		}
		else if (!hasEnoughMoney)
		{
			GameManager.instance.WarningOrMessage($"Non hai abbastanza {((ObjectBase)selected).shopInfos[index].priceCounter} per comprare {((ObjectBase)selected).name}", true);
			return;
		}
		else if (!hasItems)
		{
			GameManager.instance.WarningOrMessage("Non hai tutti gli item richiesti!", true);
			return;
		}
		else if (!canBuy)
		{
			GameManager.instance.WarningOrMessage("Hai già acquistato il numero massimo di questi oggetti!", true);
			return;
		}
		
		GameManager.instance.ChangeCounter(((ObjectBase)selected).shopInfos[index].priceCounter, -((ObjectBase)selected).shopInfos[index].price);
		GameManager.instance.ChangeCounter(((ObjectBase)selected).shopInfos[index].rewardCounter, ((ObjectBase)selected).shopInfos[index].reward);

		if (((ObjectBase)selected).usingAmount)
			((ObjectBase)selected).currentAmount++;
		if (((ObjectBase)selected).usingLevel)
		{
			if (((ObjectBase)selected).exists)
				((ObjectBase)selected).level++;
			else
				((ObjectBase)selected).exists = true;
		}

		GameManager.DestroyItemsNeededToBuyItem(((ObjectBase)selected));

		if (((ObjectBase)selected).type == ObjectType.Item)
		{
			InventoryManager.instance.Add(((ObjectBase)selected));
		}
		else if (((ObjectBase)selected).type == ObjectType.Costruzione)
		{
			ToggleShop();

			ModificaBaseTrigger.instance.SetBuildingSlotInfo((PlayerBuilding)selected);
			GameManager.instance.Built(((ObjectBase)selected));
		}
		foreach (var o in shopPanel.GetComponentsInChildren<ShopObjectBase>())
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
		if (((ObjectBase)selected).type == ObjectType.Item)
			price.text = "Compra: " + pc.ToString();
		else if (((ObjectBase)selected).type == ObjectType.Costruzione)
			price.text = (o.exists ? "Migliora: " : "Costruisci: ") + pc.ToString();
		energyLogo.SetActive(pt == Counter.Energia);
		materialsLogo.SetActive(pt == Counter.Materiali);
		pointsLogo.SetActive(pt == Counter.Punti);
		hasEnoughMoney = GameManager.instance.GetCounterValue(pt) >= pc;
		buyButton.GetComponentInChildren<TextMeshProUGUI>().color = hasEnoughMoney ? UnityEngine.Color.white : UnityEngine.Color.red;
		buyButton.GetComponent<Animator>().Play(canIncreaseLevel && hasItems && canBuy ? "Enabled" : "Disabled");

		amount.text = o.currentAmount + "/" + o.maxAmount;
		level.text = (o.exists ? (o.level + 1) : o.level) + "/" + (o.maxLevel + 1);
		amountHeader.text = "Al momento hai:";
		levelHeader.text = "Livello:";
		level.gameObject.SetActive(o.usingLevel);
		levelHeader.gameObject.SetActive(o.usingLevel);
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
					s += itNeeded.items[i].item.name + (itNeeded.items[i].amount > 1 ? $" (x{itNeeded.items[i].amount})" : "") + ", ";
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
		currentMainScreen = MainShopScreen.Costruzioni;
		currentSpecificScreen = SpecificShopScreen.Costruzioni;
		shopPanel.SetActive(!shopPanel.activeSelf);
		Camera.main.GetComponent<PanZoom>().enabled = !shopPanel.activeSelf;
		SetActivePanels();
	}
	public void ReEnableJoy()
	{
		joy.canUseJoystick = true;
	}

	public void DisableJoy()
	{
		joy.canUseJoystick = false;
	}
	public void ChangeSpecificScreen(int n)
	{
		if (!negozioIllegaleUnlocked && (SpecificShopScreen)n == SpecificShopScreen.NegozioIllegale)
		{
			GameManager.instance.WarningOrMessage("Per sbloccare il negozio illegale devi prima trovare la cassa del furfante!", true);
			return;
		}
		currentSpecificScreen = (SpecificShopScreen)n;
		ShopTabs.instance.SetActiveTabs();
		SetActivePanels();
	}

	public void ChangeMainScreen(int n)
	{
		currentMainScreen = (MainShopScreen)n;
		if (currentMainScreen == MainShopScreen.Costruzioni)
		{
			currentSpecificScreen = SpecificShopScreen.Costruzioni;
		}
		else if (currentMainScreen == MainShopScreen.Item)
		{
			currentSpecificScreen = SpecificShopScreen.Pioneristica;
		}
		ShopTabs.instance.SetActiveTabs();
		SetActivePanels();
	}


	void SetActivePanels()
	{
		pioneristica.SetActive(currentSpecificScreen == SpecificShopScreen.Pioneristica);
		cucina.SetActive(currentSpecificScreen == SpecificShopScreen.Cucina);
		topografia.SetActive(currentSpecificScreen == SpecificShopScreen.Topografia);
		infermieristica.SetActive(currentSpecificScreen == SpecificShopScreen.Infermieristica);
		espressione.SetActive(currentSpecificScreen == SpecificShopScreen.Espressione);
		negozioIllegale.SetActive(currentSpecificScreen == SpecificShopScreen.NegozioIllegale);
		costruzioni.SetActive(currentSpecificScreen == SpecificShopScreen.Costruzioni);
		decorazioni.SetActive(currentSpecificScreen == SpecificShopScreen.Decorazioni);
	}
}
