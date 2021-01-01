using TMPro;
using UnityEngine;

public class ActionInfoPanel : MonoBehaviour
{
	bool isOpen;
	public GameObject panel, overlay;
	PlayerAction selectedAction;
	public Joystick joy;
	public void ReEnableJoy()
	{
		joy.canUseJoystick = true;
	}

	public void DisableJoy()
	{
		joy.canUseJoystick = false;
	}
	public void TogglePanel(int buttonNum)
	{
		if (buttonNum == 0)
			selectedAction = null;
		else
			selectedAction = ActionButtons.instance.selected.GetComponent<InGameObject>().buttons[buttonNum - 1].generalAction;
		isOpen = !isOpen;
		overlay.SetActive(isOpen);
		PanZoom.instance.canDo = !isOpen;
		panel.SetActive(isOpen);
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(isOpen ? "click" : "clickDepitched");
		if (isOpen)
		{
			panel.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = selectedAction.name;
			panel.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = selectedAction.description;
			panel.transform.Find("Tempo").GetComponent<TextMeshProUGUI>().text = "Durata: " + GameManager.IntToMinuteSeconds(selectedAction.editableTimeNeeded);
			panel.transform.Find("Attesa").GetComponent<TextMeshProUGUI>().text = "Tempo di attesa: " + GameManager.IntToMinuteSeconds(selectedAction.editableTimeBeforeRedo);
			var neededItems = panel.transform.Find("ItemRichiesti").GetComponent<TextMeshProUGUI>();
			panel.transform.Find("Counters/Materiali/Value").GetComponent<TextMeshProUGUI>().text = selectedAction.editableMaterialsGiven > 0 ? "+" + selectedAction.editableMaterialsGiven : selectedAction.editableMaterialsGiven.ToString();
			panel.transform.Find("Counters/Energia/Value").GetComponent<TextMeshProUGUI>().text = selectedAction.editableEnergyGiven > 0 ? "+" + selectedAction.editableEnergyGiven : selectedAction.editableEnergyGiven.ToString();
			panel.transform.Find("Counters/Punti/Value").GetComponent<TextMeshProUGUI>().text = selectedAction.editablePointsGiven > 0 ? "+" + selectedAction.editablePointsGiven : selectedAction.editablePointsGiven.ToString();

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
