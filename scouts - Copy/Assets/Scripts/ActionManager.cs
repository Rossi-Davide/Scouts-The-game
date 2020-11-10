using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

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
	public TimeAction[] currentActions = new TimeAction[5];
	public List<TimeAction> currentHiddenActions = new List<TimeAction>();

	public void TogglePanel()
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

		isOpen = !isOpen;
		panel.SetActive(isOpen);
		overlay.SetActive(isOpen);
		PanZoom.instance.enabled = !isOpen;
		for (int i = 0; i < currentActions.Length; i++)
		{
			var s = actionSpots[i];
			var a = currentActions[i];
			if (a != null)
			{
				s.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.action.name;
				s.transform.Find("Building").GetComponent<TextMeshProUGUI>().text = GameManager.ChangeToFriendlyString(a.building.ToString());
				s.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = GameManager.IntToMinuteSeconds(a.timeLeft);
			}
			s.SetActive(a != null);
		}
	}





	public void AddAction(TimeAction action, bool show)
	{
		if (show)
		{
			for (int i = 0; i < currentActions.Length; i++)
			{
				if (currentActions[i] == null)
				{
					currentActions[i] = action;
					currentActions[i].timeLeft = currentActions[i].totalTime;
					currentActions[i].loadingBar.gameObject.SetActive(true);
					currentActions[i].loadingBar.slider.maxValue = currentActions[i].totalTime;
					currentActions[i].loadingBar.value.text = GameManager.IntToMinuteSeconds(currentActions[i].totalTime);
					return;
				}
			}
		}
		else
		{
			currentHiddenActions.Add(action);
			action.timeLeft = action.totalTime;
			action.loadingBar.gameObject.SetActive(true);
			action.loadingBar.slider.maxValue = action.totalTime;
			action.loadingBar.value.text = GameManager.IntToMinuteSeconds(action.totalTime);
		}
	}


	public bool CheckIfNotTooManyActions() //returns false if there are too many actions
	{
		for (int i = 0; i < currentActions.Length; i++)
		{
			if (currentActions[i] == null)
			{
				return true;
			}
		}
		return false;
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
		SaveSystem.instance.OnReadyToLoad += ReceiveSavedData;
	}

	void ReceiveSavedData(LoadPriority p)
	{
		if (p == LoadPriority.Normal)
		{
			currentActions = (TimeAction[])SaveSystem.instance.RequestData(DataCategory.ActionManager, DataKey.currentActions);
			currentHiddenActions = (List<TimeAction>)SaveSystem.instance.RequestData(DataCategory.ActionManager, DataKey.currentHiddenActions);
		}
	}

	void RefreshTimesLeft()
	{
		for (int i = 0; i < currentActions.Length; i++)
		{
			var a = currentActions[i];
			if (a != null)
			{
				var actualTimeLeft = GameManager.IntToMinuteSeconds(a.timeLeft);
				actionSpots[i].transform.Find("Time").GetComponent<TextMeshProUGUI>().text = actualTimeLeft;
				a.loadingBar.value.text = actualTimeLeft;
				a.loadingBar.slider.value = a.totalTime - a.timeLeft;
				if (a.timeLeft <= 0)
				{
					currentActions[i] = null;
					actionSpots[i].SetActive(false);
					a.loadingBar.gameObject.SetActive(false);
					a.OnEnd?.Invoke();
					a.action.ChangeCountersOnEnd();
					a.building.StartWaitToUseAgain(a.building.buttons[a.buttonNum - 1]);
				}
				a.timeLeft--;
			}
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