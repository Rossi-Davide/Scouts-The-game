using UnityEngine;

[CreateAssetMenu(fileName = "New AI Event", menuName = "AIEvent")]
public class AIEvent : ScriptableObject
{
	public new string name;
	public string description;
	public int countDownLenght;
	public int duration;
	public int day;
	public int hour;
	[HideInInspector] [System.NonSerialized]
	public int timeLeft, countDownLeft;
	[HideInInspector] [System.NonSerialized]
	public bool running;

	public CapieCambu[] mainAIs;

	[System.Serializable]
	public class Status
	{
		public int timeLeft;
		public int countDownLeft;
		public bool running;
	}
	public void SetStatus(Status status)
	{
		timeLeft = status.timeLeft;
		countDownLeft = status.countDownLeft;
		running = status.running;
	}
	public Status SendStatus()
	{
		return new Status
		{
			timeLeft = timeLeft,
			countDownLeft = countDownLeft,
			running = running,
		};
	}
}
