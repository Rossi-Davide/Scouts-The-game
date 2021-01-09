using UnityEngine;

[CreateAssetMenu(fileName = "New Building State", menuName = "BuildingState")]
public class BuildingState : ScriptableObject
{
	public string animationSubstring;
	public bool variesWithLevel;
	public int priority; //0 to 10 usually
	public bool active;
	[HideInInspector] [System.NonSerialized]
	public bool editableActive;
	public void ResetEditableInfo()
	{
		editableActive = active;
	}
}
