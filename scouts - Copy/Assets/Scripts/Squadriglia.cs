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
		return new CompactSquadriglia
		{
			baseSq = baseSq,
			ruoli = ruoli,
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
	public Squadriglia baseSq;
	[HideInInspector]
	[System.NonSerialized]
	public Ruolo[] ruoli;
	[HideInInspector]
	[System.NonSerialized]
	public string[] nomi = new string[5];
	[HideInInspector]
	[System.NonSerialized]
	public int[] AIPrefabTypes;
	[HideInInspector]
	[System.NonSerialized]
	public int materials;
	[HideInInspector]
	[System.NonSerialized]
	public int points;

}
