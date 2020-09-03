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
	public GameObject panel;
	public GameObject[] actionSpots;
	public CurrentAction[] currentActions = new CurrentAction[5];
	public void TogglePanel()
    {
		isOpen = !isOpen;
		panel.SetActive(isOpen);
		for (int i = 0; i < currentActions.Length; i++)
		{
			var s = actionSpots[i];
			var a = currentActions[i];
			s.SetActive(a != null);
			s.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.name;
			s.transform.Find("Building").GetComponent<TextMeshProUGUI>().text = a.building.ToString();
			s.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = GameManager.IntToMinuteSeconds(a.timeLeft);
		}
	}


	public bool AddAction(CurrentAction action)
	{
		for (int i = 0; i < currentActions.Length; i++)
		{
			if (currentActions[i] == null)
			{
				currentActions[i] = action;
				currentActions[i].timeLeft = currentActions[i].totalTime;
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Returns true if there aren't any other actions done on the same building
	/// </summary>
	public bool CanDoAction(Build.Objects b)
	{
		foreach (var a in currentActions)
		{
			if (a.building == b)
			{
				return false;
			}
		}
		return true;
	}

	private void Start()
	{
		InvokeRepeating("RefreshTimesLeft", 0, 1);
	}
	void RefreshTimesLeft()
	{
		for (int i = 0; i < currentActions.Length; i++)
		{
			var a = currentActions[i];
			if (a != null)
			{
				a.timeLeft--;
				if (a.timeLeft <= 0)
				{
					a = null;
				}
			}
		}
	}

	public string GetTimeLeft(CurrentAction action)
	{
		foreach (var a in currentActions)
		{
			if (a == action)
			{
				return GameManager.IntToMinuteSeconds(a.timeLeft);
			}
		}
		throw new System.Exception("Azione scorretta: non esiste");
	}

}

public class CurrentAction
{
	public string name;
	public Build.Objects building;
	public int totalTime;
	public int timeLeft;

	public CurrentAction(string name, Build.Objects building, int time)
	{
		this.name = name;
		this.building = building;
		this.totalTime = time;
	}
}