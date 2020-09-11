using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Squadrigliere : BaseAI
{
    [HideInInspector]
    public Transform tent;
    bool staFacendoLegna, homeCoroutineStarted, puoMandareAFareLegna;
    [HideInInspector]
    public GameManager.Squadriglia nomeSquadriglia;
    [HideInInspector]
    public GameManager.Ruolo nomeRuolo;
    TextMesh squadriglia, ruolo;

	protected override void Start()
	{
        squadriglia = transform.Find("Squadriglia").GetComponent<TextMesh>();
        ruolo = transform.Find("Ruolo").GetComponent<TextMesh>();
        squadriglia.text = nomeSquadriglia.ToString();
        ruolo.text = nomeRuolo.ToString();
        puoMandareAFareLegna = true;
        squadriglia.gameObject.SetActive(nomeSquadriglia == Player.instance.squadriglia);
        ruolo.gameObject.SetActive(nomeSquadriglia == Player.instance.squadriglia);
        base.Start();
    }

	void FaiLegna()
	{
        staFacendoLegna = true;
        Debug.Log("sto facendo legna");
        StartCoroutine(WaitToUseAgain(buttons[0], OnWaitEnd));
        staFacendoLegna = false;
	}



	public override void Select()
	{
		base.Select();
        squadriglia.gameObject.SetActive(true);
        ruolo.gameObject.SetActive(true);
    }
    public override void Deselect()
    {
        base.Deselect();
        squadriglia.gameObject.SetActive(nomeSquadriglia == Player.instance.squadriglia);
        ruolo.gameObject.SetActive(nomeSquadriglia == Player.instance.squadriglia);
    }



    private void OnWaitEnd()
    {
        puoMandareAFareLegna = true;
    }

    protected override bool GetConditionValue(ConditionType t)
    {
        switch (t)
        {
            case ConditionType.ConditionPuoMandareAFareLegna: return puoMandareAFareLegna;
            case ConditionType.ConditionEDellaStessaSquadriglia: return nomeSquadriglia == Player.instance.squadriglia;
            case ConditionType.ConditionStaFacendoLegnaAI: return staFacendoLegna;
            default: return base.GetConditionValue(t);
        }
    }

    protected override void DoAction(ActionButton b)
    {
        switch (b.buttonNum)
        {
            case 1:
                FaiLegna();
                break;
            default:
                throw new System.NotImplementedException();
        }
    }
}
