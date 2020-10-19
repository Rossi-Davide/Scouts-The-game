using UnityEngine;


public class Squadrigliere : BaseAI
{
    [HideInInspector]
    public Transform tent;
    bool homeCoroutineStarted;
    [HideInInspector]
    public Squadriglia sq;

	void FaiLegna()
	{
        Debug.Log("sto facendo legna");
        StartWaitToUseAgain(buttons[0]);
	}

    protected override bool GetConditionValue(ConditionType t)
    {
        switch (t)
        {
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
