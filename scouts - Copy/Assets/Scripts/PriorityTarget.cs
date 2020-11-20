using UnityEngine;

[System.Serializable]
public class PriorityTarget
{
	public string name;
	public Vector3 target;
	public int priority;
	public bool waitEndOfCurrentPath;
	public bool automatic;
	public Condition[] conditions;
}
