using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Squadrigliere : BaseAI
{
    [HideInInspector]
    public Transform tent;
    bool staFacendoLegna, homeCoroutineStarted, puoMandareAFareLegna;
    [HideInInspector]
    public Squadriglia sq;
    [HideInInspector]
    public GameManager.Ruolo nomeRuolo;
    [HideInInspector]
    public TextMesh sqText, ruolo;
	private void Awake()
	{
        sqText = transform.Find("Squadriglia").GetComponent<TextMesh>();
        ruolo = transform.Find("Ruolo").GetComponent<TextMesh>();
    }
	protected override void Start()
	{
        base.Start();
        puoMandareAFareLegna = true;
        sqText.gameObject.SetActive(sq == Player.instance.squadriglia);
        ruolo.gameObject.SetActive(sq == Player.instance.squadriglia);
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
        sqText.gameObject.SetActive(true);
        ruolo.gameObject.SetActive(true);
    }
    public override void Deselect()
    {
        base.Deselect();
        sqText.gameObject.SetActive(sq == Player.instance.squadriglia);
        ruolo.gameObject.SetActive(sq == Player.instance.squadriglia);
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
            case ConditionType.ConditionEDellaStessaSquadriglia: return sq == Player.instance.squadriglia;
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
