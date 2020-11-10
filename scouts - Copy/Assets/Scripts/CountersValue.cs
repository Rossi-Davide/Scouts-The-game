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
		energyCounter.maxValue = GameManager.instance.energyMaxValue;
		materialsCounter.maxValue = GameManager.instance.materialsMaxValue;
		pointsCounter.maxValue = GameManager.instance.pointsMaxValue;
	}

	void GetCounterValue(Counter counter, int delta)
	{
		switch (counter)
		{
			case Counter.Energia:
				energyCounter.value += delta;
				energyValue.GetComponent<TextMeshProUGUI>().text = energyCounter.value.ToString();
				break;
			case Counter.Materiali:
				materialsCounter.value += delta;
				materialsValue.GetComponent<TextMeshProUGUI>().text = materialsCounter.value.ToString();
				break;
			case Counter.Punti:
				pointsCounter.value += delta;
				pointsValue.GetComponent<TextMeshProUGUI>().text = pointsCounter.value.ToString();
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
				energyCounter.maxValue = GameManager.instance.energyMaxValue;
				break;
			case Counter.Materiali:
				materialsCounter.maxValue = GameManager.instance.materialsMaxValue;
				break;
			case Counter.Punti:
				pointsCounter.maxValue = GameManager.instance.pointsMaxValue;
				break;
			default:
				throw new System.Exception("Il counter ricercato non esiste");
		}
	}
}
