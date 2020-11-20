using UnityEngine;

[CreateAssetMenu(fileName = "New Squadriglia", menuName = "Squadriglia")]
public class Squadriglia : ScriptableObject
{
	public new string name;
	[HideInInspector] [System.NonSerialized]
	public int num; // 1 to 6
	public bool femminile;
}
[System.Serializable]
public class ConcreteSquadriglia
{
	public Squadriglia baseSq;
	public Transform angolo;
	public Transform tenda;
	[HideInInspector] [System.NonSerialized]
	public SpriteRenderer[] buildings;
	[HideInInspector] [System.NonSerialized]
	public Ruolo[] ruoli;
	[HideInInspector] [System.NonSerialized]
	public string[] nomi = new string[5];
	[HideInInspector] [System.NonSerialized]
	public int[] AIPrefabTypes;
	[HideInInspector] [System.NonSerialized]
	public int materials;
	[HideInInspector] [System.NonSerialized]
	public int points;

	public CompactSquadriglia ToCompactSquadriglia()
	{
		var aB = new bool[buildings.Length];
		for (int i = 0; i < aB.Length; i++)
		{
			aB[i] = buildings[i].gameObject.activeSelf;
		}
		return new CompactSquadriglia
		{
			name = baseSq.name,
			femminile = baseSq.femminile,
			activeBuildings = aB,
			nomi = nomi,
			AIPrefabTypes = AIPrefabTypes,
			materials = materials,
			points = points
		};
	}


}
[System.Serializable]
public class CompactSquadriglia
{
	public string name;
	public bool femminile;
	public bool[] activeBuildings;
	public string[] nomi = new string[5];
	public int[] AIPrefabTypes;
	public int materials;
	public int points;
}
