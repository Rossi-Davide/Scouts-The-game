using UnityEngine;


public class Squadrigliere : BaseAI
{
    [HideInInspector]
    public Transform tent;
    [HideInInspector]
    public Squadriglia sq;

	protected override void SetMissingPriorityTargets()
	{
		foreach (var p in priorityTargets)
		{
            if (p.name == "Tenda")
			{
                p.target = tent.position;
			}
		}
	}


	void FaiLegna()
	{
        Debug.Log("sto facendo legna");
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
