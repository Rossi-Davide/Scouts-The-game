using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class nascondinoManager : MonoBehaviour
{
    public Animator luceGlobale, pointLight;
    public GameObject luce1, luce2, testo1, testo2,joystick,player,enemy,countdownStartObj,haisec;
    bool countdownStart = false,countdownStartGrande=false,countdownGiocoInSe=false;
    float seconds = 10f,minutes=0f,secondsInizioGioco=3f;
    public TextMeshProUGUI countdownSeconds,countdownMinutes,countdownSecondsInizio;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("iniziale");
    }

    // Update is called once per frame
   

    IEnumerator iniziale()
    {
        player.SetActive(false);
        joystick.SetActive(false);
        luce1.SetActive(true);
        yield return new  WaitForSeconds(0.5f);


        luce2.SetActive(true);
        yield return new WaitForSeconds(2f);
        testo1.SetActive(true);
        yield return new WaitForSeconds(3f);

        testo1.SetActive(false);
        testo2.SetActive(true);
        yield return new WaitForSeconds(5f);
        testo2.SetActive(false);
        pointLight.SetBool("inizioGioco", true);
        yield return new WaitForSeconds(1f);
        luceGlobale.SetBool("inizioGioco", true);
        yield return new WaitForSeconds(0.5f);
        countdownStart = true;
        countdownStartObj.SetActive(true);
        yield return null;
    }

    void InizioGioco()
    {
        
    }


    void Update()
    {
        if (countdownStart == true)
        {
            secondsInizioGioco -= 1 * Time.deltaTime;
            countdownSecondsInizio.text = secondsInizioGioco.ToString("0");
            if (secondsInizioGioco < 0)
            {
                countdownStartGrande = true;
                countdownStartObj.SetActive(false);
                countdownStart = false;
            }
            
        }

        if (countdownStartGrande == true)
        {
            haisec.SetActive(true);

            joystick.SetActive(true);
            player.SetActive(true);
            seconds -= 1 * Time.deltaTime;
            if (seconds < 0)
            {
                seconds = 59f;
                countdownGiocoInSe = true;
                minutes = 1;
                //minutes -= 1;
                countdownStartGrande = false;
            }
            countdownSeconds.text = seconds.ToString("0");
            //countdownMinutes.text = minutes.ToString();
        }


        if (countdownGiocoInSe == true)
        {
            joystick.SetActive(true);
            player.SetActive(true);
            seconds -= 1 * Time.deltaTime;
            if (seconds < 0)
            {
                seconds = 59f;
                minutes -= 1;

            }
            countdownSeconds.text = seconds.ToString("0");
            countdownMinutes.text = minutes.ToString();
        }
        
    }
}
