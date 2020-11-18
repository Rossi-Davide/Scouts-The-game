using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeLeftBar : MonoBehaviour
{
	[HideInInspector] [System.NonSerialized]
	public Slider slider;
	[HideInInspector] [System.NonSerialized]
	public TextMeshProUGUI value;

	void Awake()
	{
		slider = GetComponent<Slider>();
		value = transform.Find("TimeLeft").GetComponent<TextMeshProUGUI>();
	}
}
