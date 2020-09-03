using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CapieCambu : BaseAI
{
	Sentence currentSentence;
	public new string name;
	[HideInInspector]
	public int dialoguesDone, currentSentenceIndex;
	public GameObject dialoguePanel;
	GameObject nextButton, answer1Button, answer2Button;
	TextMeshProUGUI answer1Text, answer2Text, title, sentenceText;
	bool canAnswer, canTalk, isTalking;
	public Dialogue[] dialoguesArray;
	int pointsDelta;

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

	private void OnMouseDown()
	{
		if (!ClickedObjects.instance.ClickedOnUI)
		{
			if (dialoguesDone < dialoguesArray.Length)
			{
				if (DialogueManager.instance.selectedCapoOrCambu == null)
				{
					if (canTalk)
					{
						dialoguePanel.SetActive(true);
						currentSentence = dialoguesArray[dialoguesDone].sentences[currentSentenceIndex];
						ShowSentence(currentSentence);
						pointsDelta = dialoguesArray[dialoguesDone].basePointsDelta;
						DialogueManager.instance.selectedCapoOrCambu = this;
						isTalking = true;

					}
					else
					{
						string warningText = "Puoi parlare con lo stesso capo o cambusiere solo ogni 60 secondi!";
						GameManager.instance.WarningMessage(warningText);
					}
				}
				else
				{
					Debug.Log("sta già parlando");
					//????
				}
			}
			else
			{
				string warningText = name + " ha già detto tutto quello che aveva da dirti!";
				GameManager.instance.WarningMessage(warningText);
			}
		}
	}
	void ShowSentence(Sentence s)
	{
		title.text = name;
		sentenceText.text = currentSentence.s;
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
			pointsDelta += currentSentence.answer[answerNum].pointsDelta;
			currentSentenceIndex = currentSentence.answer[answerNum].pointsDelta;
		}
		else
		{
			currentSentenceIndex = currentSentence.nextSentenceIndex;

		}
		if (currentSentenceIndex < dialoguesArray[dialoguesDone].sentences.Length)
		{
			currentSentence = dialoguesArray[dialoguesDone].sentences[currentSentenceIndex];
			ShowSentence(currentSentence);
		}
		else
		{
			dialoguePanel.SetActive(false);
			GameManager.instance.ChangeCounter(GameManager.Counter.Punti, pointsDelta);
			canTalk = false;
			StartCoroutine(WaitToTalkAgain(120));
			dialoguesDone++;
			currentSentenceIndex = 0;
			DialogueManager.instance.selectedCapoOrCambu = null;
			isTalking = false;
		}
	}

	IEnumerator WaitToTalkAgain(int secs)
	{
		yield return new WaitForSeconds(secs);
		canTalk = true;
	}


	protected override void CreateNewPath()
	{
		if (isTalking)
		{
			target = Player.instance.transform.position;
			seeker.StartPath(rb.position, target, OnPathCreated);
		}
		else
		{
			base.CreateNewPath();
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
	public string s;
	public bool canAnswer;
	public Answer[] answer;
	public int nextSentenceIndex;
}

[System.Serializable]
public class Answer
{
	public string answer;
	public int nextSentenceIndex;
	public int pointsDelta;
}
