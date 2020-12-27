using TMPro;
using UnityEditor;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	[HideInInspector] [System.NonSerialized]
	public Dialogue currentDialogue;

	#region Singleton
	public static DialogueManager instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("Dialogue manager non è un singleton");
		}
		instance = this;
	}
	#endregion

	public Joystick joy;
	public GameObject dialoguePanel;
	public GameObject blackOverlay;
	GameObject nextButton;
	TextMeshProUGUI title, sentenceText;
	public GameObject[] answerButtons;
	public TextMeshProUGUI[] answerTexts;


	[HideInInspector] [System.NonSerialized]
	public CapieCambu currentObject;


	int deltaPoints, deltaMaterials, deltaEnergy, currentSentenceIndex;
	bool isOpen, canAnswer;

	protected void Start()
	{
		nextButton = dialoguePanel.transform.Find("Next").gameObject;
		title = dialoguePanel.transform.Find("Name").GetComponent<TextMeshProUGUI>();
		sentenceText = dialoguePanel.transform.Find("Sentence").GetComponent<TextMeshProUGUI>();
		isOpen = false;
	}

	public void TogglePanel(Dialogue dialogue)
	{
		joy.canUseJoystick = isOpen;
		isOpen = !isOpen;
		dialoguePanel.SetActive(isOpen);
		blackOverlay.SetActive(isOpen);
		PanZoom.instance.canDo = !isOpen;
		currentSentenceIndex = 0;
		

		if (isOpen) 
		{
			currentDialogue = dialogue;
			deltaPoints = currentDialogue.deltaPoints;
			deltaMaterials = currentDialogue.deltaMaterials;
			deltaEnergy = currentDialogue.deltaEnergy;
			ShowSentence(currentDialogue.sentences[currentSentenceIndex]);
		}
	}
	public void ReEnableJoy()
	{
		joy.canUseJoystick = true;
	}

	public void DisableJoy()
	{
		joy.canUseJoystick = false;
	}
	void ShowSentence(Sentence s)
	{
		title.text = currentObject.name;
		sentenceText.text = s.sentence;

		canAnswer = s.canAnswer;
		ShowPossibleAnswers(s);
	}
	void ShowPossibleAnswers(Sentence s)
	{
		foreach (var b in answerButtons)
			b.SetActive(false);
		foreach (var t in answerTexts)
			t.gameObject.SetActive(false);

		for (int a = 0; a < s.answers.Length; a++)
		{
			answerButtons[a].SetActive(true);
			answerTexts[a].text = s.answers[a].answer;
		}
		nextButton.SetActive(!canAnswer);
	}

	public void NextSentence(int answerNum)//0 or null if no answer
	{
		var s = currentDialogue.sentences[currentSentenceIndex];
		if (canAnswer)
		{
			deltaPoints += s.answers[answerNum].deltaPoints;
			deltaMaterials += s.answers[answerNum].deltaMaterials;
			deltaEnergy += s.answers[answerNum].deltaEnergy;
			currentSentenceIndex = s.answers[answerNum].nextSentenceNum - 1;
		}
		else
		{
			currentSentenceIndex = s.nextSentenceNum - 1;
		}

		if (currentSentenceIndex < currentDialogue.sentences.Length - 1)
		{
			ShowSentence(currentDialogue.sentences[currentSentenceIndex]);
		}
		else
		{
			TogglePanel(null);
			currentObject.nextDialogueIndex++;
			currentObject.Unlock();
			GameManager.instance.ChangeCounter(Counter.Punti, deltaPoints);
			GameManager.instance.ChangeCounter(Counter.Materiali, deltaMaterials);
			GameManager.instance.ChangeCounter(Counter.Energia, deltaEnergy);
		}
	}
}
