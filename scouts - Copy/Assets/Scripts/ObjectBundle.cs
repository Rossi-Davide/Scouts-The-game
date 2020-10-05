using UnityEngine;
[CreateAssetMenu(fileName = "New ObjectBundle", menuName = "ObjectBundle")]
public class ObjectBundle : ScriptableObject
{
	public IterableObjects[] objects;
	[HideInInspector]
	public int nextAction; //index
}
[System.Serializable]
public class IterableObjects
{
	public InGameObject obj;
	public int buttonNum;
}