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
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

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

	public void Menu()
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

		Time.timeScale = 1;
		mainMenu.instance.GoToMenu();
	}

	public void ResumeClicked()              //ho applicato delle diversificazioni per i suoni, null'altro è stato cambiato
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

	}
	public void ClosedClicked()
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");

	}
}
