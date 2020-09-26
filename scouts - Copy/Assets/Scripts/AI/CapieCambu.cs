using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CapieCambu : BaseAI
{
	Sentence currentSentence;
	[HideInInspector]
	public int dialoguesDone, currentSentenceIndex;
	public GameObject dialoguePanel;
	public GameObject blackOverlay;
	GameObject nextButton, answer1Button, answer2Button;
	TextMeshProUGUI answer1Text, answer2Text, title, sentenceText;
	bool canAnswer, canTalk;
	public Dialogue[] dialoguesArray;
	int pointsToAdd;

	void Awake()
	{
		nextButton = dialoguePanel.transform.Find("Next").gameObject;
		answer1Button = dialoguePanel.transform.Find("Answer1").gameObject;
		answer2Button = dialoguePanel.transform.Find("Answer2").gameObject;
		title = dialoguePanel.transform.Find("Name").GetComponent<TextMeshProUGUI>();
		sentenceText = dialoguePanel.transform.Find("Sentence").GetComponent<TextMeshProUGUI>();
		answer1Text = answer1Button.transform.Find("Text").GetComponent<TextMeshProUGUI>();
		answer2Text = answer2Button.transform.Find("Text").GetComponent<TextMeshProUGUI>();
		canTalk = true;
	}

	void ShowSentence(Sentence s)
	{
		title.text = objectName;
		sentenceText.text = currentSentence.sentence;
		if (s.canAnswer)
		{
			canAnswer = true;
			answer1Button.SetActive(true);
			answer2Button.SetActive(true);
			nextButton.SetActive(false);
			answer1Text.text = s.answer[0].answer;
			answer2Text.text = s.answer[1].answer;
		}
		else
		{
			canAnswer = false;
			answer1Button.SetActive(false);
			answer2Button.SetActive(false);
			nextButton.SetActive(true);
		}
	}
	public void NextSentence(int answerNum)//null if no answer
	{
		if (canAnswer)
		{
			pointsToAdd += currentSentence.answer[answerNum].pointsDelta;
			currentSentenceIndex = currentSentence.answer[answerNum].nextSentenceNum - 1;
		}
		else
		{
			currentSentenceIndex = currentSentence.nextSentenceNum - 1;
		}
		if (currentSentenceIndex < dialoguesArray[dialoguesDone].sentences.Length)
		{
			currentSentence = dialoguesArray[dialoguesDone].sentences[currentSentenceIndex];
			ShowSentence(currentSentence);
		}
		else
		{
			EndTalk();
		}
	}


	public void CancelDialogue()
	{
		ClosePanel();
	}

	protected override void Start()
	{
		base.Start();
		canTalk = true;
	}
	void EndTalk()
	{
		ClosePanel();
		GameManager.instance.ChangeCounter(GameManager.Counter.Punti, pointsToAdd);
		canTalk = false;
		dialoguesDone++;
		StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
	}

	void ClosePanel()
	{
		dialoguePanel.SetActive(false);
		blackOverlay.SetActive(false);
		currentSentenceIndex = 0;
		DialogueManager.instance.selectedCapoOrCambu = null;
		RefreshButtonsState();
	}

	void Talk()
	{
		DialogueManager.instance.selectedCapoOrCambu = this;
		dialoguePanel.SetActive(true);
		blackOverlay.SetActive(true);
		currentSentence = dialoguesArray[dialoguesDone].sentences[currentSentenceIndex];
		ShowSentence(currentSentence);
		pointsToAdd = dialoguesArray[dialoguesDone].basePointsDelta;
	}

	private void OnWaitEnd()
	{
		canTalk = true;
	}

	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanTalkAI: return canTalk;
			case ConditionType.ConditionHasAnythingToSayAI: return dialoguesDone < dialoguesArray.Length;
			default: return base.GetConditionValue(t);
		}
	}

	protected override void DoAction(ActionButton b)
	{
		switch (b.buttonNum)
		{
			case 1:
				Talk();
				break;
			default:
				throw new NotImplementedException();
		}
	}

}




[System.Serializable]
public class Dialogue
{
	public int basePointsDelta;
	public Sentence[] sentences;
}
[System.Serializable]
public class Sentence
{
	public string sentence;
	public bool canAnswer;
	public Answer[] answer;
	public int nextSentenceNum; //more than 1 (it's not like an array)
}

[System.Serializable]
public class Answer
{
	public string answer;
	public int nextSentenceNum;
	public int pointsDelta;
}
