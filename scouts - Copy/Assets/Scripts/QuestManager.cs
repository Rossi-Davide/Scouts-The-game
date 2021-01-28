using System;
using System.Collections;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	public GameObject questPanel, overlay;
	public Joystick joy;
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

	public void ReEnableJoy()
	{
		joy.canUseJoystick = true;
	}

	public void DisableJoy()
	{
		joy.canUseJoystick = false;
	}

	private void Start()
	{
		quests = questPanel.GetComponentsInChildren<QuestUI>();
		GameManager.instance.OnActionDo += RefreshQuests;
		GameManager.instance.OnInventoryChange += RefreshAllActions;
		GameManager.instance.OnBuild += RefreshAllActions;
		GameManager.instance.OnInventoryChange += RefreshCountersMaxValues;
		GameManager.instance.OnBuild += RefreshCountersMaxValues;
		saveSystem = SaveSystem.instance;
		StartCoroutine(GetAllStartInfo());
		SetStatus(saveSystem.LoadData<Status>(saveSystem.questManagerFileName, false));
	}

	public PlayerAction GetActionByName(string name)
	{
		return Array.Find(actionDatabase, el => el.name == name);
	}
	IEnumerator GetAllStartInfo()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		foreach (var a in actionDatabase)
		{
			a.ResetEditableInfo();
		}
		foreach (var q in quests)
		{
			q.quest.ResetEditableInfo();
		}
		RefreshAllActions();
		RefreshCountersMaxValues();
	}

	public Status SendStatus()
	{
		var qs = new Quest.Status[quests.Length];
		for (int q = 0; q < quests.Length; q++)
		{
			qs[q] = quests[q].quest.SendStatus();
		}
		return new Status
		{
			quests = qs,
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
		}
	}
	public class Status
	{
		public Quest.Status[] quests;
	}

	void RefreshQuests(PlayerAction a)
	{
		foreach (var q in quests)
		{
			if (q.quest.action == a)
			{
				q.quest.timesDone++;
			}
		}
	}
	//void RefreshAction(ObjectBase obj)
	//{
	//	bool isABuilding = false;
	//	foreach (var b in SquadrigliaManager.instance.GetPlayerSq().buildings)
	//	{
	//		if (b.GetComponent<PlayerBuildingBase>().building == obj)
	//			isABuilding = true;
	//	}
	//	bool removed = !InventoryManager.instance.Contains(obj) && !ChestManager.instance.Contains(obj) && !isABuilding;
	//	ChangeActionParameter(obj, removed);
	//	ChangeCountersMaxValue(obj, removed);
	//}

	void RefreshAllActions()
	{
		foreach (var slot in InventoryManager.instance.slots)
		{
			if (slot.item != null && slot.item.modifiedActions.Length > 0)
			{
				var m = slot.item.modifiedActions[slot.item.level];
				ChangeActionParameter(m.action, m.delta, m.parameter);
			}
		}
		foreach (var slot in ChestManager.instance.slots)
		{
			if (slot.item != null && slot.item.modifiedActions.Length > 0)
			{
				var m = slot.item.modifiedActions[slot.item.level];
				ChangeActionParameter(m.action, m.delta, m.parameter);
			}
		}
		foreach (var bld in SquadrigliaManager.instance.GetPlayerSq().buildings)
		{
			var b = bld.GetComponent<PlayerBuildingBase>().building;
			if (b.modifiedActions.Length > 0)
			{
				var m = b.modifiedActions[b.level];
				ChangeActionParameter(m.action, m.delta, m.parameter);
			}
		}
	}
	void RefreshCountersMaxValues()
	{
		foreach (var slot in InventoryManager.instance.slots)
		{
			if (slot.item != null && slot.item.changedMaxAmounts.Length > 0)
			{
				var m = slot.item.changedMaxAmounts[slot.item.level];
				GameManager.instance.ChangeCounterMaxValue(m.counter, m.delta);
			}
		}
		foreach (var slot in ChestManager.instance.slots)
		{
			if (slot.item != null && slot.item.changedMaxAmounts.Length > 0)
			{
				var m = slot.item.changedMaxAmounts[slot.item.level];
				GameManager.instance.ChangeCounterMaxValue(m.counter, m.delta);
			}
		}
		foreach (var bld in SquadrigliaManager.instance.GetPlayerSq().buildings)
		{
			var b = bld.GetComponent<PlayerBuildingBase>().building;
			if (b.changedMaxAmounts.Length > 0)
			{
				var m = b.changedMaxAmounts[b.level];
				GameManager.instance.ChangeCounterMaxValue(m.counter, m.delta);
			}
		}
	}

	void ChangeActionParameter(PlayerAction a, int delta, PlayerAction.ActionParams parameter)
	{
		switch (parameter)
		{
			case PlayerAction.ActionParams.timeNeeded:
				a.EditableTimeNeeded += delta;
				break;
			case PlayerAction.ActionParams.energyGiven:
				a.EditableEnergyGiven += delta;
				break;
			case PlayerAction.ActionParams.materialsGiven:
				a.EditableMaterialsGiven += delta;
				break;
			case PlayerAction.ActionParams.pointsGiven:
				a.EditablePointsGiven += delta;
				break;
			case PlayerAction.ActionParams.timeBeforeRedo:
				a.EditableTimeBeforeRedo += delta;
				break;
		}
	}
	public void ToggleQuestPanel()
	{
		isOpen = !isOpen;
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(isOpen ? "click" : "clickDepitched");

		overlay.SetActive(isOpen);
		questPanel.SetActive(isOpen);
		PanZoom.instance.canDo = !isOpen;

		foreach (QuestUI q in quests)
		{
			q.RefreshQuest();
		}
	}
}
