using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class AIsManager : MonoBehaviour
{
	const int percentageOfActiveAIs = 50;
	public CapieCambu[] allCapiECambu;
	[System.NonSerialized]
	public Squadrigliere[] allSquadriglieri;
	public GameObject AIContainer;

	public AIEvent[] events;
	public GameObject eventButton, eventPanel, overlay;
	bool isOpen;
	public Transform[] squadriglieAI;

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
		InvokeRepeating(nameof(CallSetActiveOrInactiveAI), 2f, 30f);
		InvokeRepeating(nameof(RefreshEventTimeLeft), 1f, 1f);
		InvokeRepeating(nameof(RefreshPanelUI), 1f, 1f);
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
		AIEvent.Status[] ai = new AIEvent.Status[events.Length];
		for (int i = 0; i < events.Length; i++)
		{
			ai[i] = events[i].SendStatus();
		}

		return new Status
		{
			nextDialogueIndices = indices,
			capiECambuInfo = c,
			squadriglieriInfo = s,
			aiEventsInfo = ai,
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
			for (int i = 0; i < events.Length; i++)
			{
				events[i].SetStatus(status.aiEventsInfo[i]);
			}
			eventButton.SetActive(AreThereAnyRunningEvents != null);
		}
	}
	public class Status
	{
		public InGameObject.Status[] capiECambuInfo;
		public int[] nextDialogueIndices;
		public InGameObject.Status[] squadriglieriInfo;
		public AIEvent.Status[] aiEventsInfo;
	}
	#endregion
	
	void CallSetActiveOrInactiveAI()
	{
		SetActiveOrInactiveAI(100 - percentageOfActiveAIs);
	}
	void SetActiveOrInactiveAI(int perc)//despawn IA
	{
		if (AreThereAnyRunningEvents == null)
		{
			foreach (var sq in allSquadriglieri)
			{
				Collider2D[] collidersIA = Physics2D.OverlapCircleAll(transform.position,5f);
				bool nearPlayer = false;

                foreach (Collider2D c in collidersIA)
                {
                    if (c.name == "Player")
                    {
						nearPlayer = true;
						break;
                    }
                }

				if (!nearPlayer)
				{
					sq.UnlockNow();

					if (sq.sq != Player.instance.squadriglia && GameManager.DoIfPercentage(perc))
					{
						StartCoroutine(sq.ForceTarget("Tenda", true, true));
					}
				}				
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
				SetActiveOrInactiveAI(100);
				GameManager.instance.WarningOrMessage($"L'evento {e.name.ToLower()} comincia tra...", false);
			}
		}
	}


	public void SendToSleep(bool isDay)
    {
		Debug.Log("reached AIs manager");
        if (!isDay)
        {
			foreach (Transform a in squadriglieAI)
			{
				foreach (Transform b in a)
				{
					StartCoroutine(b.GetComponent<BaseAI>().ForceTarget(a.position, true, true, false));
				}
			}
        }
        else
        {
			foreach (Transform a in squadriglieAI)
			{
				foreach (Transform b in a)
				{
					b.GetComponent<BaseAI>().UnlockNow();
				}
			}
		}       
	}


	public AIEvent AreThereAnyRunningEvents
	{
		get
		{
			foreach (var e in events)
			{
				if (e.running)
					return e;
			}
			return null;
		}
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
					e.timeLeft = e.duration;
					eventButton.gameObject.SetActive(true);
					e.running = true;
					foreach (var a in e.mainAIs)
					{
						a.gameObject.SetActive(true);
						a.ToggleClickListener(true);
						//a.GetComponent<Animator>().SetBool("move", true);
						a.ForceToggleName(true);
					}
				}
			}
			else if (e.running)
			{
				e.timeLeft--;
				eventButton.gameObject.SetActive(true);
				if (e.timeLeft <= 0)
				{
					e.running = false;
					GameManager.instance.WarningOrMessage($"L'evento {e.name.ToLower()} è terminato!", false);
					eventButton.gameObject.SetActive(false);
					foreach (var a in e.mainAIs)
					{
						a.ForceToggleNameInstant(false);
						//Debug.LogWarning("should have set inactive");
						a.ToggleClickListener(false);
						a.GetComponent<Animator>().SetBool("move", false);
						a.gameObject.SetActive(false);
					}
					ToggleEventPanel(false);
				}
			}
		}
	}
	public void ToggleEventPanel(bool open)
	{
		isOpen = open;
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(isOpen ? "click" : "clickDepitched");
		eventPanel.SetActive(isOpen);
		Joystick.instance.canUseJoystick = !isOpen;
		overlay.SetActive(isOpen);
		PanZoom.instance.canDo = !isOpen;
		RefreshPanelUI();
	}

	public void ToggleEventPanel()
	{
		isOpen = !isOpen;
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(isOpen ? "click" : "clickDepitched");
		eventPanel.SetActive(isOpen);
		Joystick.instance.canUseJoystick = !isOpen;
		overlay.SetActive(isOpen);
		PanZoom.instance.canDo = !isOpen;
		RefreshPanelUI();
	}

	public void RefreshPanelUI()
	{
		var e = AreThereAnyRunningEvents;
		if (e != null) {
			eventPanel.transform.Find("Description1").GetComponent<TextMeshProUGUI>().text = $"Nome: {e.name}";
			eventPanel.transform.Find("Description2").GetComponent<TextMeshProUGUI>().text = $"Descrizione: {e.description}";
			eventPanel.transform.Find("Description3").GetComponent<TextMeshProUGUI>().text = $"Inizio: Giorno {e.day}, ore {e.hour}:00";
			eventPanel.transform.Find("Description4").GetComponent<TextMeshProUGUI>().text = $"Durata: {GameManager.IntToMinuteSeconds(e.duration)}";
			eventPanel.transform.Find("Description5").GetComponent<TextMeshProUGUI>().text = $"Tempo rimasto: {GameManager.IntToMinuteSeconds(e.timeLeft)}";
		}
	}
}