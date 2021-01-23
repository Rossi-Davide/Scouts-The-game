using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Duration
{
	public string name;
	public int totalDays;
	[Header("If this is the easiest and shortest duration, the following aren't calculated")]
	public int actionDurationFactor;
	public int actionWaitTimeFactor;
	public int shopPricesFactor;
	public int actionPricesFactor;
	public double prizesFactor;
}