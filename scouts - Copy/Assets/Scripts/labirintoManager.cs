using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class labirintoManager : MonoBehaviour
{
    public Animator luceGlobale, pointLight, regole, titolo,pannelloTimer,timer;
    public GameObject luce1, luce2, testo1, testo2, joystick, player, enemy, haisec,riavvioScenaErrorMessage,levGen,panel;
    bool countdownStart = false, countdownStartGrande = false;
    public TextMeshProUGUI editorCountdown, countdownSecondsInizio;
    Transform spawnPoint;
    [HideInInspector]
    public bool aumentoDifficoltà = false;
    GameObject[] enemies;
    public int seconds, secondsInizioGioco;
    LevelGenerator l;
    int StopScene = 15;
    bool StopSceneTrigger = false,alreadyStarted=false;
    public int nSpilli,nDinamiti,nCaramelle;
    public GameObject spillo, dinamite,candy,bottoneTornaAlGioco,sconfitta,victoryText,score;
    public bool endGen = false;
    public movimentoLabirinto movePlayer;

    #region Utility functions
    public static string IntToMinutesColonSeconds(int time)
    {
        string st = "";
        int other = time % 3600;
        int hours = (time - other) / 3600;
        int seconds = other % 60;
        int minutes = (other - seconds) / 60;
        if (hours > 0)
            st = hours > 10 ? hours + "." : "0" + hours + ".";
        st += minutes > 10 ? minutes + ":" : "0" + minutes + ":";
        st += seconds > 10 ? seconds.ToString() : ("0" + seconds);
        return st;
    }
    #endregion


    public void CollsDisabled()
    {
        endGen = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.Find("spawnPoint");
        InvokeRepeating("CountDown", 1, 1);
        secondsInizioGioco = 3;
        l = levGen.GetComponent<LevelGenerator>();
        panel.SetActive(true);
        GameObject audio = GameObject.Find("/AudioManager");

        if (audio != null)
        {
            audio.GetComponent<AudioManager>().Play("musicaGiochi");
        }
        else
        {
            Debug.Log("audioManager non trovato");
        }

    }

    // Update is called once per frame


    IEnumerator Iniziale()
    {
        panel.SetActive(false);
        //GameObject panel = transform.FindChild("Panel").gameObject;
        editorCountdown.transform.parent.gameObject.SetActive(false);
        //player.transform.position = spawnPoint.position;
        player.SetActive(true);
        joystick.SetActive(false);
        luce1.SetActive(true);
        yield return new WaitForSeconds(0.5f);


        luce2.SetActive(true);
        yield return new WaitForSeconds(0.5f); // era 2
        testo1.SetActive(true);
        yield return new WaitForSeconds(2f); // era 4
        titolo.SetBool("uscitaTesto", true);
        yield return new WaitForSeconds(1f);
        testo1.SetActive(false);
        testo2.SetActive(true);
        yield return new WaitForSeconds(5f); // era 7
        regole.SetBool("fadeOut", true);
        yield return new WaitForSeconds(1f);
        testo2.SetActive(false);
        pointLight.SetBool("inizioGioco", true);
        yield return new WaitForSeconds(1f);
        luceGlobale.SetBool("inizioGioco", true);
        yield return new WaitForSeconds(2f);
        countdownStart = true;
        countdownSecondsInizio.gameObject.SetActive(true);
        countdownSecondsInizio.text = secondsInizioGioco.ToString();
        for(int i = 0; i < nSpilli; i++)
        {
            Instantiate(spillo);
        }
        for (int i = 0; i < nDinamiti; i++)
        {
            Instantiate(dinamite);
        }
        for (int i = 0; i < nCaramelle; i++)
        {
            Instantiate(candy);
        }
        yield return null;
    }


    void CountDown()
    {
        if (countdownStart == true)
        {
            secondsInizioGioco--;
            countdownSecondsInizio.text = secondsInizioGioco.ToString();
            if (secondsInizioGioco < 0)
            {
                countdownStartGrande = true;
                countdownSecondsInizio.gameObject.SetActive(false);
                countdownStart = false;
                editorCountdown.transform.parent.gameObject.SetActive(true);
            }

        }

        if (countdownStartGrande == true)
        {
            joystick.SetActive(true);
            //player.SetActive(true);
            seconds--;
            if (seconds < 0)
            {
                StartCoroutine(Sconfitta());

                countdownStartGrande = false;
                //InizioGioco();
            }
            editorCountdown.text = IntToMinutesColonSeconds(seconds);
        }

        if (StopSceneTrigger == false)
        {
            StopScene--;
        }
    }


    private void Update()
    {
        


        if (StopScene < 0)
        {
            StartCoroutine(ricaricaScena());
        }
        if (l.stopGeneration == true)
        {
            StopSceneTrigger = true;
            if (alreadyStarted == false)
            {
                StartCoroutine(Iniziale());

                alreadyStarted = true;
            }
        }
    }



    IEnumerator ricaricaScena()
    {
        riavvioScenaErrorMessage.SetActive(true);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


   



    public IEnumerator Sconfitta()
    {
        seconds = 0;
        editorCountdown.text = IntToMinutesColonSeconds(seconds);
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        movePlayer.enabled = false;
        playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        joystick.SetActive(false);
       
        //Debug.Log("hai perso");
        //animazione da decidere

        luceGlobale.SetBool("inizioGioco", false);
        countdownStartGrande = false;
        yield return new WaitForSeconds(0.5f);

        pannelloTimer.SetBool("perditaGioco", true);
        timer.SetBool("perditaGioco", true);
        yield return new WaitForSeconds(0.5f);
        Animator bottoneTornaAlGiocoAn = bottoneTornaAlGioco.GetComponent<Animator>();
        bottoneTornaAlGiocoAn.SetBool("fineGioco", true);
        yield return new WaitForSeconds(0.5f);
        Animator sconfittaAn = sconfitta.GetComponent<Animator>();
        sconfittaAn.SetBool("fineGioco", true);
        yield return null;
    }

    public IEnumerator Vittoria()
    {
        Debug.Log("vittoria");
        luceGlobale.SetBool("inizioGioco", false);
        countdownStartGrande = false;
        yield return new WaitForSeconds(0.5f);
        Animator victoryTextAn = victoryText.GetComponent<Animator>();
        victoryTextAn.SetBool("vinto", true);
        yield return new WaitForSeconds(0.5f);
        Animator scoreAn = score.GetComponent<Animator>();
        scoreAn.SetBool("vinto", true);
        yield return new WaitForSeconds(0.5f);
        Animator bottoneTornaAlGiocoAn = bottoneTornaAlGioco.GetComponent<Animator>();
        bottoneTornaAlGiocoAn.SetBool("fineGioco", true);
        yield return null;

    }
}
