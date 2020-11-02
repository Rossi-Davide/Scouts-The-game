using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dialogueManagerTutorial : MonoBehaviour
{
    
    public DialogueTtutorial types;
    public TextMeshProUGUI up;
    int contatore = -1;

    // Start is called before the first frame update
    void Start()
    {
        NextSentence();
    }

    // Update is called once per frame
   



    public void NextSentence()
    {
        contatore++;
        if (contatore < types.sentences.Length)
        {
            string sentence = types.sentences[contatore];
            StartCoroutine(TypeSentence(sentence));
            //up.text = sentence;
           
        }
        else
        {
            contatore = types.sentences.Length-1;
        }     
    }

    public void PreviousSentence()
    {
        contatore--;
        if (contatore >=0)
        {
            string sentence = types.sentences[contatore];
           
            //up.text = sentence;
    
        }
        else
        {
            contatore = 0;
        }
    }
    

    IEnumerator TypeSentence(string sentence)
    {
        up.text = "";
        foreach (char item in sentence.ToCharArray())
        {
            up.text += item;
            yield return null;
        }
    }
}
