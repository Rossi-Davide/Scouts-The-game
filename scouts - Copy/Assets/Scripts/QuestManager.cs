using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	public GameObject questPanel, overlay;
	[HideInInspector]
	[System.NonSerialized]
	public QuestUI[] quests;
	public PlayerAction[] actionDatabase;
	bool isOpen;
	SaveSystem saveSystem;
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
		GameManager.instance.OnBuild += RefreshActions;
		saveSystem = SaveSystem.instance;
	}

	public Status SendStatus()
	{
		var qs = new Quest.Status[quests.Length];
		for (int q = 0; q < quests.Length; q++)
		{
			qs[q] = quests[q].quest.SendStatus();
		}
		var actions = new PlayerAction.Status[actionDatabase.Length];
		for (int a = 0; a < actionDatabase.Length; a++)
		{
			actions[a] = actionDatabase[a].SendStatus();
		}
		return new Status
		{
			quests = qs,
			actions = actions,
		};
	}
	void SetStatus(Status status)
	{
		if (status != null)
		{
			for (int q = 0; q < status.quests.Length; q++)
			{
				quests[q].quest.SetStatus(status.quests[q]);
			}
			for (int a = 0; a < status.actions.Length; a++)
			{
				actionDatabase[a].SetStatus(status.actions[a]);
			}
		}
	}
	public class Status
	{
		public Quest.Status[] quests;
		public PlayerAction.Status[] actions;
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
	void RefreshActions(ObjectBase obj)
	{
		bool isABuilding = false;
		foreach (var b in SquadrigliaManager.instance.GetPlayerSq().buildings)
		{
			if (b.GetComponent<PlayerBuildingBase>().building == obj)
				isABuilding = true;
		}
		bool removed = !InventoryManager.instance.Contains(obj.ToItem()) && !ChestManager.instance.Contains(obj.ToItem()) && !isABuilding;
		ChangeActionParameter(obj, removed);
		ChangeCountersMaxValue(obj, removed);
	}
	void ChangeCountersMaxValue(ObjectBase obj, bool removed)
	{
		if (obj != null && obj.changedMaxAmounts.Length > 0)
		{
			var o = obj.changedMaxAmounts[obj.level];
			GameManager.instance.CounterMaxValueChanged(o.counter, removed ? -o.delta : o.delta);
		}
	}



	void ChangeActionParameter(ObjectBase obj, bool removed)
	{
		if (obj != null && obj.modifiedActions.Length > 0)
		{
			var a = obj.modifiedActions[obj.level].action;
			var n = obj.modifiedActions[obj.level].delta;
			if (removed) { n = -n; }
			if (!obj.modifiedActions[obj.level].hasToBeInInventory)
			{
				switch (obj.modifiedActions[obj.level].parameter)
				{
					case PlayerAction.ActionParams.timeNeeded:
						a.timeNeeded += n;
						break;
					case PlayerAction.ActionParams.energyGiven:
						a.energyGiven += n;
						break;
					case PlayerAction.ActionParams.materialsGiven:
						a.materialsGiven += n;
						break;
					case PlayerAction.ActionParams.pointsGiven:
						a.pointsGiven += n;
						break;
					case PlayerAction.ActionParams.timeBeforeRedo:
						a.timeBeforeRedo += n;
						break;
				}
			}
		}
	}

	public void ToggleQuestPanel()
	{
		isOpen = !isOpen;
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(isOpen ? "click" : "clickDepitched");

		overlay.SetActive(isOpen);
		questPanel.SetActive(isOpen);
		PanZoom.instance.canDo = !isOpen;
		Joystick.instance.enabled = !isOpen;

		foreach (QuestUI q in quests)
		{
			q.RefreshQuest();
		}
	}
}
