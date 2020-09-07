using UnityEngine;

public class QuestManager : MonoBehaviour
{
	public GameObject questPanel, overlay;
	QuestUI[] quests;
	bool isOpen;
    public void ToggleQuestPanel()
	{

		if (!isOpen)
		{
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

			overlay.SetActive(true);
			questPanel.SetActive(true);
			Time.timeScale = 0;
			isOpen = true;
			quests = questPanel.GetComponentsInChildren<QuestUI>();
			foreach (QuestUI q in quests)
			{
				q.RefreshQuest();
			}
		}
		else
		{
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");

			Time.timeScale = 1;
			isOpen = false;
			questPanel.SetActive(false);
			overlay.SetActive(false);
		}
	}
}
