using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
	bool isOpen;
	public GameObject panel, overlay, nomeSq, descrizione, materiali, punti;
	public void ToggleStatisticsPanel()
	{
		isOpen = !isOpen;
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(isOpen ? "click" : "clickDepitched");
		panel.SetActive(isOpen);
		overlay.SetActive(isOpen);
		FindObjectOfType<StatisticsTabs>().OnClick(1);
		FindObjectOfType<StatisticsTabs>().RefreshSqInfo();
		PanZoom.instance.canDo = !isOpen;
		Joystick.instance.enabled = !isOpen;
	}
}
