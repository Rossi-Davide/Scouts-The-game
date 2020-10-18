using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ObjectBase
{
	public Item(ObjectBase obj)
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