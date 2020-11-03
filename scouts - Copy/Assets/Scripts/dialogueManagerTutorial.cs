using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dialogueManagerTutorial : MonoBehaviour
{
    
    public DialogueTtutorial types;
    public TextMeshProUGUI up;
    int contatore = -1;
    bool typing;
    GameObject bongia, cesco, fumo, tommi, fra;
    public Animator pointLight;

    // Start is called before the first frame update
    void Start()
    {
            
        bongia = GameObject.Find("/bongia1");
        cesco = GameObject.Find("/cesco1");
        tommi = GameObject.Find("/tommaso1");
        fra = GameObject.Find("/fra1");
        fumo = GameObject.Find("/cesco1/fumoCesco");
        cesco.SetActive(false);
        fra.SetActive(false);
        tommi.SetActive(false);
        fumo.SetActive(false);
        NextSentence();
    }

    // Update is called once per frame
   



    public void NextSentence()
    {
        if (!typing)
        {
            contatore++;
            if (contatore < types.sentences.Length)
            {
                string sentence = types.sentences[contatore];
                StartCoroutine(TypeSentence(sentence));
                //up.text = sentence;
                if (contatore < 3)
                {
                    bongia.GetComponent<Animator>().SetBool("pausa", false);
                    bongia.GetComponent<Animator>().SetBool("staParlando", true);
                }
                else if (contatore == 3)
                {
                    pointLight.SetBool("bongia", false);
                    pointLight.SetBool("cesco", true);
                    bongia.SetActive(false);
                    cesco.SetActive(true);
                    fumo.SetActive(true);
                    //cesco.GetComponent<Animator>().SetBool("pausa", false);
                    cesco.GetComponent<Animator>().SetBool("awake", true);
                }
                else if (contatore == 6)
                {
                    pointLight.SetBool("cesco", false);
                    pointLight.SetBool("tommi", true);
                    cesco.SetActive(false);
                    fumo.SetActive(false);
                    tommi.SetActive(true);
                    tommi.GetComponent<Animator>().SetBool("pausa", false);
                    tommi.GetComponent<Animator>().SetBool("staParlando", true);
                }
                else if (contatore > 6 && contatore < 9)
                {
                    tommi.GetComponent<Animator>().SetBool("pausa", false);
                    tommi.GetComponent<Animator>().SetBool("staParlando", true);
                }
                else if (contatore == 9)
                {
                    pointLight.SetBool("tommi", false);
                    pointLight.SetBool("fra", true);
                    tommi.SetActive(false);
                    fra.SetActive(true);
                    fra.GetComponent<Animator>().SetBool("pausa", false);
                    fra.GetComponent<Animator>().SetBool("staParlando", true);
                }
                else if (contatore > 9)
                {
                    fra.GetComponent<Animator>().SetBool("pausa", false);
                    fra.GetComponent<Animator>().SetBool("staParlando", true);
                }

            }
            else
            {
                contatore = types.sentences.Length - 1;
            }

            

           
            Debug.Log(contatore);
        }
        
    }

    public void PreviousSentence()
    {
        if (!typing)
        {
            contatore--;
            if (contatore >= 0)
            {
                string sentence = types.sentences[contatore];
                StartCoroutine(TypeSentence(sentence));
                //up.text = sentence;
                if (contatore < 2)
                {
                    bongia.GetComponent<Animator>().SetBool("pausa", false);
                    bongia.GetComponent<Animator>().SetBool("staParlando", true);
                }
                else if (contatore == 2)
                {
                    pointLight.SetBool("cesco", false);
                    pointLight.SetBool("bongia", true);
                    cesco.SetActive(false);
                    fumo.SetActive(false);
                    bongia.SetActive(true);
                    bongia.GetComponent<Animator>().SetBool("pausa", false);
                    bongia.GetComponent<Animator>().SetBool("staParlando", true);
                }
                else if (contatore == 5)
                {
                    pointLight.SetBool("tommi", false);
                    pointLight.SetBool("cesco", true);
                    tommi.SetActive(false);
                    cesco.SetActive(true);
                    fumo.SetActive(true);
                    cesco.GetComponent<Animator>().SetBool("awake", true);
                }
                else if (contatore >= 6 && contatore < 8)
                {
                    tommi.GetComponent<Animator>().SetBool("pausa", false);
                    tommi.GetComponent<Animator>().SetBool("staParlando", true);
                }
                else if (contatore == 8)
                {
                    pointLight.SetBool("fra", false);
                    pointLight.SetBool("tommi", true);
                    fra.SetActive(false);
                    tommi.SetActive(true);
                    tommi.GetComponent<Animator>().SetBool("pausa", false);
                    tommi.GetComponent<Animator>().SetBool("staParlando", true);
                }
                else if (contatore >= 9)
                {
                    fra.GetComponent<Animator>().SetBool("pausa", false);
                    fra.GetComponent<Animator>().SetBool("staParlando", true);
                }
            }
            else
            {
                contatore = 0;
            }
            Debug.Log(contatore);
        }

    }
    

    IEnumerator TypeSentence(string sentence)
    {
        typing = true;
        up.text = "";
        foreach (char item in sentence.ToCharArray())
        {
            up.text += item;
            yield return new WaitForSeconds(.01f);
        }

        if (contatore < 3)
        {
            bongia.GetComponent<Animator>().SetBool("staParlando", false);
            bongia.GetComponent<Animator>().SetBool("pausa", true);
        }
        else if (contatore >= 6 && contatore < 9)
        {
            tommi.GetComponent<Animator>().SetBool("staParlando", false);
            tommi.GetComponent<Animator>().SetBool("pausa", true);
        }
        else if (contatore >= 9)
        {
            fra.GetComponent<Animator>().SetBool("staParlando", false);
            fra.GetComponent<Animator>().SetBool("pausa", true);
        }

        typing = false;
    }



    private void Update()
    {
        
    }
}
