using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
	bool isOpen;
	public GameObject panel, overlay;
	public void TogglePausePanel()
	{
		isOpen = !isOpen;
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(isOpen ? "click" : "");

		overlay.SetActive(isOpen);
		panel.SetActive(isOpen);
		Time.timeScale = isOpen ? 0 : 1;
		PanZoom.instance.canDo = !isOpen;
		Joystick.instance.enabled = !isOpen;
	}

	public void Menu()
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

		Time.timeScale = 1;
		SceneLoader.instance.LoadMainMenuScene();
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
