using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
	bool isOpen;
	public GameObject panel, overlay, nomeSq, descrizione, materiali, punti;
	public Joystick joy;
	public void ToggleStatisticsPanel()
	{
		joy.canUseJoystick = isOpen;
		isOpen = !isOpen;
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(isOpen ? "click" : "clickDepitched");
		panel.SetActive(isOpen);
		overlay.SetActive(isOpen);
		if (isOpen)
		{
			FindObjectOfType<StatisticsTabs>().OnClick(1);
			FindObjectOfType<StatisticsTabs>().RefreshSqInfo();
		}		
		PanZoom.instance.canDo = !isOpen;
	}
	public void ReEnableJoy()
	{
		joy.canUseJoystick = true;
	}

	public void DisableJoy()
	{
		joy.canUseJoystick = false;
	}
}
