using UnityEngine;

[CreateAssetMenu(fileName = "New Building State", menuName = "BuildingState")]
public class BuildingState : ScriptableObject
{
	public string animationSubstring;
	public bool hasAnimation;
	public int priority; //0 to 10 usually
	
}
