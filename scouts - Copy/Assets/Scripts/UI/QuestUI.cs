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
