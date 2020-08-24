using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
	bool isOpen;
	public GameObject panel, overlay;
	public void TogglePausePanel()
	{
		if (!isOpen)
		{
			overlay.SetActive(true);
			panel.SetActive(true);
			Time.timeScale = 0;
			isOpen = true;
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
