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

	private void OnEnable()
	{
		slider = GetComponent<Slider>();
		value = transform.Find("TimeLeft").GetComponent<TextMeshProUGUI>();
		slider.maxValue = totalTime;
		slider.value = 0;
		timeLeft = totalTime;
		InvokeRepeating("RefreshTime", 0, 1);
	}
	private void OnDisable()
	{
		CancelInvoke("RefreshTime");
	}
	void RefreshTime()
	{
		if (timeLeft == 0)
		{
			gameObject.SetActive(false);
		}
		value.text = ActionManager.instance.GetTimeLeft(action);
	}
}
