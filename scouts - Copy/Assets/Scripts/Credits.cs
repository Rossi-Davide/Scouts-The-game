using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public Button homeButton;

	private void Start()
	{
		homeButton.onClick.AddListener(SceneLoader.instance.LoadMainMenuScene);
	}

	public void InstagramDavide()
	{
		System.Diagnostics.Process.Start("https://youtube.com/");
	}
}
