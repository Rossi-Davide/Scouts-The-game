using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
	public Quest quest;
	Slider bar;
	Text title;
	Text description;
	Text barValue;
	Text prizeValue;
	Text completato;
	GameObject energyLogo;
	GameObject materialsLogo;
	GameObject pointsLogo;
	Button riscuoti;
	void Awake()
	{
		bar = transform.Find("Bar").GetComponent<Slider>();
		barValue = bar.transform.Find("Value").GetComponent<Text>();
		title = transform.Find("Titolo").GetComponent<Text>();
		description = title.transform.Find("Description").GetComponent<Text>();
		prizeValue = transform.Find("Prize/Value").GetComponent<Text>();
		energyLogo = transform.Find("Prize/Energia").gameObject;
		materialsLogo = transform.Find("Prize/Materiali").gameObject;
		pointsLogo = transform.Find("Prize/Punti").gameObject;
		riscuoti = transform.Find("Riscuoti").GetComponent<Button>();
		completato = transform.Find("Completato").GetComponent<Text>();
		riscuoti.onClick.AddListener(Riscuoti);
	}
	void Riscuoti()
	{
		quest.GetPrize();
		RefreshQuest();
	}

	public void RefreshQuest()
	{
		title.text = quest.name;
		description.text = quest.description;
		if (quest.timesDone >= quest.timesToDo)
		{
			bar.enabled = false;
			completato.enabled = quest.prizeTaken;
			riscuoti.enabled = !quest.prizeTaken;
		}
		else
		{
			bar.enabled = true;
			bar.maxValue = quest.timesToDo;
			bar.value = quest.timesDone;
			riscuoti.enabled = false;
			completato.enabled = false;
			barValue.text = quest.timesDone + "/" + quest.timesToDo;
			prizeValue.text = quest.prizeAmount.ToString();
			energyLogo.SetActive(quest.prizeCounter == GameManager.Counter.Energia);
			materialsLogo.SetActive(quest.prizeCounter == GameManager.Counter.Materiali);
			pointsLogo.SetActive(quest.prizeCounter == GameManager.Counter.Punti);
		}
	}
}
