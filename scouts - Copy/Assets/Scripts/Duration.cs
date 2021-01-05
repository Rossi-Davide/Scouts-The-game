using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Duration", menuName = "Duration")]
public class Duration : ScriptableObject
{
	public new string name;
	public int totalDays;
	public int actionDurationFactor;
	public int actionWaitTimeFactor;
}