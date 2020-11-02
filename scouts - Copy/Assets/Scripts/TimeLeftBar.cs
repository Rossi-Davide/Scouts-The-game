using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeLeftBar : MonoBehaviour
{
	[HideInInspector]
	public Slider slider;
	[HideInInspector]
	public TextMeshProUGUI value;

	void Awake()
	{
		slider = GetComponent<Slider>();
		value = transform.Find("TimeLeft").GetComponent<TextMeshProUGUI>();
	}
}
