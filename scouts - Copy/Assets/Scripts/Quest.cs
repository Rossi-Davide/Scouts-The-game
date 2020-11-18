using UnityEngine;

[CreateAssetMenu(fileName ="New Quest", menuName ="Quest")]
public class Quest : ScriptableObject
{
    public new string name;
    public string description;
    [HideInInspector] [System.NonSerialized]
    public bool prizeTaken;
    public Counter prizeCounter;
    public int prizeAmount;
	public PlayerAction action;
    public int timesToDo;
    public int timesDone;


    public void GetPrize()
	{
        GameManager.instance.ChangeCounter(prizeCounter, prizeAmount);
        prizeTaken = true;
	}
}
