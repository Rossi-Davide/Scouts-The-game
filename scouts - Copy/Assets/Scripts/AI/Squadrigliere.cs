using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Squadrigliere : BaseAI
{
    #region Movement & click events
    public Transform tent;
    bool staFacendoLegna, homeCoroutineStarted;
    public GameManager.Squadriglia nomeSquadriglia;
    public GameManager.Ruolo nomeRuolo;
    TextMesh squadriglia, ruolo;
    public Button mandaAFareLegna;
	protected override void Start()
	{
        squadriglia = transform.Find("Squadriglia").GetComponent<TextMesh>();
        ruolo = transform.Find("Ruolo").GetComponent<TextMesh>();
        squadriglia.text = nomeSquadriglia.ToString();
        ruolo.text = nomeRuolo.ToString();
        if (nomeSquadriglia != Player.instance.squadriglia)
        {
            squadriglia.gameObject.SetActive(false);
            ruolo.gameObject.SetActive(false);
        }
        else
        {
            squadriglia.color = new Color(111, 255, 59);
            ruolo.color = new Color(111, 255, 59);
        }
        base.Start();
    }
	protected override void CreateNewPath()
    {
        if (GameManager.instance.isDay)
        {
            gameObject.SetActive(true);
            homeCoroutineStarted = false;
        }
        else if (!homeCoroutineStarted)
        {
            StartCoroutine(BackToHome());
        }
        else if (staFacendoLegna)
        {
            //?????????
        }
    }
    IEnumerator BackToHome()
    {
        homeCoroutineStarted = true;
        target = tent.position;
        seeker.StartPath(rb.position, target, OnPathCreated);
        yield return new WaitForSeconds(5f);
    }


    
    void OnMouseDown()
    {
        if (nomeSquadriglia != Player.instance.squadriglia)
        {
            StartCoroutine(ShowAgentInfo());
        }
        else
        {
            StartCoroutine(ShowAgentActions());
        }
    }
    IEnumerator ShowAgentInfo()
    {
        squadriglia.gameObject.SetActive(true);
        ruolo.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        squadriglia.gameObject.SetActive(false);
        ruolo.gameObject.SetActive(false);
    }
    IEnumerator ShowAgentActions()
    {
        mandaAFareLegna.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        mandaAFareLegna.gameObject.SetActive(false);
    }
    public void MandaAFareLegna()
    {
        mandaAFareLegna.gameObject.SetActive(false);
        StartCoroutine(FareLegna());
    }
    private GameObject tempTarget;
    IEnumerator FareLegna()
    {
        staFacendoLegna = true;
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            tempTarget = GameManager.instance.spawnedDecorations[Random.Range(0, GameManager.instance.spawnedDecorations.Count)];
            target = tempTarget.transform.position;
            seeker.StartPath(rb.position, target, OnPathCreated);
            yield return new WaitForSeconds(Random.Range(10, 20));
            if (target.x >= transform.position.x)
            {
                GetComponentInChildren<Animator>().Play("leftPunch");
            }
            else
            {
                GetComponentInChildren<Animator>().Play("rightPunch");
            }
            tempTarget.GetComponent<ParticleSystem>().Play();
            yield return new WaitForSeconds(3f);
            tempTarget.GetComponent<ParticleSystem>().Stop();
            GameManager.instance.ChangeCounter(GameManager.Counter.Materiali, 15);
        }
        staFacendoLegna = false;
    }
	#endregion


	protected override void DoAction(ActionButton b)
	{
		throw new System.NotImplementedException();
	}
}
