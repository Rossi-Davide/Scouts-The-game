using UnityEngine;


[CreateAssetMenu(fileName = "New PlayerBuilding", menuName = "PlayerBuilding")]
public class PlayerBuilding : ObjectBase
{
    public HealthInfo[] healthInfos;
    public Vector3 clickListenerOffset;


	public PlayerBuilding(ObjectBase obj)
	{
		name = obj.name;
		description = obj.description;
		type = obj.type;
		icon = obj.icon;
		shopScreen = obj.shopScreen;
		maxAmount = obj.maxAmount;
		maxLevel = obj.maxLevel;
		showLevel = obj.showLevel;
		usingAmount = obj.usingAmount;
		itemsNeededs = obj.itemsNeededs;
		shopInfos = obj.shopInfos;
		periodicUses = obj.periodicUses;
		modifiedActions = obj.modifiedActions;
		changedMaxAmounts = obj.changedMaxAmounts;
		currentAmount = obj.currentAmount;
		currentLevel = obj.currentLevel;
	}


}
[System.Serializable]
public class HealthInfo
{
    public int maxHealth;
    public int healthLossInterval;
}