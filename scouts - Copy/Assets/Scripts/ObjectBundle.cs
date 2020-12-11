using UnityEngine;
[CreateAssetMenu(fileName = "New ObjectBundle", menuName = "ObjectBundle")]
public class ObjectBundle : ScriptableObject
{
	public PlayerAction[] objects;
	[Header("DONT MODIFY")]
	public int nextAction; //index

	public Status SendStatus()
	{
		return new Status
		{
			nextAction = nextAction,
		};
	}
	void SetStatus(Status status)
	{
		if (status != null)
		{
			nextAction = status.nextAction;
		}
	}
	[System.Serializable]
	public class Status
	{
		public int nextAction;
	}

}