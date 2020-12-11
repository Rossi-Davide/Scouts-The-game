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

	public Status SendStatus()
	{
		var aB = new bool[buildings.Length];
		for (int i = 0; i < aB.Length; i++)
		{
			aB[i] = buildings[i].gameObject.activeSelf;
		}
		return new Status
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
	public void SetStatus(Status status)
	{
		baseSq.name = status.name;
		baseSq.femminile = status.femminile;
		for (int i = 0; i < buildings.Length; i++)
		{
			buildings[i].gameObject.SetActive(status.activeBuildings[i]);
		}
		nomi = status.nomi;
		AIPrefabTypes = status.AIPrefabTypes;
		materials = status.materials;
		points = status.points;
	}
	[System.Serializable]
	public class Status
	{
		public string name;
		public bool femminile;
		public bool[] activeBuildings;
		public string[] nomi = new string[5];
		public int[] AIPrefabTypes;
		public int materials;
		public int points;
	}


}
