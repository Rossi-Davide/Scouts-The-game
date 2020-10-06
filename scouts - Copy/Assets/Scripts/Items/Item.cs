using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
	public new string name;
	public string description;
	public string abilityDescription;
	public Sprite icon;

	[Header("Item and shop")]
	public ItemType type;
	public GameManager.SpecificShopScreen shopScreen;
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
	public bool hasToBeInInventory;
	public PlayerAction.ActionParams modifiedParameter;
	public int newValue;
	[Header("ShopScreen")]
	public GameManager.SpecificShopScreen screenUnlocked;

	public enum ItemType
	{
		Oggetto,
	}
	public GameManager.PeriodicActionInterval PeriodicUse => periodicUseInterval;
	public void DoAction()
	{
		if (changedCounter != GameManager.Counter.None)
		{
			GameManager.instance.ChangeCounter(changedCounter, deltaPoints);
		}
	}
}