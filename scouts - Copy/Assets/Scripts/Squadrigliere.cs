using System.Collections;
using UnityEngine;


public class Squadrigliere : BaseAI
{
    [HideInInspector] [System.NonSerialized]
    public Squadriglia sq;
    Plant currentPlant;

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
        for (int i = 0; i < Random.Range(2, 5); i++)
		{
            currentPlant = GameManager.instance.spawnedPlants[Random.Range(0, GameManager.instance.spawnedPlants.Length)];
            ForceTarget(currentPlant.transform.position, false, false);
		}
    }
    void EndLegna()
	{
        Destroy(currentPlant);
        GameManager.instance.ChangeCounter(Counter.Materiali, 15);
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
