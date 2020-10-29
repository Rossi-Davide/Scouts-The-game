using System.Collections;
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
		squadriglieInGioco = new ConcreteSquadriglia[6];
		for (int s = 0; s < squadriglieInGioco.Length; s++)
			squadriglieInGioco[s] = new ConcreteSquadriglia();
	}

	#endregion


	public Transform playerAngoloPos;
	[HideInInspector]
	public ConcreteSquadriglia[] squadriglieInGioco;
	public AngoloDiAltraSquadriglia[] angoli;
	public PlayerBuildingBase[] playerBuildingPrefabs;
	public SpriteRenderer[] otherSqBuildingsPrefabs;

	public GameObject[] squadriglieriAIMalePrefabs;
	public GameObject[] squadriglieriAIFemalePrefabs;

	string[] nomiF = { "Sara", "Sofia", "Beatrice", "Federica", "Caterina", "Emma", "Vera", "Lucia", "Martina", "Matilde", "Letizia", "Giada", "Chiara", "Annalisa", "Francesca", "Victoria", "Giulia", "Ginevra", "Viola", "Greta", "Aurora", "Simona", "Monica", "Bianca", "Miriam", "Gloria", "Greta", "Ilaria", "Bianca", "Anita", "Gilda" };
	string[] nomiM = { "Simone", "Lorenzo", "Nicola", "Francesco", "Michele", "Giovanni", "Fabio", "Nicolò", "Davide", "Matteo", "Tommaso", "Samuele", "Raffaele", "Giulio", "Pietro", "Luca", "Andrea", "Giacomo", "Gianluca", "Riccardo", "Filippo", "Lukas", "Gabriele" };
	string[] cognomi = { "Rossi", "Ferrari", "Russo", "Bianchi", "Romano", "Gallo", "Costa", "Fontana", "Conti", "Esposito", "Ricci", "Bruno", "Rizzo", "Moretti", "De Luca", "Marino", "Greco", "Barbieri", "Lombardi", "Giordano", "Rinaldi", "Colombo", "Mancini", "Longo", "Leone", "Martinelli", "Marchetti", "Martini", "Galli", "Gatti", "Mariani", "Ferrara", "Santoro", "Marini", "Bianco", "Conte", "Serra", "Farina", "Gentile", "Caruso", "Morelli", "Ferri", "Testa", "Ferraro", "Pellegrini", "Grassi", "Rossetti", "D'Angelo", "Bernardi", "Mazza", "Rizzi", "Silvestri", "Vitale", "Franco", "Parisi", "Martino", "Valentini", "Castelli", "Bellini", "Monti", "Lombardo", "Fiore", "Grasso", "Ferro", "Carbone", "Orlando", "Guerra", "Palmieri", "Milani", "Villa", "Viola", "Ruggeri", "De Santis", "D'Amico", "Negri", "Battaglia", "Sala", "Palumbo", "Benedetti", "Olivieri", "Giuliani", "Rosa", "Amato", "Molinari", "Alberti", "Barone", "Pellegrino", "Piazza", "Moro", "Vitali", "Spinelli", "Sartori", "Fabbri", "Vaccari", "Massari", "Medici", "Sarti", "Venturi", "Montanari", "Cappelli" };

	public GameObject[] AIcontainers;


	#region Metodi per dare informazioni sulle squadriglie
	public string GetSquadrigliaName(int n)
	{
		foreach (var sq in squadriglieInGioco)
		{
			if (sq.baseSq.num == n)
			{
				if (Player.instance.squadriglia == sq.baseSq)
				{
					return "Squadriglia " + sq.baseSq.name + " (Tu)";
				}
				else
				{
					return "Squadriglia " + sq.baseSq.name;
				}
			}
		}
		throw new System.Exception("La squadriglia ricercata non esiste" + n);
	}
	public string GetSquadrigliaDescription(int n)
	{
		foreach (ConcreteSquadriglia sq in squadriglieInGioco)
		{
			if (sq.baseSq.num == n)
			{
				return $"{sq.ruoli[0]}: {sq.nomi[0]} \n{sq.ruoli[1]}: {sq.nomi[1]} \n{sq.ruoli[2]}: {sq.nomi[2]} \n{sq.ruoli[3]}: {sq.nomi[3]} \n{sq.ruoli[4]}: {sq.nomi[4]}";
			}
		}
		throw new System.Exception("La squadriglia ricercata non esiste" + n);
	}
	public string GetSquadrigliaMaterials(int n)
	{
		foreach (ConcreteSquadriglia sq in squadriglieInGioco)
		{
			if (sq.baseSq.num == n)
			{
				return sq.materials.ToString();
			}
		}
		throw new System.Exception("La squadriglia ricercata non esiste");
	}
	public string GetSquadrigliaPoints(int n)
	{
		foreach (ConcreteSquadriglia sq in squadriglieInGioco)
		{
			if (sq.baseSq.num == n)
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
		foreach (var sq in squadriglieInGioco)
		{
			for (int p = 0; p < sq.nomi.Length; p++)
			{
				if (sq.baseSq.femminile)
				{
					sq.nomi[p] = nomiF[Random.Range(0, nomiF.Length)];
				}
				else
				{
					sq.nomi[p] = nomiM[Random.Range(0, nomiM.Length)];
				}
				sq.nomi[p] += $" {cognomi[Random.Range(0, cognomi.Length)]}";
			}
			if (Player.instance.squadriglia == sq.baseSq)
			{
				sq.nomi[0] = Player.instance.playerName + " (Tu)";
			}
		}
		SetSqNums();
	}



	#endregion

	#region Counters and buildings
	private void Start()
	{
		GameManager.instance.OnCounterValueChange += RefreshPlayerCounters;
		InvokeRepeating(nameof(ChangeOtherSqCounters), 30, Random.Range(10, 30));
		InvokeRepeating(nameof(OtherSQBuildBuildings), 30, Random.Range(30, 60));
		CampManager.instance.InitializeSquadrigliaManager();
	}

	void RefreshPlayerCounters(Counter c, int newValue)
	{
		if (c == Counter.Materiali)
			GetPlayerSq().materials = newValue;
		else if (c == Counter.Punti)
			GetPlayerSq().points = newValue;
	}

	void ChangeOtherSqCounters()
	{
		foreach (ConcreteSquadriglia sq in squadriglieInGioco)
		{
			if (sq.baseSq != Player.instance.squadriglia)
			{
				if (Random.Range(0, 100) >= 50)
				{
					sq.materials += Random.Range(5, 15);
				}
				if (Random.Range(0, 100) >= 70)
				{
					sq.points += Random.Range(1, 3);
				}
			}
		}
	}

	void OtherSQBuildBuildings()
	{
		foreach (var sq in squadriglieInGioco)
		{
			if (sq.baseSq != Player.instance.squadriglia)
			{
				bool canBuild = true;
				if (Random.Range(0, 100) >= 50)
				{
					foreach (var b in sq.buildings)
					{
						if (!b.gameObject.activeSelf && Random.Range(0, 100) >= 50 && canBuild)
						{
							canBuild = false;
							b.gameObject.SetActive(true);
						}
					}
				}
			}
		}
	}
	#endregion

	#region Assegnazione
	void SetSqNums()
	{
		int currentNum = 1;
		foreach (var sq in squadriglieInGioco)
		{
			if (!(sq.baseSq == Player.instance.squadriglia))
			{
				sq.baseSq.num = currentNum;
				currentNum++;
			}
			else
			{
				sq.baseSq.num = squadriglieInGioco.Length;
			}
		}
		StartCoroutine(AssegnazioneAI());
	}
	IEnumerator AssegnazioneAI()
	{
		yield return new WaitForEndOfFrame();
		AIsManager.instance.allSquadriglieri = new Squadrigliere[squadriglieInGioco.Length * squadriglieInGioco[0].ruoli.Length - 1];
		for (int i = 0; i < squadriglieInGioco.Length; i++)
		{
			var sq = squadriglieInGioco[i];

			for (int p = 0; p < sq.ruoli.Length; p++)
			{
				if (sq.baseSq.femminile)
					Instantiate(squadriglieriAIFemalePrefabs[Random.Range(0, squadriglieriAIFemalePrefabs.Length)], sq.angolo.position, Quaternion.identity, AIcontainers[i].transform);
				else
					Instantiate(squadriglieriAIMalePrefabs[Random.Range(0, squadriglieriAIMalePrefabs.Length)], sq.angolo.position, Quaternion.identity, AIcontainers[i].transform);
			}
			var squadriglieri = AIcontainers[i].GetComponentsInChildren<Squadrigliere>(true);
			AIcontainers[i].transform.position = sq.angolo.transform.position;
			bool hasDestroyed = false;
			for (int p = 0; p < squadriglieri.Length; p++)
			{
				squadriglieri[p].objectSubName = sq.ruoli[p] + " " + sq.baseSq.name;
				squadriglieri[p].sq = sq.baseSq;
				squadriglieri[p].SetMissingPriorityTarget("Tenda", sq.tenda.position);
				squadriglieri[p].objectName = sq.nomi[p];
				if (sq.baseSq == Player.instance.squadriglia && p == 0)
				{
					Destroy(squadriglieri[p]);
					hasDestroyed = true;
				}
				else
				{
					AIsManager.instance.allSquadriglieri[hasDestroyed ? p + (i - 1) * sq.ruoli.Length + sq.ruoli.Length - 1 : p + i * sq.ruoli.Length] = squadriglieri[p];
				}
			}
		}
	}


	IEnumerator AssegnazioneSquadriglieInGioco()
	{
		yield return new WaitForEndOfFrame();
		for (int s = 0; s < squadriglieInGioco.Length; s++)
		{
			var sq = squadriglieInGioco[s];
			sq.angolo = angoli[s].transform;
			if (sq.baseSq == Player.instance.squadriglia)
			{
				playerAngoloPos.position = sq.angolo.transform.position;
				Destroy(sq.angolo.GetComponent<AngoloDiAltraSquadriglia>().clickListener);
				Destroy(sq.angolo.GetComponent<AngoloDiAltraSquadriglia>());
			}
			else
			{
				sq.angolo.GetComponent<AngoloDiAltraSquadriglia>().objectName = "Angolo " + sq.baseSq.name;
				sq.angolo.GetComponent<AngoloDiAltraSquadriglia>().squadriglia = sq.baseSq;
			}


			sq.tenda = sq.angolo.Find("Tent");
			sq.nomi = new string[5];
			sq.ruoli = new GameManager.Ruolo[5] { GameManager.Ruolo.Capo, GameManager.Ruolo.Vice, GameManager.Ruolo.Terzino, GameManager.Ruolo.Novizio, GameManager.Ruolo.Novizio };
		}
		InstantiateBuildings();
		GetRandomNames();
	}


	void InstantiateBuildings()
	{
		foreach (var sq in squadriglieInGioco)
		{
			if (sq.baseSq == Player.instance.squadriglia)
			{
				sq.buildings = new SpriteRenderer[playerBuildingPrefabs.Length];
				for (int b = 0; b < playerBuildingPrefabs.Length; b++)
				{
					var build = Instantiate(playerBuildingPrefabs[b], sq.angolo.position, Quaternion.identity, sq.angolo);
					sq.buildings[b] = build.GetComponent<SpriteRenderer>();
					if (playerBuildingPrefabs[b].gameObject.name == "Tent")
						sq.tenda = build.transform;
				}
			}
			else
			{
				sq.buildings = new SpriteRenderer[otherSqBuildingsPrefabs.Length];
				for (int b = 0; b < otherSqBuildingsPrefabs.Length; b++)
				{
					var build = Instantiate(otherSqBuildingsPrefabs[b], sq.angolo.position, Quaternion.identity, sq.angolo);
					sq.buildings[b] = build.GetComponent<SpriteRenderer>();
					if (otherSqBuildingsPrefabs[b].gameObject.name == "Tent")
						sq.tenda = build.transform;
				}
			}
		}
	}




	public void InitializeSquadrigliaManager(Gender gender, Squadriglia[] possibleFemaleSqs, Squadriglia[] possibleMaleSqs, int[] femaleSqs, int[] maleSqs, int playerSqIndex)
	{
		Player.instance.squadriglia = gender == Gender.Femmina ? possibleFemaleSqs[femaleSqs[playerSqIndex]] : possibleMaleSqs[maleSqs[playerSqIndex]];
		for (int s = 0; s < femaleSqs.Length; s++)
		{
			squadriglieInGioco[s].baseSq = possibleFemaleSqs[femaleSqs[s]];
		}
		for (int s = 0; s < maleSqs.Length; s++)
		{
			instance.squadriglieInGioco[s + femaleSqs.Length].baseSq = possibleMaleSqs[maleSqs[s]];
		}
		StartCoroutine(AssegnazioneSquadriglieInGioco());

	}
	#endregion


	public ConcreteSquadriglia[] GetInfo()
	{
		return squadriglieInGioco;
	}

	public ConcreteSquadriglia GetPlayerSq()
	{
		foreach (var sq in squadriglieInGioco)
		{
			if (sq.baseSq == Player.instance.squadriglia)
				return sq;
		}
		throw new System.Exception("l'array 'squadriglieInGioco' non contiene la squadriglia del player");
	}

}