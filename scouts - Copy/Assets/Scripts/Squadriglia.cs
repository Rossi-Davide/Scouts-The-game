using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Squadriglia", menuName = "Squadriglia")]
public class Squadriglia : ScriptableObject
{
	public new string name;
	[HideInInspector]
	public int num; // 1 to 6
	public bool femminile;
}
[System.Serializable]
public class ConcreteSquadriglia
{
	public Squadriglia baseSq;
	public AngoloDiAltraSquadriglia angolo;
	public Transform tenda;
	[HideInInspector]
	public Dictionary<BuildingsActionsAbstract, bool> buildings;
	[HideInInspector]
	public GameManager.Ruolo[] ruoli;
	[HideInInspector]
	public string[] nomi = new string[5];
	[HideInInspector]
	public int materials;
	[HideInInspector]
	public int points;
}
