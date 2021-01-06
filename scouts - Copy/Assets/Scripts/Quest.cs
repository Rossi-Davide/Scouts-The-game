using UnityEngine;

[CreateAssetMenu(fileName ="New Quest", menuName ="Quest")]
public class Quest : ScriptableObject
{
    public new string name;
    public string description;
    [HideInInspector] [System.NonSerialized]
    public bool prizeTaken;
    public Counter prizeCounter;
	public int PrizeAmount { get { return PrizeAmount * CampManager.instance.possibleDifficulties[CampManager.instance.camp.settings.difficultyIndex].prizesFactor; } set { PrizeAmount = value; } }
	//public int PrizeAmount;
	public PlayerAction action;
    public int timesToDo;
    public int timesDone;

    public void GetPrize()
	{
        GameManager.instance.ChangeCounter(prizeCounter, PrizeAmount);
        prizeTaken = true;
	}

	public Status SendStatus()
	{
		return new Status
		{
			timesDone = timesDone,
			prizeTaken = prizeTaken,
		};
	}
	public void SetStatus(Status status)
	{
		if (status != null)
		{
			timesDone = status.timesDone;
			prizeTaken = status.prizeTaken;
		}
	}
	[System.Serializable]
	public class Status
	{
		public int timesDone;
		public bool prizeTaken;
	}
	public void ResetEditableInfo()
	{
		timesDone = 0;
		prizeTaken = false;
	}
}
