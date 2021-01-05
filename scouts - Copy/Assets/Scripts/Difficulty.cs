using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Difficulty", menuName = "Difficulty")]
public class Difficulty : ScriptableObject
{
	public new string name;
	public int shopPricesFactor;
	public int actionPricesFactor;
	public int prizesFactor;
}