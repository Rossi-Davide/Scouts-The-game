using UnityEngine;


public class Squadrigliere : BaseAI
{
    [HideInInspector] [System.NonSerialized]
    public Squadriglia sq;

	public override void SetMissingPriorityTarget(string targetName, Vector3 pos)
	{
		foreach (var p in priorityTargets)
		{
            if (p.name == targetName)
			{
                p.target = pos;
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
