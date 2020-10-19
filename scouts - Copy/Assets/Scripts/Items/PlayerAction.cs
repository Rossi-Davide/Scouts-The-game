using UnityEngine;

[CreateAssetMenu(fileName = "New Player Action", menuName = "PlayerAction")]
public class PlayerAction : ScriptableObject
{
    public new string name;
    public string description;
    public Item[] neededItems;
    public Condition[] conditions;
    public bool hasInfoPanel;
    public bool showInActionList;

    [Header("Parameters")]
    public int timeNeeded;
    public int energyGiven, materialsGiven, pointsGiven; // can be less than 0
    public int timeBeforeRedo;

    public enum ActionParams
	{
        None,
        timeNeeded,
        energyGiven,
        materialsGiven,
        pointsGiven,
        timeBeforeRedo,
	}


    public void ChangeCountersOnStart()
	{
        if (energyGiven < 0)
            GameManager.instance.ChangeCounter(Counter.Energia, energyGiven);
        if (materialsGiven < 0)
            GameManager.instance.ChangeCounter(Counter.Materiali, materialsGiven);
        if (pointsGiven < 0)
            GameManager.instance.ChangeCounter(Counter.Punti, pointsGiven);
    }
    public void ChangeCountersOnEnd()
	{
        if (energyGiven > 0)
            GameManager.instance.ChangeCounter(Counter.Energia, energyGiven);
        if (materialsGiven > 0)
            GameManager.instance.ChangeCounter(Counter.Materiali, materialsGiven);
        if (pointsGiven > 0)
            GameManager.instance.ChangeCounter(Counter.Punti, pointsGiven);
    }

}
