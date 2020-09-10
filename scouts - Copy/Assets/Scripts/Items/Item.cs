using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
	public new string name;
	public string description;
	public string abilityDescription;
	public Sprite icon;

	[Header("Item and shop")]
	public Type type;
	public GameManager.ShopScreen shopScreen;
	public int currentAmount;
	public int maxAmount;
	public Item[] neededItems;
	public int price;
	public GameManager.Counter priceType;

	[Header("PeriodicUse")]
	public GameManager.PeriodicActionInterval periodicUseInterval;
	public GameManager.Counter changedCounter;
	public int deltaPoints;
	[Header("Modified Action")]
	public PlayerAction modifiedAction;
	public PlayerAction.ActionParams modifiedParameter;
	public int newValue;
	[Header("ShopScreen")]
	public GameManager.ShopScreen screenUnlocked;
	[Header("Buildings")]
	public GameObject realBuilding;
	public int timeRequired;
	public int pointsGiven;

	public enum Type
	{
		Costruzione,
		Oggetto,
		OggettoSpeciale,
	}

	public GameManager.PeriodicActionInterval PeriodicUse => periodicUseInterval;
	public void DoAction()
	{
		if (changedCounter != GameManager.Counter.None && type == Type.Oggetto)
		{
			GameManager.instance.ChangeCounter(changedCounter, deltaPoints);
		}
		if (type == Type.Costruzione)
		{
			InventoryManager.instance.ToggleInventoryPanel();
			FindObjectOfType<ModificaBaseTrigger>().ToggleModificaBase();
			realBuilding.SetActive(true);
		}
	}
}