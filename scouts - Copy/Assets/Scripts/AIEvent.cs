using UnityEngine;
using System.Collections;
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
		public InGameObject.Status[] aiInfo;
		public int[] nextDialogueIndices;
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
				//a.GetComponent<Animator>().SetBool("move", true);
				a.ForceToggleName(true);
			}
		}
		for (int s = 0; s < mainAIs.Length; s++)
		{
			mainAIs[s].SetStatus(status.aiInfo[s]);
			mainAIs[s].nextDialogueIndex = status.nextDialogueIndices[s];
		}
	}
	public Status SendStatus()
	{
		int[] indices = new int[mainAIs.Length];
		InGameObject.Status[] c = new InGameObject.Status[mainAIs.Length];
		for (int i = 0; i < mainAIs.Length; i++)
		{
			indices[i] = mainAIs[i].nextDialogueIndex;
			c[i] = mainAIs[i].SendStatus();
		}
		return new Status
		{
			timeLeft = timeLeft,
			countDownLeft = countDownLeft,
			running = running,
			nextDialogueIndices = indices,
			aiInfo = c,
		};
	}

	public void ResetEditableInfo()
	{
		timeLeft = 0;
		countDownLeft = 0;
		running = false;
	}
}
