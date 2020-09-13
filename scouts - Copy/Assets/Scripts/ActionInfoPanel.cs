using TMPro;
using UnityEngine;

public class ActionInfoPanel : MonoBehaviour
{
	bool isOpen;
	public GameObject panel, overlay;
	PlayerAction selectedAction;

	public void TogglePanel(int buttonNum)
	{
		if (buttonNum == 0)
			selectedAction = null;
		else
			selectedAction = ActionButtons.instance.selected.GetComponent<InGameObject>().buttons[buttonNum - 1].generalAction;
		isOpen = !isOpen;
		overlay.SetActive(isOpen);
		panel.SetActive(isOpen);
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(isOpen ? "click" : "clickDepitched");
		if (isOpen)
		{
			panel.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = selectedAction.name;
			panel.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = selectedAction.description;
			panel.transform.Find("Tempo").GetComponent<TextMeshProUGUI>().text = "Durata: " + GameManager.IntToMinuteSeconds(selectedAction.timeNeeded);
			panel.transform.Find("Attesa").GetComponent<TextMeshProUGUI>().text = "Tempo di attesa: " + GameManager.IntToMinuteSeconds(selectedAction.timeBeforeRedo);
			var neededItems = panel.transform.Find("ItemRichiesti").GetComponent<TextMeshProUGUI>();
			panel.transform.Find("Counters/Materiali/Value").GetComponent<TextMeshProUGUI>().text = selectedAction.materialsGiven > 0 ? "+" + selectedAction.materialsGiven : selectedAction.materialsGiven.ToString();
			panel.transform.Find("Counters/Energia/Value").GetComponent<TextMeshProUGUI>().text = selectedAction.energyGiven > 0 ? "+" + selectedAction.energyGiven : selectedAction.energyGiven.ToString();
			panel.transform.Find("Counters/Punti/Value").GetComponent<TextMeshProUGUI>().text = selectedAction.pointsGiven > 0 ? "+" + selectedAction.pointsGiven : selectedAction.pointsGiven.ToString();

			neededItems.text = "Item richiesti: ";
			if (selectedAction.neededItems.Length == 0)
				neededItems.text += "nessuno.";
			else
			{
				for (int i = 0; i < selectedAction.neededItems.Length - 1; i++)
				{
					neededItems.text += selectedAction.neededItems[i].name + ", ";
				}
				neededItems.text += selectedAction.neededItems[selectedAction.neededItems.Length - 1].name + ".";
			}
		}
	}
}
