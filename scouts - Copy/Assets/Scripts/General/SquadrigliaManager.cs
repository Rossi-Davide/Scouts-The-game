using UnityEngine;

public class SquadrigliaManager : MonoBehaviour
{
	#region Singleton
	public static SquadrigliaManager instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("SquadrigliaManager non è un singleton!");
		}
		instance = this;
	}

	#endregion
	public Squadriglia[] squadrigliePossibili;
	public Squadriglia[] squadriglieInGioco;
	string[] nomiF = { "Sara", "Sofia", "Beatrice", "Federica", "Caterina", "Emma", "Vera", "Lucia", "Martina", "Matilde", "Letizia", "Giada", "Chiara", "Annalisa", "Francesca", "Victoria", "Giulia", "Ginevra", "Viola", "Greta", "Aurora", "Simona", "Monica", "Bianca", "Miriam", "Gloria", "Greta", "Ilaria", "Bianca", "Anita", "Gilda" };
	string[] nomiM = { "Simone", "Lorenzo", "Nicola", "Francesco", "Michele", "Giovanni", "Fabio", "Nicolò", "Davide", "Matteo", "Tommaso", "Samuele", "Raffaele", "Giulio", "Pietro", "Luca", "Andrea", "Giacomo", "Gianluca", "Riccardo", "Filippo", "Lukas" };
	#region Metodi per dare informazioni sulle squadriglie
	public string GetSquadrigliaName(int n)
	{
		foreach (Squadriglia sq in squadriglieInGioco)
		{
			if (sq.num == n)
			{
				if (Player.instance.squadriglia == sq.name)
				{
					return "Squadriglia " + sq.name + " (Tu)";
				}
				else
				{
					return "Squadriglia " + sq.name;
				}
			}
		}
		throw new System.Exception("La squadriglia ricercata non esiste" + n);
	}
	public string GetSquadrigliaDescription(int n)
	{
		foreach (Squadriglia sq in squadriglieInGioco)
		{
			if (sq.num == n)
			{
				return "CapoSquadriglia: " + sq.capo + "\nVice: " + sq.vice + "\nTerzino: " + sq.terzino + "\nNovizio: " + sq.novizio1 + "\nNovizio: " + sq.novizio2;
			}
		}
		throw new System.Exception("La squadriglia ricercata non esiste" + n);
	}
	public string GetSquadrigliaMaterials(int n)
	{
		foreach (Squadriglia sq in squadriglieInGioco)
		{
			if (sq.num == n)
			{
				return sq.materials.ToString();
			}
		}
		throw new System.Exception("La squadriglia ricercata non esiste");
	}
	public string GetSquadrigliaPoints(int n)
	{
		foreach (Squadriglia sq in squadriglieInGioco)
		{
			if (sq.num == n)
			{
				return sq.points.ToString();
			}
		}
		throw new System.Exception("La squadriglia ricercata non esiste");
	}
	#endregion
	#region Nomi a caso
	public void GetRandomNames()
	{
		foreach (Squadriglia sq in squadriglieInGioco)
		{
			if (sq.femminile)
			{
				sq.capo = nomiF[Random.Range(0, nomiF.Length)];
				sq.vice = nomiF[Random.Range(0, nomiF.Length)];
				sq.terzino = nomiF[Random.Range(0, nomiF.Length)];
				sq.novizio1 = nomiF[Random.Range(0, nomiF.Length)];
				sq.novizio2 = nomiF[Random.Range(0, nomiF.Length)];
			}
			else
			{
				sq.capo = nomiM[Random.Range(0, nomiM.Length)];
				sq.vice = nomiM[Random.Range(0, nomiM.Length)];
				sq.terzino = nomiM[Random.Range(0, nomiM.Length)];
				sq.novizio1 = nomiM[Random.Range(0, nomiM.Length)];
				sq.novizio2 = nomiM[Random.Range(0, nomiM.Length)];
			}
			if (Player.instance.squadriglia == sq.name)
			{
				sq.capo = Player.instance.playerName + " (Tu)";
			}
		}
	}
	#endregion
	#region Counters
	private void Start()
	{
		GameManager.instance.OnMaterialsChange += RefreshPlayerMaterials;
		GameManager.instance.OnPointsChange += RefreshPlayerPoints;
		InvokeRepeating("ChangeOtherSqCounters", 30, Random.Range(15, 40));
		GetRandomNames();
	}
	private void OnDestroy()
	{
		GameManager.instance.OnMaterialsChange -= RefreshPlayerMaterials;
		GameManager.instance.OnPointsChange -= RefreshPlayerPoints;
	}
	void RefreshPlayerMaterials(int newValue)
	{
		foreach (Squadriglia sq in squadriglieInGioco)
		{
			if (sq.name == Player.instance.squadriglia)
			{
				sq.materials = newValue;
			}
		}
	}
	void RefreshPlayerPoints(int newValue)
	{
			foreach (Squadriglia sq in squadriglieInGioco)
			{
				if (sq.name == Player.instance.squadriglia)
				{
					sq.points = newValue;
				}
			}
	}

	void ChangeOtherSqCounters()
	{
		foreach (Squadriglia sq in squadriglieInGioco)
		{
			if (sq.name != Player.instance.squadriglia)
			{
				if (Random.Range(0, 100) >= 50)
				{
					sq.materials += Random.Range(5, 10);
				}
				if (Random.Range(0, 100) >= 70)
				{
					sq.points += Random.Range(2, 6);
				}
			}
		}
	}
	#endregion
	}


	[System.Serializable]
	public class Squadriglia
	{
		public int num;
		public bool femminile;
		public GameManager.Squadriglia name;
		public string capo, vice, terzino, novizio1, novizio2;
		public int materials;
		public int points;
	}
