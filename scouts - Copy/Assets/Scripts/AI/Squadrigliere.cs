using UnityEngine;


public class Squadrigliere : BaseAI
{
    [HideInInspector]
    public Transform tent;
    bool staFacendoLegna, homeCoroutineStarted;
    [HideInInspector]
    public Squadriglia sq;
    [HideInInspector]
    public GameManager.Ruolo nomeRuolo;
    [HideInInspector]
    public TextMesh sqText, ruolo;

	protected override void Start()
	{
        base.Start();
        sqText = transform.Find("Squadriglia").GetComponent<TextMesh>();
        ruolo = transform.Find("Ruolo").GetComponent<TextMesh>();
        sqText.gameObject.SetActive(sq == Player.instance.squadriglia);
        ruolo.gameObject.SetActive(sq == Player.instance.squadriglia);
    }

	void FaiLegna()
	{
        staFacendoLegna = true;
        Debug.Log("sto facendo legna");
        StartWaitToUseAgain(buttons[0]);
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

    protected override bool GetConditionValue(ConditionType t)
    {
        switch (t)
        {
            case ConditionType.ConditionStaFacendoLegnaAI: return staFacendoLegna;
            case ConditionType.ConditionEDellaStessaSquadriglia: return sq == Player.instance.squadriglia;
            default: return base.GetConditionValue(t);
        }
    }

    protected override System.Action DoAction(ActionButton b)
    {
        switch (b.buttonNum)
        {
            case 1:
                FaiLegna();
                return null;
            default:
                throw new System.NotImplementedException();
        }
    }
}
