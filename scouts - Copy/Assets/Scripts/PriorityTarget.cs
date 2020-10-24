using UnityEngine;

[CreateAssetMenu(fileName = "New Priority Target", menuName = "PriorityAITarget")]
public class PriorityTarget : ScriptableObject
{
	public new string name;
	public Vector3 target;
	public int priority;
	public bool waitEndOfCurrentPath;
	public Condition[] conditions;
}
