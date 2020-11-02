using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
	bool isOpen;
	public GameObject panel, overlay, nomeSq, descrizione, materiali, punti;
	public void ToggleStatisticsPanel()
	{

		if (!isOpen)
		{
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

			panel.SetActive(true);
			overlay.SetActive(true);
			isOpen = true;
			FindObjectOfType<StatisticsTabs>().OnClick(1);
			FindObjectOfType<StatisticsTabs>().RefreshSqInfo();
		}
		else
		{
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");
			isOpen = false;
			panel.SetActive(false);
			overlay.SetActive(false);
			
		}
	}
}
