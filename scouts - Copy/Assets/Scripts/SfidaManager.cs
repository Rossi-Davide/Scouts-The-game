using UnityEngine;
using TMPro;

public class SfidaManager : MonoBehaviour
{
	#region Singleton
	public static SfidaManager instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("SfidaManager non è un singleton!");
		}
		instance = this;
	}

	#endregion

	bool isOpen;
	public GameObject panel, overlay, topArrow, bottomArrow;
	public GameObject[] selectChallengeButtons;
	Challenge selectedChallenge;
	int points = 0;
	private void Start()
	{
		selectedChallenge = Challenge.hebert;
		topArrow = panel.transform.Find("Texts/Punti/TopArrow").gameObject;
		bottomArrow = panel.transform.Find("Texts/Punti/BottomArrow").gameObject;
	}
	public void ToggleChallengePanel()
	{
		if (!isOpen)
		{
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

			overlay.SetActive(true);
			panel.SetActive(true);
			isOpen = true;
			RefreshButtons();
		}
		else
		{
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");

			isOpen = false;
			panel.SetActive(false);
			overlay.SetActive(false);
		}
	}


	public void RefreshChallenge(Squadriglia sq)
	{
		panel.transform.Find("Texts/Sfidanti").GetComponent<TextMeshProUGUI>().text = sq.name + " VS " + Player.instance.squadriglia.name + " (Tu)";
	}

	void RefreshButtons()
	{
		for (int i = 0; i < selectChallengeButtons.Length; i++)
		{
			selectChallengeButtons[i].GetComponent<Animator>().Play((int)selectedChallenge == i ? "Enabled" : "Disabled");
		}
		topArrow.GetComponent<Animator>().Play(points + 1 > GameManager.instance.GetCounterValue(Counter.Punti) ? "Disabled" : "Enabled");
		bottomArrow.GetComponent<Animator>().Play(points - 1 < 0 ? "Disabled" : "Enabled");
		panel.transform.Find("Texts/Punti/Value").GetComponent<TextMeshProUGUI>().text = points.ToString();
	}
	public void SelectChallenge(int num)
	{
		selectedChallenge = (Challenge)num - 1;
		RefreshButtons();
	}

	public void ChangePoints(int delta)
	{
		points += delta;
		if (points > GameManager.instance.GetCounterValue(Counter.Punti))
			points--;
		if (points < 0)
			points++;
		RefreshButtons();
	}


}
public enum Challenge
{
	nascondino,
	hebert,
	sumo,
	//labirinto,
}