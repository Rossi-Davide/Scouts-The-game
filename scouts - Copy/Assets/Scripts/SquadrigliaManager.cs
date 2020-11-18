using System.Collections;
using System.Data.SqlClient;
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


	public Transform playerAngoloPos;
	[HideInInspector] [System.NonSerialized]
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
	SaveSystem saveSystem;


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

	private void Start()
	{
		saveSystem.OnReadyToLoad += ReceiveSavedData;
		GameManager.instance.OnCounterValueChange += RefreshPlayerCounters;
		InvokeRepeating(nameof(ChangeOtherSqCounters), 30, Random.Range(10, 30));
		InvokeRepeating(nameof(OtherSQBuildBuildings), 30, Random.Range(30, 60));
		GameManager.instance.OnCampStart += WhenCampStarts;
		saveSystem = SaveSystem.instance;
		var campManager = CampManager.instance;
		InitializeSquadriglias(campManager.camp.settings.gender, campManager.possibleFemaleSqs, campManager.possibleMaleSqs, campManager.camp.settings.femaleSqs, campManager.camp.settings.maleSqs, campManager.camp.settings.playerSqIndex);
		InstantiateStuff();
	}

	public Status SendStatus()
	{
		var s = new CompactSquadriglia[squadriglieInGioco.Length];
		for (int i = 0; i < squadriglieInGioco.Length; i++)
			s[i] = squadriglieInGioco[i].ToCompactSquadriglia();
		return new Status
		{
			squadriglieInGioco = s
		};
	}
	void SetStatus(Status status)
	{
		if (status != null)
		{
			squadriglieInGioco = new ConcreteSquadriglia[status.squadriglieInGioco.Length];
			for (int i = 0; i < squadriglieInGioco.Length; i++)
			{
				var sq = squadriglieInGioco[i];
				var stSq = status.squadriglieInGioco[i];
				sq.AIPrefabTypes = (stSq.AIPrefabTypes);
				sq.baseSq = (stSq.baseSq);
				sq.materials = (stSq.materials);
				sq.points = (stSq.points);
				sq.ruoli = (stSq.ruoli);
				sq.nomi = (stSq.nomi);
			}
		}
	}
	public class Status
	{
		public CompactSquadriglia[] squadriglieInGioco;
	}

	void ReceiveSavedData(LoadPriority p)
	{
		if (p == LoadPriority.High)
		{
			
		}
	}

	void WhenCampStarts()
	{
		var campManager = CampManager.instance;
		
	}

	void InstantiateStuff()
	{
		AIsManager.instance.allSquadriglieri = new Squadrigliere[squadriglieInGioco.Length * squadriglieInGioco[0].ruoli.Length - 1];
		for (int s = 0; s < squadriglieInGioco.Length; s++)
		{
			var sq = squadriglieInGioco[s];
			for (int p = 0; p < sq.ruoli.Length; p++)
			{
				if (sq.baseSq.femminile) { Instantiate(squadriglieriAIFemalePrefabs[sq.AIPrefabTypes[p]], sq.angolo.position, Quaternion.identity, AIcontainers[s].transform); }
				else { Instantiate(squadriglieriAIMalePrefabs[Random.Range(0, squadriglieriAIMalePrefabs.Length)], sq.angolo.position, Quaternion.identity, AIcontainers[s].transform); }
			}
			var squadriglieri = AIcontainers[s].GetComponentsInChildren<Squadrigliere>(true);
			AIcontainers[s].transform.position = sq.angolo.transform.position;
			bool hasDestroyed = false;
			for (int p = 0; p < squadriglieri.Length; p++)
			{
				squadriglieri[p].objectSubName = sq.ruoli[p] + " " + sq.baseSq.name;
				squadriglieri[p].sq = sq.baseSq;
				squadriglieri[p].SetMissingPriorityTarget("Tenda", sq.tenda.position);
				squadriglieri[p].objectName = sq.nomi[p];
				if (sq.baseSq == Player.instance.squadriglia)
				{
					Destroy(sq.angolo.GetComponent<AngoloDiAltraSquadriglia>().clickListener);
					Destroy(sq.angolo.GetComponent<AngoloDiAltraSquadriglia>());
					if (p == 0)
					{
						Destroy(squadriglieri[p]);
						hasDestroyed = true;
					}
				}
				else
				{
					AIsManager.instance.allSquadriglieri[hasDestroyed ? p + (s - 1) * sq.ruoli.Length + sq.ruoli.Length - 1 : p + s * sq.ruoli.Length] = squadriglieri[p];
				}
			}
		}
	}

	public void InitializeSquadriglias(Gender gender, Squadriglia[] possibleFemaleSqs, Squadriglia[] possibleMaleSqs, int[] femaleSqs, int[] maleSqs, int playerSqIndex)
	{
		squadriglieInGioco = new ConcreteSquadriglia[6];
		/*     Section 1     */
		Player.instance.squadriglia = gender == Gender.Femmina ? possibleFemaleSqs[femaleSqs[playerSqIndex]] : possibleMaleSqs[maleSqs[playerSqIndex]];
		for (int s = 0; s < femaleSqs.Length; s++)
		{
			squadriglieInGioco[s].baseSq = possibleFemaleSqs[femaleSqs[s]];
		}
		for (int s = 0; s < maleSqs.Length; s++)
		{
			instance.squadriglieInGioco[s + femaleSqs.Length].baseSq = possibleMaleSqs[maleSqs[s]];
		}
		/*   Section 2    */
		int currentNum = 1;
		
		for (int s = 0; s < squadriglieInGioco.Length; s++)
		{
			var sq = squadriglieInGioco[s];
			sq = new ConcreteSquadriglia();
			sq.angolo = angoli[s].transform;
			sq.nomi = new string[5];
			for (int p = 0; p < sq.nomi.Length; p++)
			{
				sq.nomi[p] = sq.baseSq.femminile ? nomiF[Random.Range(0, nomiF.Length)] : nomiM[Random.Range(0, nomiM.Length)];
				sq.nomi[p] += $" {cognomi[Random.Range(0, cognomi.Length)]}";
			}
			if (sq.baseSq == Player.instance.squadriglia)
			{
				playerAngoloPos.position = sq.angolo.transform.position;
				sq.nomi[0] = Player.instance.playerName + " (Tu)";
				sq.baseSq.num = squadriglieInGioco.Length;
			}
			else
			{
				sq.angolo.GetComponent<AngoloDiAltraSquadriglia>().objectName = "Angolo " + sq.baseSq.name;
				sq.angolo.GetComponent<AngoloDiAltraSquadriglia>().squadriglia = sq.baseSq;
				sq.buildings = new SpriteRenderer[otherSqBuildingsPrefabs.Length];
				sq.baseSq.num = currentNum;
				currentNum++;
			}
			for (int b = 0; b < otherSqBuildingsPrefabs.Length; b++)
			{
				sq.buildings[b] = Instantiate(sq.baseSq == Player.instance.squadriglia ? playerBuildingPrefabs[b].GetComponent<SpriteRenderer>() : otherSqBuildingsPrefabs[b], sq.angolo.position, Quaternion.identity, sq.angolo).GetComponent<SpriteRenderer>();
			}

			sq.tenda = sq.angolo.Find("Tent");
			sq.ruoli = new Ruolo[5] { Ruolo.Capo, Ruolo.Vice, Ruolo.Terzino, Ruolo.Novizio, Ruolo.Novizio };

			/* Section 3 */
			for (int p = 0; p < sq.ruoli.Length; p++)
			{
				sq.AIPrefabTypes[p] = Random.Range(0, sq.baseSq.femminile ? squadriglieriAIFemalePrefabs.Length : squadriglieriAIMalePrefabs.Length);
			}
		}


	}

	#region Counters and buildings

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