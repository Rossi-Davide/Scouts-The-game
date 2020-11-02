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
		GetCounterValue(Counter.Energia, GameManager.instance.energyValue);
		GetCounterValue(Counter.Materiali, GameManager.instance.materialsValue);
		GetCounterValue(Counter.Punti, GameManager.instance.pointsValue);
	}

	void GetCounterValue(Counter counter, int newValue)
	{
		switch (counter)
		{
			case Counter.Energia:
				energyCounter.value = newValue;
				energyValue.GetComponent<TextMeshProUGUI>().text = newValue.ToString();
				break;
			case Counter.Materiali:
				materialsCounter.value = newValue;
				materialsValue.GetComponent<TextMeshProUGUI>().text = newValue.ToString();
				break;
			case Counter.Punti:
				pointsCounter.value = newValue;
				pointsValue.GetComponent<TextMeshProUGUI>().text = newValue.ToString();
				break;
			case Counter.None:
				break;
			default:
				throw new System.Exception("Il counter ricercato non esiste");
		}
	}

	void GetCounterMaxValue(Counter counter, int delta)
	{
		switch (counter)
		{
			case Counter.Energia:
				energyCounter.maxValue += delta;
				break;
			case Counter.Materiali:
				materialsCounter.maxValue += delta;
				break;
			case Counter.Punti:
				pointsCounter.maxValue += delta;
				break;
			default:
				throw new System.Exception("Il counter ricercato non esiste");
		}
	}
}
