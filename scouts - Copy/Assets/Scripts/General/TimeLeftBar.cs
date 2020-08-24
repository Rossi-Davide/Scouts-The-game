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

	private void OnEnable()
	{
		slider = GetComponent<Slider>();
		value = transform.Find("TimeLeft").GetComponent<TextMeshProUGUI>();
		slider.maxValue = totalTime;
		slider.value = 0;
		timeLeft = totalTime;
		InvokeRepeating("Counter", 0, 1);
	}
	private void OnDisable()
	{
		CancelInvoke("Counter");
	}
	void Counter()
	{
		if (timeLeft == 0)
		{
			gameObject.SetActive(false);
		}
		int sec = (timeLeft % 60);
		int min = (timeLeft - sec) / 60;
		string timeString = timeLeft >= 60 ? min + "m " + sec + "s" : sec + "s";
		value.text = timeString;
		slider.value = totalTime - timeLeft;
		timeLeft--;
		
	}
}
