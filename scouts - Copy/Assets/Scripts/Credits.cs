using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public Button homeButton;

	private void Start()
	{
		//homeButton.onClick.AddListener(SceneLoader.instance.LoadMainMenuScene);
	}

	public void InstagramDavide()
	{
		Application.OpenURL("https://www.instagram.com/big_baddave/");
	}
	public void InstagramSara()
	{
		Application.OpenURL("https://www.instagram.com/sara_cappe22/");
	}
	public void InstagramSimo()
	{
		Application.OpenURL("https://www.instagram.com/simone_ceccarelli__/");
	}
	public void InstagramPietro()
	{
		Application.OpenURL("https://www.instagram.com/pietropappo/");
	}
	public void InstagramReparto()
	{
		Application.OpenURL("https://www.instagram.com/repartolaquercia/");
	}

	public void Menu()
	{
		SceneManager.LoadScene(0);
	}
}
