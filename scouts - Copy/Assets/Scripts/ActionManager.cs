using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

public class ActionManager : MonoBehaviour
{
	#region Singleton
	public static ActionManager instance;
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("Action Manager non è un singleton");
		instance = this;
	}
	#endregion
	bool isOpen;
	public Joystick joy;
	public GameObject panel, overlay;
	public GameObject[] actionSpots;
	public List<TimeAction> currentHiddenActions, currentActions;

	public void TogglePanel()
	{
		isOpen = !isOpen;
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(isOpen ? "click" : "clickDepitched");
		panel.SetActive(isOpen);
		overlay.SetActive(isOpen);
		PanZoom.instance.canDo = !isOpen;
		for (int i = 0; i < currentActions.Count; i++)
		{
			var s = actionSpots[i];
			var a = currentActions[i];
			s.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.action.name;
			s.transform.Find("Building").GetComponent<TextMeshProUGUI>().text = GameManager.ChangeToFriendlyString(a.building.objectName + (a.building.objectSubName != "" ? $"({a.building.objectSubName})" : ""));
			s.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = GameManager.IntToMinuteSeconds(a.timeLeft);
			s.SetActive(true);
		}
	}

	public void ReEnableJoy()
	{
		joy.canUseJoystick = true;
	}
	
	public void DisableJoy()
	{
		joy.canUseJoystick = false;
	}



	public void AddAction(TimeAction action)
	{
		if (action.show)
		{
			currentActions.Add(action);
		}
		else
		{
			currentHiddenActions.Add(action);
		}
		action.loadingBar.gameObject.SetActive(true);
		action.loadingBar.slider.maxValue = action.totalTime;
		action.loadingBar.value.text = GameManager.IntToMinuteSeconds(action.totalTime);
	}


	public bool CheckIfNotTooManyActions() //returns false if there are too many actions
	{
		return currentActions.Count <= 5;
	}



	/// <summary>
	/// Returns true if there aren't any other actions done on the same building
	/// </summary>
	public bool CanDoAction(string b)
	{
		foreach (var a in currentActions)
		{
			if (a != null && a.building.objectName == b)
			{
				return false;
			}
		}
		return true;
	}

	private void Start()
	{
		InvokeRepeating(nameof(RefreshTimesLeft), .1f, 1);
		isOpen = false;
		currentActions = new List<TimeAction>();
		currentHiddenActions = new List<TimeAction>();
		StartCoroutine(CoroutineSetStatus());
	}

	IEnumerator CoroutineSetStatus()
	{
		yield return new WaitForSeconds(.1f);
		SetStatus(SaveSystem.instance.LoadData<Status>(SaveSystem.instance.actionManagerFileName, false));
	}

	public Status SendStatus()
	{
		var actions = new TimeAction.Status[currentActions.Count + currentHiddenActions.Count];
		for (int i = 0; i < currentActions.Count; i++)
		{
			actions[i] = currentActions[i].SendStatus();
		}
		for (int i = currentActions.Count; i < actions.Length; i++)
		{
			actions[i] = currentHiddenActions[i].SendStatus();
		}
		return new Status
		{
			actions = actions,
		};
	}
	void SetStatus(Status status)
	{
		if (status != null)
		{
			foreach (var a in status.actions)
			{
				var aa = new TimeAction();
				aa.SetStatus(a);
				AddAction(aa);
			}
		}
	}
	public class Status
	{
		public TimeAction.Status[] actions;
	}

	void RefreshTimesLeft()
	{
		for (int i = 0; i < currentActions.Count; i++)
		{
			var a = currentActions[i];
			var actualTimeLeft = GameManager.IntToMinuteSeconds(a.timeLeft);
			actionSpots[i].transform.Find("Time").GetComponent<TextMeshProUGUI>().text = actualTimeLeft;
			a.loadingBar.value.text = actualTimeLeft;
			a.loadingBar.slider.value = a.totalTime - a.timeLeft;
			if (a.timeLeft <= 0)
			{
				//a.building.PlayOnActionEndState(a.action);
				currentActions.Remove(a);
				actionSpots[i].SetActive(false);
				a.loadingBar.gameObject.SetActive(false);
				a.OnEnd?.Invoke();
				a.action.ChangeCountersOnEnd();
				a.building.StartWaitToUseAgain(a.building.buttons[a.buttonNum - 1]);
			}
			a.timeLeft--;
		}
		for(int i=0;i<currentHiddenActions.Count;i++)
		{
			var a = currentActions[i];
			var actualTimeLeft = GameManager.IntToMinuteSeconds(a.timeLeft);
			a.loadingBar.value.text = actualTimeLeft;
			a.loadingBar.slider.value = a.totalTime - a.timeLeft;
			if (a.timeLeft <= 0)
			{
				currentHiddenActions.Remove(a);
				a.loadingBar.gameObject.SetActive(false);
				a.OnEnd?.Invoke();
				a.action.ChangeCountersOnEnd();
				a.building.StartWaitToUseAgain(a.building.buttons[a.buttonNum - 1]);
			}
			a.timeLeft--;
		}
	}
}

[System.Serializable]
public class TimeAction
{
	public PlayerAction action;
	public InGameObject building;
	public int buttonNum;
	public int totalTime;
	public int timeLeft;
	public bool show;
	public TimeLeftBar loadingBar;
	public Action OnEnd;

	public TimeAction(string actionName, string buildingId, int buttonNum, bool show)
	{
		SetStatus(new Status
		{ 
			action = actionName,
			buildingId = buildingId,
			buttonNum = buttonNum,
			show = show,
		});
		timeLeft = action.EditableTimeNeeded;
	}
	public TimeAction() { }

	[System.Serializable]
	public class Status
	{
		public string action;
		public string buildingId;
		public int buttonNum;
		public int timeLeft;
		public bool show;
	}
	public void SetStatus(Status status)
	{
		action = QuestManager.instance.GetActionByName(status.action);
		building = GameManager.instance.GetObjectById(status.buildingId);
		buttonNum = status.buttonNum;
		timeLeft = status.timeLeft;
		OnEnd = building.GetOnEndAction(buttonNum - 1);
		totalTime = action.EditableTimeNeeded;
		loadingBar = building.loadingBar;
		show = status.show;
	}
	public Status SendStatus()
	{
		return new Status
		{
			action = action.name,
			buildingId = building.id,
			buttonNum = buttonNum,
			timeLeft = timeLeft,
			show = show,
		};
	}
}