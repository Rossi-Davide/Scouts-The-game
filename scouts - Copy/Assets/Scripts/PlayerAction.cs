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
    public BuildingState state;

    [Header("Parameters")]
    public int timeNeeded;
    public int energyGiven, materialsGiven, pointsGiven; // can be less than 0
    public int timeBeforeRedo;


    public Status SendStatus()
    {
        return new Status
        {
            timeBeforeRedo = timeBeforeRedo,
            timeNeeded = timeNeeded,
            energyGiven = energyGiven,
            materialsGiven = materialsGiven,
            pointsGiven = pointsGiven,
        };
    }
    public void SetStatus(Status status)
    {
        if (status != null)
        {
            timeBeforeRedo = status.timeBeforeRedo;
            timeNeeded = status.timeNeeded;
            energyGiven = status.energyGiven;
            pointsGiven = status.pointsGiven;
            materialsGiven = status.materialsGiven;
        }
    }
    [System.Serializable]
    public class Status
    {
        public int timeNeeded;
        public int energyGiven, materialsGiven, pointsGiven;
        public int timeBeforeRedo;
    }


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
