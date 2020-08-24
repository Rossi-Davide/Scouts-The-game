using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountersValue : MonoBehaviour
{
	public Slider energyCounter, materialsCounter, pointsCounter;
	public GameObject energyValue, materialsValue, pointsValue;
	private void Start()
	{
		GameManager.instance.OnEnergyChange += GetEnergyValue;
		GameManager.instance.OnMaterialsChange += GetMaterialsValue;
		GameManager.instance.OnPointsChange += GetPointsValue;
		GetEnergyValue(GameManager.instance.energyValue);
		GetMaterialsValue(GameManager.instance.materialsValue);
		GetPointsValue(GameManager.instance.pointsValue);
	}
	private void OnDestroy()
	{
		if (GameManager.instance != null)
		{
			GameManager.instance.OnEnergyChange -= GetEnergyValue;
			GameManager.instance.OnMaterialsChange -= GetMaterialsValue;
			GameManager.instance.OnPointsChange -= GetPointsValue;
		}
	}

	private void GetEnergyValue(int newValue)
	{
		energyCounter.value = newValue;
		energyValue.GetComponent<TextMeshProUGUI>().text = newValue.ToString();
	}
	private void GetMaterialsValue(int newValue)
	{
		materialsCounter.value = newValue;
		materialsValue.GetComponent<TextMeshProUGUI>().text = newValue.ToString();
	}
	private void GetPointsValue(int newValue)
	{
		pointsCounter.value = newValue;
		pointsValue.GetComponent<TextMeshProUGUI>().text = newValue.ToString();
	}
}
