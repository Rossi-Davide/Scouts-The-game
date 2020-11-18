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
}
