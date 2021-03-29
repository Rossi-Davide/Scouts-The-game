using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "BuildingState")]
public class BuildingState : ScriptableObject
{
	//the string to add to the object name
	public string[] boolNames;
	public bool variesWithLevel;
	public int priority; //0 to 10 usually
	public PlayerAction action;
	public bool[] boolValues;
	//[HideInInspector] [System.NonSerialized]
	//public bool active;
	//public int keepAfterPlay;
	//[HideInInspector] [System.NonSerialized]
	//public int keepTimeLeft;

	[Header("Indipendent states only:")]
	public Condition[] conditions;
}



//INDIPENDENT STATES
	//conditions
	//play for a certain amount of time when the conditions are met and then play default state even if the conditions haven't changed or play when conditions are met and keep playing until they aren't met anymore
//ACTION STATES
	//play at action start and switch to default when the action ends or play at action end for a certain amount of time

//BOTH
	//save keepAfterPlay left. KeepAfterPlay feature is a bit different for indipendent states but they both have it.