using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class nascondinoManager : MonoBehaviour
{
    public Animator luceGlobale, pointLight,regole,titolo;
    public GameObject luce1, luce2, testo1, testo2,joystick,player,enemy,countdownStartObj,haisec;
    bool countdownStart = false,countdownStartGrande=false,countdownGiocoInSe=false;
    float seconds = 10f,minutes=0f,secondsInizioGioco=3f;
    public TextMeshProUGUI countdownSeconds,countdownMinutes,countdownSecondsInizio;
    Transform spawnPoint;
    [HideInInspector]
    public bool aumentoDifficoltà=false;
    GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("iniziale");
        spawnPoint = transform.Find("spawnPoint");
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
        yield return new WaitForSeconds(4f);
        titolo.SetBool("uscitaTesto", true);
        yield return new WaitForSeconds(1f);
        testo1.SetActive(false);
        testo2.SetActive(true);
        yield return new WaitForSeconds(7f);
        regole.SetBool("fadeOut", true);
        yield return new WaitForSeconds(1f);
        testo2.SetActive(false);
        pointLight.SetBool("inizioGioco", true);
        yield return new WaitForSeconds(1f);
        luceGlobale.SetBool("inizioGioco", true);
        yield return new WaitForSeconds(2f);
        haisec.SetActive(true);
        yield return new WaitForSeconds(4f);
        haisec.SetActive(false);
        countdownStart = true;
        countdownStartObj.SetActive(true);
        yield return null;
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
            //haisec.SetActive(true);

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
                InizioGioco();
            }
            countdownSeconds.text = seconds.ToString("0");
            //countdownMinutes.text = minutes.ToString();
        }


        if (countdownGiocoInSe == true)
        {
            
            seconds -= 1 * Time.deltaTime;
            if (seconds < 0)
            {
                seconds = 59f;
                minutes -= 1;

            }
            countdownSeconds.text = seconds.ToString("0");
            countdownMinutes.text = minutes.ToString();
            if (minutes <= 0)
            {
                //aumentoDifficoltà = true; da decidere se implementare
            }
            if (minutes <= 0)
            {
                StartCoroutine("Vittoria");
            }
        }
       
    }

    void InizioGioco()
    {
        for(int i = 1; i <= 5; i++)
        {
           enemies[i]= Instantiate(enemy,spawnPoint.position,Quaternion.identity);
        }
    }



    IEnumerator Vittoria()
    {
        /*Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        joystick.SetActive(false);*/
        foreach (GameObject item in enemies)
        {
            Rigidbody2D enemyRb = item.GetComponent<Rigidbody2D>();
            AImaster movement = item.GetComponent<AImaster>();
            movement.enabled = false;
            enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        Debug.Log("hai vinto");
        //animazione da decidere
        yield return null;

    }

    
    
     public  IEnumerator Sconfitta()
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        joystick.SetActive(false);
        foreach (GameObject item in enemies)
        {
            Rigidbody2D enemyRb = item.GetComponent<Rigidbody2D>();
            AImaster movement = item.GetComponent<AImaster>();
            movement.enabled = false;
            enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        Debug.Log("hai perso");
        //animazione da decidere
        yield return null;
    }
}
