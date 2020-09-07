using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	[HideInInspector]
	public GameManager.ShopScreen currentScreen;
	public GameObject itemsPanel, shopPanel, costruzioniEPioneristicaPanel, cucinaPanel, altreCassePanel, negozioIllegalePanel, closeButton;
	bool hasEnoughMoney, canBuy;

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
		currentScreen = GameManager.ShopScreen.Costruzioni;
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

	TextMeshProUGUI itemName, description, price, amount;
	public GameObject infoPanel;
	GameObject materialsLogo, pointsLogo, energyLogo, icon, buyButton;

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
		if (GameManager.instance.CheckCounterValue(item.priceType) < item.price)
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

		itemsPanel.SetActive(!itemsPanel.activeSelf);
		shopPanel.SetActive(!shopPanel.activeSelf);
		costruzioniEPioneristicaPanel.SetActive(currentScreen == GameManager.ShopScreen.Costruzioni || currentScreen == GameManager.ShopScreen.Pioneristica);
		cucinaPanel.SetActive(currentScreen == GameManager.ShopScreen.Cucina);
		altreCassePanel.SetActive(currentScreen == GameManager.ShopScreen.Infermieristica || currentScreen == GameManager.ShopScreen.Topografia || currentScreen == GameManager.ShopScreen.Espressione);
		negozioIllegalePanel.SetActive(currentScreen == GameManager.ShopScreen.NegozioIllegale);
		closeButton.SetActive(!closeButton.activeSelf);
	}

	public void ChangePanel(int n)
	{
		currentScreen = (GameManager.ShopScreen)n;
		costruzioniEPioneristicaPanel.SetActive(currentScreen == GameManager.ShopScreen.Costruzioni || currentScreen == GameManager.ShopScreen.Pioneristica);
		cucinaPanel.SetActive(currentScreen == GameManager.ShopScreen.Cucina);
		altreCassePanel.SetActive(currentScreen == GameManager.ShopScreen.Infermieristica || currentScreen == GameManager.ShopScreen.Topografia || currentScreen == GameManager.ShopScreen.Espressione);
		negozioIllegalePanel.SetActive(currentScreen == GameManager.ShopScreen.NegozioIllegale);
	}
}
