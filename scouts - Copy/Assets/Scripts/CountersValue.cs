using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountersValue : MonoBehaviour
{
	public Slider energyCounter, materialsCounter, pointsCounter;
	public GameObject energyValue, materialsValue, pointsValue;
	private void Start()
	{
		GameManager.instance.OnCounterValueChange += GetCounterValue;
		GameManager.instance.OnCounterMaxValueChange += GetCounterMaxValue;
	}

	void GetCounterValue(Counter counter, int newValue)
	{
		switch (counter)
		{
			case Counter.Energia:
				energyCounter.value = newValue;
				energyValue.GetComponent<TextMeshProUGUI>().text = energyCounter.value.ToString();
				break;
			case Counter.Materiali:
				materialsCounter.value = newValue;
				materialsValue.GetComponent<TextMeshProUGUI>().text = materialsCounter.value.ToString();
				break;
			case Counter.Punti:
				pointsCounter.value = newValue;
				pointsValue.GetComponent<TextMeshProUGUI>().text = pointsCounter.value.ToString();
				break;
			case Counter.None:
				break;
			default:
				throw new System.Exception("Il counter ricercato non esiste");
		}
	}

	void GetCounterMaxValue(Counter counter, int newValue)
	{
		switch (counter)
		{
			case Counter.Energia:
				energyCounter.maxValue = newValue;
				break;
			case Counter.Materiali:
				materialsCounter.maxValue = newValue;
				break;
			case Counter.Punti:
				pointsCounter.maxValue = newValue;
				break;
			default:
				throw new System.Exception("Il counter ricercato non esiste");
		}
	}
}
