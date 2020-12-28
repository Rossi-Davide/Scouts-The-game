using System.Collections;
using System.Threading;
using UnityEngine;

public class AIsManager : MonoBehaviour
{
	const int percentageOfActiveAIs = 50;
	public CapieCambu[] allCapiECambu;
	[System.NonSerialized]
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
		InvokeRepeating(nameof(SetActiveOrInactiveAI), 2f, 30f);
		InvokeRepeating(nameof(RefreshEventTimeLeft), 1f, 1f);
		GameManager.instance.OnHourChange += CheckAIEvents;
	}
	#region Status

	public Status SendStatus()
	{
		int[] indices = new int[allCapiECambu.Length];
		InGameObject.Status[] c = new InGameObject.Status[allCapiECambu.Length];
		InGameObject.Status[] s = new InGameObject.Status[allSquadriglieri.Length];
		for (int i = 0; i < allCapiECambu.Length; i++)
		{
			indices[i] = allCapiECambu[i].nextDialogueIndex;
			c[i] = allCapiECambu[i].SendStatus();
		}
		for (int i = 0; i < allSquadriglieri.Length; i++)
		{
			s[i] = allSquadriglieri[i].SendStatus();
		}

		return new Status
		{
			nextDialogueIndices = indices,
			capiECambuInfo = c,
			squadriglieriInfo = s,
		};
	}
	public void SetStatus(Status status) //called by Squadriglia manager
	{
		if (status != null)
		{
			for (int s = 0; s < allSquadriglieri.Length; s++)
			{
				allSquadriglieri[s].SetStatus(status.squadriglieriInfo[s]);
			}
			for (int s = 0; s < allCapiECambu.Length; s++)
			{
				allCapiECambu[s].SetStatus(status.capiECambuInfo[s]);
				allCapiECambu[s].nextDialogueIndex = status.nextDialogueIndices[s];
			}
		}
	}
	public class Status
	{
		public InGameObject.Status[] capiECambuInfo;
		public int[] nextDialogueIndices;
		public InGameObject.Status[] squadriglieriInfo;
	}
	#endregion
	void SetActiveOrInactiveAI()
	{
		foreach (var sq in allSquadriglieri)
		{
			StartCoroutine(sq.Unlock());
			if (sq.sq != Player.instance.squadriglia && GameManager.DoIfPercentage(100 - percentageOfActiveAIs))
			{
				StartCoroutine(sq.ForceTarget("Tenda", true, true));
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
				e.running = true;
				GameManager.instance.WarningOrMessage($"L'evento {e.name.ToLower()} comincia tra...", false);
			}
		}
	}

	public bool AreThereAnyRunningEvents()
	{
		foreach (var e in events)
		{
			if (e.running)
				return true;
		}
		return false;
	}

	void RefreshEventTimeLeft()
	{
		foreach (var e in events)
		{
			if (e.countDownLeft > 0)
			{
				e.countDownLeft--;
				if (e.countDownLeft >= 1)
					GameManager.instance.WarningOrMessage($"L'evento {e.name.ToLower()} comincia tra... {e.countDownLeft}", false);
				else
				{
					GameManager.instance.WarningOrMessage($"L'evento è cominciato!", false);
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
					GameManager.instance.WarningOrMessage($"L'evento {e.name.ToLower()} è terminato!", false);
				}
			}
		}
	}
}