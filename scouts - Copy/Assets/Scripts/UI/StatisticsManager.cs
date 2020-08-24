using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
	bool isOpen;
	public GameObject panel, overlay, nomeSq, descrizione, materiali, punti;
	public void ToggleStatisticsPanel()
	{
		if (!isOpen)
		{
			panel.SetActive(true);
			overlay.SetActive(true);
			Time.timeScale = 0;
			isOpen = true;
			FindObjectOfType<StatisticsTabs>().OnClick(1);
			FindObjectOfType<StatisticsTabs>().RefreshSqInfo();
		}
		else
		{
			Time.timeScale = 1;
			isOpen = false;
			panel.SetActive(false);
			overlay.SetActive(false);
			
		}
	}
}
