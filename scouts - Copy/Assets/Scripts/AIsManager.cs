using System.Collections;
using System.Threading;
using UnityEngine;

public class AIsManager : MonoBehaviour
{
	public CapieCambu[] allCapiECambu;
	[HideInInspector]
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
		InvokeRepeating(nameof(SetActiveOrInactiveAI), 10, Random.Range(10, 30));
		InvokeRepeating(nameof(RefreshEventTimeLeft), 1f, 1f);
		GameManager.instance.OnHourChange += CheckAIEvents;
	}

	void SetActiveOrInactiveAI()
	{
		foreach (var sq in allSquadriglieri)
		{
			if (sq.sq != Player.instance.squadriglia && GetProbability(40))
			{
				sq.ForceTarget("Tenda", Random.Range(13, 45), true); //set inactive
			}
		}
	}

	bool GetProbability(int percentage)
	{
		return Random.Range(1, 101) <= percentage;
	}

	void CheckAIEvents(int hour)
	{
		foreach (var e in events)
		{
			if (GameManager.instance.currentDay == e.day && hour == e.hour)
			{
				StartCoroutine(EventCountDown(e));
			}
		}
	}

	IEnumerator EventCountDown(AIEvent e)
	{
		var g = GameManager.instance;
		g.WarningOrMessage($"L'evento {e.name} comincia tra...", false);
		yield return new WaitForSeconds(1f);
		for (int i = e.countDownLenght; i > 0; i--)
		{
			g.WarningOrMessage(i.ToString(), false);
			yield return new WaitForSeconds(1f);
		}
		e.running = true;
		e.timeLeft = e.duration;
	}

	void RefreshEventTimeLeft()
	{
		foreach (var e in events)
		{
			if (e.running)
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