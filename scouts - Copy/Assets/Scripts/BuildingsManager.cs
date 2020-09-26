using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class BuildingsManager : MonoBehaviour
{
	bool isOpen;
	public GameObject panel, overlay;
	public GameObject[] buttons;
	ConcreteSquadriglia playerSq;
	private void Start()
	{
		playerSq = SquadrigliaManager.instance.GetPlayerSq();
	}
	public void ToggleBuildingsPanel()
	{
		isOpen = !isOpen;
		panel.SetActive(isOpen);
		overlay.SetActive(isOpen);
		RefreshUI();
	}
	void RefreshUI()
	{
		for (int i = 0; i < playerSq.buildings.Length; i++)
		{
			var b = playerSq.buildings[i];
			var button = buttons[i].transform.Find("Button");
			button.GetComponent<Animator>().Play(b.currentLevelIndex >= b.building.maxLevel && b.gameObject.activeSelf ? "Disabled" : "Enabled");
			button.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = b.building.prices[b.currentLevelIndex] > GameManager.instance.materialsValue ? Color.red : Color.white;
			button.parent.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = b.building.prices[b.currentLevelIndex].ToString();
			button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = b.gameObject.activeSelf ? "Migliora" : "Costruisci";
		}
	}
	public void BuildOrImprove(int index)
	{
		var b = playerSq.buildings[index];
		if (b.currentLevelIndex >= b.building.maxLevel && b.gameObject.activeSelf)
		{
			GameManager.instance.WarningMessage($"La costruzione {b.objectName} è già al livello massimo!");
			return;
		}
		else if (GameManager.instance.materialsValue < b.building.prices[b.currentLevelIndex])
		{
			GameManager.instance.WarningMessage("Non hai abbastanza materiali!");
			return;
		}
		else if (b.gameObject.activeSelf)
		{
			b.currentLevelIndex++;
		}
		else
		{
			b.gameObject.SetActive(true);
			b.instanceOfListener.SetActive(true);
		}
		RefreshUI();
	}
}
