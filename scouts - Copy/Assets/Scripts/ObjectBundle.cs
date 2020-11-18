using UnityEngine;
[CreateAssetMenu(fileName = "New ObjectBundle", menuName = "ObjectBundle")]
public class ObjectBundle : ScriptableObject
{
	public IterableObjects[] objects;
	[Header("DONT MODIFY")]
	public int nextAction; //index
}
[System.Serializable]
public class IterableObjects
{
	public string objectName;
	[HideInInspector] [System.NonSerialized]
	public InGameObject obj;
	public int buttonNum;
}