using UnityEngine;
using UnityEngine.Rendering;

public class QuestManager : MonoBehaviour
{
	public GameObject questPanel, overlay;
	[HideInInspector]
	public QuestUI[] quests;
	public PlayerAction[] actionDatabase;
	bool isOpen;

	#region Singleton
	public static QuestManager instance;
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("QuestManager is not a singleton");
		instance = this;
	}
	#endregion



	private void Start()
	{
		quests = questPanel.GetComponentsInChildren<QuestUI>();
		GameManager.instance.OnActionDo += RefreshQuests;
		GameManager.instance.OnInventoryChange += RefreshActions;
	}

	void RefreshQuests(PlayerAction a)
	{
		foreach (var q in quests)
		{
			if (q.quest.action == a)
			{
				q.quest.timesDone++;
				q.RefreshQuest();
			}
		}
	}
	void RefreshActions()
	{
		IterateChestOrInventory(InventoryManager.instance.slots);
		IterateChestOrInventory(ChestManager.instance.slots);
	}
	void IterateChestOrInventory(InventorySlot[] items)
	{
		foreach (var i in items)
		{
			if (i.item != null && i.item.modifiedAction != null)
			{
				var a = i.item.modifiedAction;
				var n = i.item.newValue;
				if (!i.item.hasToBeInInventory)
				{
					switch (i.item.modifiedParameter)
					{
						case PlayerAction.ActionParams.timeNeeded:
							a.timeNeeded = n;
							break;
						case PlayerAction.ActionParams.energyGiven:
							a.energyGiven = n;
							break;
						case PlayerAction.ActionParams.materialsGiven:
							a.materialsGiven = n;
							break;
						case PlayerAction.ActionParams.pointsGiven:
							a.pointsGiven = n;
							break;
						case PlayerAction.ActionParams.timeBeforeRedo:
							a.timeBeforeRedo = n;
							break;
					}
				}
			}
		}
	}



	public void ToggleQuestPanel()
	{

		if (!isOpen)
		{
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

			overlay.SetActive(true);
			questPanel.SetActive(true);

			isOpen = true;
			foreach (QuestUI q in quests)
			{
				q.RefreshQuest();
			}
		}
		else
		{
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");
			isOpen = false;
			questPanel.SetActive(false);
			overlay.SetActive(false);
		}
	}
}
