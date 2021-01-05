using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : MonoBehaviour
{
	public Quest quest;
	Slider bar;
	TextMeshProUGUI title;
	TextMeshProUGUI description;
	TextMeshProUGUI barValue;
	TextMeshProUGUI prizeValue;
	TextMeshProUGUI completato;
	GameObject energyLogo;
	GameObject materialsLogo;
	GameObject pointsLogo;
	Button riscuoti;
	void Awake()
	{
		bar = transform.Find("Bar").GetComponent<Slider>();
		barValue = bar.transform.Find("Value").GetComponent<TextMeshProUGUI>();
		title = transform.Find("Titolo").GetComponent<TextMeshProUGUI>();
		description = title.transform.Find("Description").GetComponent<TextMeshProUGUI>();
		prizeValue = transform.Find("Prize/Value").GetComponent<TextMeshProUGUI>();
		energyLogo = transform.Find("Prize/Energia").gameObject;
		materialsLogo = transform.Find("Prize/Materiali").gameObject;
		pointsLogo = transform.Find("Prize/Punti").gameObject;
		riscuoti = transform.Find("Riscuoti").GetComponent<Button>();
		completato = transform.Find("Completato").GetComponent<TextMeshProUGUI>();
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
		barValue.text = quest.timesDone + "/" + quest.timesToDo;
		prizeValue.text = quest.PrizeAmount.ToString();
		bar.maxValue = quest.timesToDo;
		bar.value = quest.timesDone;
		energyLogo.SetActive(quest.prizeCounter == Counter.Energia);
		materialsLogo.SetActive(quest.prizeCounter == Counter.Materiali);
		pointsLogo.SetActive(quest.prizeCounter == Counter.Punti);
		if (quest.timesDone >= quest.timesToDo)
		{
			bar.gameObject.SetActive(false);
			completato.gameObject.SetActive(quest.prizeTaken);
			riscuoti.gameObject.SetActive(!quest.prizeTaken);
		}
		else
		{
			bar.gameObject.SetActive(true);
			completato.gameObject.SetActive(false);
			riscuoti.gameObject.SetActive(false);
		}
	}
}
