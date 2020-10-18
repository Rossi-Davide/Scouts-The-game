using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Player Action", menuName = "PlayerAction")]

public class PlayerAction : ScriptableObject
{
    public new string name;
    public string description;
    public Item[] neededItems;
    public Condition[] conditions;
    public bool hasInfoPanel;

    [Header("Parameters")]
    public int timeNeeded;
    public int energyGiven, materialsGiven, pointsGiven; // can be less than 0
    public int timeBeforeRedo;

    public void SetProperties(int timeNeeded, int energyGiven, int materialsGiven, int pointsGiven, int timeBeforeRedo)
	{
        this.timeNeeded = timeNeeded;
        this.energyGiven = energyGiven;
        this.materialsGiven = materialsGiven;
        this.pointsGiven = pointsGiven;
        this.timeBeforeRedo = timeBeforeRedo;
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
}
