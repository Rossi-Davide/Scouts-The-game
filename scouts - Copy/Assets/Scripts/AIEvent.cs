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
	[HideInInspector]
	public int timeLeft, countDownLeft;
	[HideInInspector]
	public bool running;

	public CapieCambu[] mainAIs;
}
