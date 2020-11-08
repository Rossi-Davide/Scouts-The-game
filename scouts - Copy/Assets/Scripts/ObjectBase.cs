using UnityEngine;

public abstract class ObjectBase : ScriptableObject
{
    public new string name;
    public ObjectType type;
    public string description;
    public Sprite icon;
    public SpecificShopScreen shopScreen;
    public int maxAmount;
    public int maxLevel; //index
    public bool usingLevel;
    public bool usingAmount;


    public ItemsNeeded[] itemsNeededs;
    public ShopInfo[] shopInfos;
    public PeriodicUse[] periodicUses;
    public ModifiedAction[] modifiedActions;
    public ChangedCounterMaxAmount[] changedMaxAmounts;

    [Header("Please do not modify if not testing")]
    public int currentAmount;
    public int level; //index
    public bool exists;

    public virtual void DoAction()
    {
        if (periodicUses[level].counter != Counter.None)
        {
            GameManager.instance.ChangeCounter(periodicUses[level].counter, periodicUses[level].delta);
        }
    }

    public Item ToItem()
	{
        return new Item(this);
	}
    public PlayerBuilding ToPlayerBuilding()
    {
        return new PlayerBuilding(this);
    }

}

[System.Serializable]
public class ItemsNeeded
{
    public ItemNeeded[] items;
}
[System.Serializable]
public class ItemNeeded
{
    public Item item;
    public int amount;
    public bool isDestroyed;
}

[System.Serializable]
public class ShopInfo
{
    public int price;
    public Counter priceCounter;
    public int reward;
    public Counter rewardCounter;
}

[System.Serializable]
public class PeriodicUse
{
    public PeriodicActionInterval interval;
    public Counter counter;
    public int delta;
}

[System.Serializable]
public class ModifiedAction
{
    public PlayerAction action;
    public bool hasToBeInInventory;
    public PlayerAction.ActionParams parameter;
    public int delta;
}

[System.Serializable]
public class ChangedCounterMaxAmount
{
    public Counter counter;
    public int delta;
}
public enum ObjectType
{
    Item,
    Costruzione,
    Decorazione,
}
