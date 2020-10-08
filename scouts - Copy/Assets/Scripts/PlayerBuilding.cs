using UnityEngine;


[CreateAssetMenu(fileName = "New PlayerBuilding", menuName = "PlayerBuilding")]
public class PlayerBuilding : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite icon;
    public int maxLevel;
    public int currentLevel;
    public ItemsNeeded[] itemsNeeded;
    public int[] maxHealth;
    public int[] healthLossInterval;
    public int[] prices; //a price for each level
    public GameManager.Counter[] priceTypes;
    public Vector3 clickListenerOffset;
    public int[] pointsGiven; //different points given for each level

    [Header("Properties")]
    public BuildingProperty[] properties;
}
[System.Serializable]
public class BuildingProperty
{
    public ObjectProperty property;
    public int[] values; //one value foreach level
}
public enum ObjectProperty
{
    changeMaterialsMaxAmount,
    changeEnergyMaxAmount,
    changePointsMaxAmount,
    changeEnergyDropOverTime,
    changePointsIncreaseOverTime,
    changeMaterialsIncreaseOverTime,
    changeEnergyTimeBeforeDrop,
    changePointsTimeBeforeIncrease,
    changeMaterialsTimeBeforeIncrease,
}

[System.Serializable]
public class ItemsNeeded
{
    public Item[] items;
}