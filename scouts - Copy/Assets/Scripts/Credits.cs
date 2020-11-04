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
		System.Diagnostics.Process.Start("https://www.instagram.com/big_baddave/");
	}
	public void InstagramSara()
	{
		System.Diagnostics.Process.Start("https://www.instagram.com/sara_cappe22/");
	}
	public void InstagramSimo()
	{
		System.Diagnostics.Process.Start("https://www.instagram.com/simone_ceccarelli__/");
	}
	public void InstagramPietro()
	{
		System.Diagnostics.Process.Start("https://www.instagram.com/pietropappo/");
	}
	public void InstagramReparto()
	{
		System.Diagnostics.Process.Start("https://www.instagram.com/repartolaquercia/");
	}
}
