using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class dialogueManagerTutorial : MonoBehaviour
{

	public DialogueTtutorial types;
	public TextMeshProUGUI description;
	bool isTyping;
	int contatore = 0;
	public GameObject[] capi;
	public GameObject fumo;
	public Animator pointLight;

	void Start()
	{
		NextSentence();
	}

	public void NextSentence()
	{
		if (!isTyping)
		{
			contatore++;
			if (contatore <= types.sentences.Length)
			{
				StopAllCoroutines();
				var sentence = types.sentences[contatore - 1];
				StartCoroutine(TypeSentence(sentence));
			}
			else
			{
				contatore = types.sentences.Length;
			}
			Debug.Log(contatore);
		}
	}

	public void PreviousSentence()
	{
		if (!isTyping)
		{
			contatore--;
			if (contatore >= 1)
			{
				var sentence = types.sentences[contatore - 1];
				StopAllCoroutines();
				StartCoroutine(TypeSentence(sentence));
			}
			else
			{
				contatore = 1;
			}
			Debug.Log(contatore);
		}
	}
	IEnumerator TypeSentence(TutorialSentence sentence)
	{
		isTyping = true;
		description.text = "";
		GameObject capo = Array.Find(capi, el => { return el.name == sentence.capo.ToString(); });
		Array.ForEach(capi, el => el.SetActive(el == capo));
		capo.GetComponent<Animator>().SetBool("staParlando", true);
		pointLight.Play("PointLightTutorial" + capo.name);

		foreach (char item in sentence.sentence.ToCharArray())
		{
			description.text += item;
			yield return new WaitForSeconds(.01f);
		}

		capo.GetComponent<Animator>().SetBool("staParlando", false);
		isTyping = false;
	}
}
