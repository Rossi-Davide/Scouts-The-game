using UnityEngine;
[CreateAssetMenu(fileName ="New Action", menuName ="Action")]

public class PlayerAction : ScriptableObject
{
    public new string name;
    public string description;
    public Item[] neededItems;
    public Item[] optionalItems;
    public bool canBeDone;
    [Header("Parameters")]
    public int timeNeeded;
    public int energyNeeded, materialsNeeded, pointsNeeded;
    public int energyGiven, materialsGiven, pointsGiven;
    public int timeBeforeRedo;
    
    public enum ActionParams
	{
        None,
        timeNeeded,
        energyNeeded,
        materialsNeeded,
        pointsNeeded,
        energyGiven,
        materialsGiven,
        pointsGiven,
        timeBeforeRedo,
	}
}
