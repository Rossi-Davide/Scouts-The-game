using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeLeftBar : MonoBehaviour
{
	public int totalTime;
	[HideInInspector]
	public int timeLeft;
	private Slider slider;
	private TextMeshProUGUI value;
	[HideInInspector]
	public CurrentAction action;
	[HideInInspector]
	public System.Action OnEnd;

	void OnEnable()
	{
		slider = GetComponent<Slider>();
		value = transform.Find("TimeLeft").GetComponent<TextMeshProUGUI>();
		slider.maxValue = totalTime;
		slider.value = 0;
		timeLeft = totalTime;
		InvokeRepeating("RefreshTime", 0, .3f);
	}
	void OnDisable()
	{
		CancelInvoke("RefreshTime");
	}
	void RefreshTime()
	{
		if (timeLeft == 0)
		{
			gameObject.SetActive(false);
			OnEnd();
		}
		timeLeft = ActionManager.instance.GetTimeLeft(action);
		value.text = GameManager.IntToMinuteSeconds(timeLeft);
		slider.value = totalTime - timeLeft;
	}

	public void InitializeValues(CurrentAction action, System.Action OnEnd)
	{
		this.action = action;
		this.OnEnd = OnEnd;
		this.totalTime = action.totalTime;
		gameObject.SetActive(true);
	}
}
