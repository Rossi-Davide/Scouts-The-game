using UnityEngine;


[CreateAssetMenu(fileName = "New PlayerBuilding", menuName = "PlayerBuilding")]
public class PlayerBuilding : ObjectBase
{
    public HealthInfo[] healthInfos;


	public PlayerBuilding(ObjectBase obj)
	{
		name = obj.name;
		description = obj.description;
		type = obj.type;
		icon = obj.icon;
		shopScreen = obj.shopScreen;
		maxAmount = obj.maxAmount;
		maxLevel = obj.maxLevel;
		usingLevel = obj.usingLevel;
		usingAmount = obj.usingAmount;
		itemsNeededs = obj.itemsNeededs;
		shopInfos = obj.shopInfos;
		periodicUses = obj.periodicUses;
		modifiedActions = obj.modifiedActions;
		changedMaxAmounts = obj.changedMaxAmounts;
		currentAmount = obj.currentAmount;
		level = obj.level;
	}
}
[System.Serializable]
public class HealthInfo
{
    public int maxHealth;
    public int healthLossInterval;
}