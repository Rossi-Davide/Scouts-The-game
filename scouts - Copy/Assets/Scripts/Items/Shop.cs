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
	bool hasEnoughMoney, canBuy;
	[HideInInspector]
	public bool negozioIllegaleUnlocked;


	TextMeshProUGUI itemName, description, price, amount;
	public GameObject infoPanel;
	GameObject materialsLogo, pointsLogo, energyLogo, icon, buyButton;



	Item selectedItem;
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
		price = buyButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
		amount = infoPanel.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
	}
	public void Buy()
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
		var items = FindObjectsOfType<ShopItem>();
		foreach (var i in items)
		{
			i.RefreshInfo();
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
		infoPanel.SetActive(true);
		energyLogo.SetActive(item.priceType == GameManager.Counter.Energia);
		materialsLogo.SetActive(item.priceType == GameManager.Counter.Materiali);
		pointsLogo.SetActive(item.priceType == GameManager.Counter.Punti);
		selectedItem = item;
		if (GameManager.instance.GetCounterValue(item.priceType) < item.price)
			hasEnoughMoney = false;
		else
			hasEnoughMoney = true;
		if (item.currentAmount < item.maxAmount)
			canBuy = true;
		else
			canBuy = false;
		buyButton.GetComponentInChildren<TextMeshProUGUI>().color = hasEnoughMoney ? Color.white : Color.red;
		buyButton.GetComponent<Animator>().Play(canBuy ? "Enabled" : "Disabled");
	}

	public void CloseInfoPanel()
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");

		infoPanel.SetActive(false);
	}

	public void ToggleShop()
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");
		shopPanel.SetActive(!shopPanel.activeSelf);
		currentMainScreen = GameManager.MainShopScreen.Costruzioni;
		currentSpecificScreen = GameManager.SpecificShopScreen.Costruzioni;
		Camera.main.GetComponent<PanZoom>().enabled = shopPanel.activeSelf;
		SetActivePanels();
	}

	public bool ChangeSpecificScreen(int n)
	{
		if (!negozioIllegaleUnlocked && (GameManager.SpecificShopScreen)n == GameManager.SpecificShopScreen.NegozioIllegale)
		{
			GameManager.instance.WarningMessage("Per sbloccare il negozio illegale devi prima trovare la cassa del furfante!");
			return false;
		}
		currentSpecificScreen = (GameManager.SpecificShopScreen)n;
		SetActivePanels();
		return true;
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
