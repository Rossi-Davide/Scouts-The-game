using UnityEngine;

[System.Serializable]
public class AIEvent
{
	public string name;
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
		if (running)
		{
			foreach (var a in mainAIs)
			{
				a.gameObject.SetActive(true);
				a.ToggleClickListener(true);
				a.GetComponent<Animator>().SetBool("move", true);
				a.ForceToggleName(true);
			}
		}
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

	public void ResetEditableInfo()
	{
		timeLeft = 0;
		countDownLeft = 0;
		running = false;
	}
}
