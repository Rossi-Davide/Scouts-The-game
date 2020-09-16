using UnityEngine;
using TMPro;

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
				s.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.name;
				s.transform.Find("Building").GetComponent<TextMeshProUGUI>().text = GameManager.ChangeToFriendlyString(a.building.ToString());
				s.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = GameManager.IntToMinuteSeconds(a.timeLeft);
			}
			s.SetActive(a != null);
		}
	}





	public void AddAction(TimeAction action)
	{
		for (int i = 0; i < currentActions.Length; i++)
		{
			if (currentActions[i] == null)
			{
				currentActions[i] = action;
				currentActions[i].timeLeft = currentActions[i].totalTime;
				return;
			}
		}
	}


	public bool CheckIfTooManyActions()
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
			if (a != null && a.building == b)
			{
				return false;
			}
		}
		return true;
	}

	private void Start()
	{
		InvokeRepeating("RefreshTimesLeft", 0, 1);
		isOpen = false;
	}
	void RefreshTimesLeft()
	{
		for (int i = 0; i < currentActions.Length; i++)
		{
			var a = currentActions[i];
			if (a != null)
			{
				a.timeLeft--;
				actionSpots[i].transform.Find("Time").GetComponent<TextMeshProUGUI>().text = GameManager.IntToMinuteSeconds(a.timeLeft);
				if (a.timeLeft <= 0)
				{
					currentActions[i] = null;
					actionSpots[i].SetActive(false);
				}
			}
		}
	}

	public int GetTimeLeft(TimeAction action)
	{
		foreach (var a in currentActions)
		{
			if (a == action)
			{
				return a.timeLeft;
			}
		}
		return 0;
	}

}

public class TimeAction
{
	public string name;
	public string building;
	public int totalTime;
	public int timeLeft;

	public TimeAction(string name, string building, int time)
	{
		this.name = name;
		this.building = building;
		this.totalTime = time;
	}
}