using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class dialogueManagerTutorial : MonoBehaviour
{

	public DialogueTtutorial types;
	public TextMeshProUGUI description;
	int contatore = -1;
	public GameObject[] capi;
	public GameObject fumo;
	public Animator pointLight;

	void Start()
	{
		NextSentence();
	}

	public void NextSentence()
	{
		contatore++;
		if (contatore <= types.sentences.Length - 1)
		{
			StopAllCoroutines();
			var sentence = types.sentences[contatore];
			StartCoroutine(TypeSentence(sentence));
		}
		else
		{
			contatore = types.sentences.Length - 1;
		}
		Debug.Log(contatore);
	}

	public void PreviousSentence()
	{
		contatore--;
		if (contatore <= 0)
		{
			var sentence = types.sentences[contatore];
			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentence));
		}
		else
		{
			contatore = 0;
		}
		Debug.Log(contatore);
	}
	IEnumerator TypeSentence(TutorialSentence sentence)
	{
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
	}
}
