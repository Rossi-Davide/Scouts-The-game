using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using System.Linq;

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
		Joystick.instance.enabled = false;
		for (int i = 0; i < currentActions.Count; i++)
		{
			var s = actionSpots[i];
			var a = currentActions[i];
			s.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.action.name;
			s.transform.Find("Building").GetComponent<TextMeshProUGUI>().text = GameManager.ChangeToFriendlyString(a.building.ToString());
			s.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = GameManager.IntToMinuteSeconds(a.timeLeft);
			s.SetActive(true);
		}
	}





	public void AddAction(TimeAction action, bool show)
	{
		if (show)
		{
			currentActions.Add(action);
		}
		else
		{
			currentHiddenActions.Add(action);
		}
		action.timeLeft = action.totalTime;
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
		InvokeRepeating(nameof(RefreshTimesLeft), 0, 1);
		isOpen = false;
		currentActions = new List<TimeAction>();
		currentHiddenActions = new List<TimeAction>();
		SetStatus(SaveSystem.instance.LoadData<Status>(SaveSystem.instance.actionManagerFileName));
	}

	public Status SendStatus()
	{
		return new Status
		{
			currentActions = currentActions.ToArray(),
			currentHiddenActions = currentHiddenActions.ToArray(),
		};
	}
	void SetStatus(Status status)
	{
		if (status != null)
		{
			if (status.currentActions != null)
				Array.ForEach(status.currentActions, element => currentActions.Add(element));
			if (status.currentHiddenActions != null)
				Array.ForEach(status.currentHiddenActions, element => currentHiddenActions.Add(element));
		}
	}
	public class Status
	{
		public TimeAction[] currentActions;
		public TimeAction[] currentHiddenActions;
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
				currentActions.Remove(a);
				actionSpots[i].SetActive(false);
				a.loadingBar.gameObject.SetActive(false);
				a.OnEnd?.Invoke();
				a.action.ChangeCountersOnEnd();
				a.building.StartWaitToUseAgain(a.building.buttons[a.buttonNum - 1]);
			}
			a.timeLeft--;
		}
		foreach (var a in currentHiddenActions)
		{
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
	public TimeLeftBar loadingBar;
	public Action OnEnd;

	public TimeAction(PlayerAction action, InGameObject building, int buttonNum, TimeLeftBar bar, Action OnEnd)
	{
		this.action = action;
		this.building = building;
		this.buttonNum = buttonNum;
		totalTime = this.action.timeNeeded;
		loadingBar = bar;
		this.OnEnd = OnEnd;
	}
}