using UnityEngine;
[CreateAssetMenu(fileName = "New Player Action", menuName = "PlayerAction")]

public class PlayerAction : ScriptableObject
{
    public new string name;
    public Item[] neededItems;

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
