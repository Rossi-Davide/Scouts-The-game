using System.Collections;
using System.Threading;
using UnityEngine;

public class AIsManager : MonoBehaviour
{
	int percentageOfActiveAIs = 15;
	public CapieCambu[] allCapiECambu;
	[HideInInspector] [System.NonSerialized]
	public Squadrigliere[] allSquadriglieri;
	public GameObject AIContainer;

	public AIEvent[] events;
	public GameObject eventButton;

	#region Singleton
	public static AIsManager instance;
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("AIsManager non è un singleton!");
		instance = this;
	}
	#endregion

	private void Start()
	{
		InvokeRepeating(nameof(SetActiveOrInactiveAI), .5f, Random.Range(6, 13));
		InvokeRepeating(nameof(RefreshEventTimeLeft), 1f, 1f);
		GameManager.instance.OnHourChange += CheckAIEvents;
		SaveSystem.instance.OnReadyToLoad += ReceiveSavedData;
	}

	void ReceiveSavedData(LoadPriority p)
	{
		if (p == LoadPriority.Normal)
		{
			for (int i = 0; i < allCapiECambu.Length; i++)
			{
				allCapiECambu[i].nextDialogueIndex = (int)SaveSystem.instance.RequestData(DataCategory.AIsManager, DataKey.allCapiECambu, DataParameter.nextDialogueIndex, i);
			}
			for (int i = 0; i < events.Length; i++)
			{
				events[i].timeLeft = (int)SaveSystem.instance.RequestData(DataCategory.AIsManager, DataKey.events, DataParameter.timeLeft, i);
				events[i].running = (bool)SaveSystem.instance.RequestData(DataCategory.AIsManager, DataKey.events, DataParameter.running, i);
			}
		}
	}

	void SetActiveOrInactiveAI()
	{
		foreach (var sq in allSquadriglieri)
		{
			sq.gameObject.SetActive(true);
			sq.ToggleUI(true);
			if (sq.sq != Player.instance.squadriglia && GameManager.DoIfPercentage(100 - percentageOfActiveAIs))
			{
				sq.ForceTarget("Tenda", Random.Range(6, 13), true);
				sq.ToggleUI(false);
			}
		}
	}

	void CheckAIEvents(int hour)
	{
		foreach (var e in events)
		{
			if (GameManager.instance.currentDay == e.day && hour == e.hour)
			{
				e.countDownLeft = e.countDownLenght;
				GameManager.instance.WarningOrMessage($"L'evento {e.name} comincia tra...", false);
			}
		}
	}

	void RefreshEventTimeLeft()
	{
		foreach (var e in events)
		{
			if (e.countDownLeft > 0)
			{
				e.countDownLeft--;
				GameManager.instance.WarningOrMessage(e.countDownLeft.ToString(), false);
				if (e.countDownLeft <= 0)
				{
					e.running = true;
					e.timeLeft = e.duration;
				}
			}
			else if (e.running)
			{
				e.timeLeft--;
				if (e.timeLeft <= 0)
				{
					e.running = false;
				}
			}
		}
	}
}