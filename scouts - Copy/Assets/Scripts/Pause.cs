using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
	public Joystick joy;
	bool isOpen;
	public GameObject panel, overlay;
	public void ReEnableJoy()
	{
		joy.canUseJoystick = true;
	}

	public void DisableJoy()
	{
		joy.canUseJoystick = false;
	}
	public void TogglePausePanel()
	{
		isOpen = !isOpen;
		AudioManager.instance.Play(isOpen ? "click" : "clickDepitched");
		overlay.SetActive(isOpen);
		panel.SetActive(isOpen);
		PanZoom.instance.canDo = !isOpen;
		Time.timeScale = isOpen ? 0 : 1;
	}


	public void Menu()
	{
		Time.timeScale = 1;
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");
		SaveSystem.instance.GetSaveAll();
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
