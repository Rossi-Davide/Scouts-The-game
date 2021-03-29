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
	public Joystick joy;
	bool isOpen;
	public GameObject panel, overlay, topArrow, bottomArrow;
	AngoloDiAltraSquadriglia angolo;
	public GameObject[] selectChallengeButtons;
	Challenge selectedChallenge;
	int points = 0;
	private void Start()
	{
		selectedChallenge = Challenge.nascondino;
		topArrow = panel.transform.Find("Texts/Punti/TopArrow").gameObject;
		bottomArrow = panel.transform.Find("Texts/Punti/BottomArrow").gameObject;
	}
	public void ToggleChallengePanel()
	{
		joy.canUseJoystick = isOpen;
		isOpen = !isOpen;
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(isOpen ? "click" : "clickDepitched");
		overlay.SetActive(isOpen);
		panel.SetActive(isOpen);
		PanZoom.instance.canDo = !isOpen;
		RefreshButtons();
	}
	public void ChiudiPannello()
	{
		joy.canUseJoystick = true;
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");
		overlay.SetActive(false);
		panel.SetActive(false);
		PanZoom.instance.canDo = true;
		angolo.ResetWait(0);
	}

	public void ReEnableJoy()
	{
		joy.canUseJoystick = true;
	}


	public void RefreshChallenge(Squadriglia sq, AngoloDiAltraSquadriglia a)
	{
		panel.transform.Find("Texts/Sfidanti").GetComponent<TextMeshProUGUI>().text = sq.name + " VS " + Player.instance.squadriglia.name + " (Tu)";
		angolo = a;
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
		selectedChallenge = (Challenge)(num - 1);
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


	public void StartChallenge()
	{
		if (AIsManager.instance.AreThereAnyRunningEvents == null)
		{
			GameManager.instance.ChangeCounter(Counter.Punti, points);
			CampManager.instance.StartChallenge(selectedChallenge, points);
		}
		else
		{
			ToggleChallengePanel();
			GameManager.instance.WarningOrMessage("Non puoi sfidare una squadriglia durante un evento!", true);
		}
	}

}
public enum Challenge
{
	nascondino,
	labirinto,
}