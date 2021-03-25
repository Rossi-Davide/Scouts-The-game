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
	[System.NonSerialized]
	public ConcreteSquadriglia[] squadriglieInGioco;
	public AngoloDiAltraSquadriglia[] angoli;
	public PlayerBuildingBase[] playerBuildingPrefabs;
	public SpriteRenderer[] otherSqBuildingsPrefabs;

	public posizioneOggettiAngoli[] nAngoli;

	public GameObject[] squadriglieriAIMalePrefabs;
	public GameObject[] squadriglieriAIFemalePrefabs;

	readonly string[] nomiF = { "Sara", "Sofia", "Beatrice", "Federica", "Caterina", "Emma", "Vera", "Lucia", "Martina", "Matilde", "Letizia", "Giada", "Chiara", "Annalisa", "Francesca", "Victoria", "Giulia", "Ginevra", "Viola", "Greta", "Aurora", "Simona", "Monica", "Bianca", "Miriam", "Gloria", "Greta", "Ilaria", "Bianca", "Anita", "Gilda" };
	readonly string[] nomiM = { "Simone", "Lorenzo", "Nicola", "Francesco", "Michele", "Giovanni", "Fabio", "Nicolò", "Davide", "Matteo", "Tommaso", "Samuele", "Raffaele", "Giulio", "Pietro", "Luca", "Andrea", "Giacomo", "Gianluca", "Riccardo", "Filippo", "Lukas", "Gabriele" };
	readonly string[] cognomi = { "Rossi", "Ferrari", "Russo", "Bianchi", "Romano", "Gallo", "Costa", "Fontana", "Conti", "Esposito", "Ricci", "Bruno", "Rizzo", "Moretti", "De Luca", "Marino", "Greco", "Barbieri", "Lombardi", "Giordano", "Rinaldi", "Colombo", "Mancini", "Longo", "Leone", "Martinelli", "Marchetti", "Martini", "Galli", "Gatti", "Mariani", "Ferrara", "Santoro", "Marini", "Bianco", "Conte", "Serra", "Farina", "Gentile", "Caruso", "Morelli", "Ferri", "Testa", "Ferraro", "Pellegrini", "Grassi", "Rossetti", "D'Angelo", "Bernardi", "Mazza", "Rizzi", "Silvestri", "Vitale", "Franco", "Parisi", "Martino", "Valentini", "Castelli", "Bellini", "Monti", "Lombardo", "Fiore", "Grasso", "Ferro", "Carbone", "Orlando", "Guerra", "Palmieri", "Milani", "Villa", "Viola", "Ruggeri", "De Santis", "D'Amico", "Negri", "Battaglia", "Sala", "Palumbo", "Benedetti", "Olivieri", "Giuliani", "Rosa", "Amato", "Molinari", "Alberti", "Barone", "Pellegrino", "Piazza", "Moro", "Vitali", "Spinelli", "Sartori", "Fabbri", "Vaccari", "Massari", "Medici", "Sarti", "Venturi", "Montanari", "Cappelli" };

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
	public int GetSquadrigliaMaterials(int n)
	{
		foreach (ConcreteSquadriglia sq in squadriglieInGioco)
		{
			if (sq.baseSq.num == n)
			{
				return sq.materials;
			}
		}
		throw new System.Exception("La squadriglia ricercata non esiste");
	}
	public int GetSquadrigliaPoints(int n)
	{
		foreach (ConcreteSquadriglia sq in squadriglieInGioco)
		{
			if (sq.baseSq.num == n)
			{
				return sq.points;
			}
		}
		throw new System.Exception("La squadriglia ricercata non esiste");
	}
	#endregion

	private void Start()
	{
		GameManager.instance.OnCounterValueChange += RefreshPlayerCounters;
		InvokeRepeating(nameof(ChangeOtherSqCounters), 20, 20);
		InvokeRepeating(nameof(OtherSQBuildBuildings), 30, 30);
		var campManager = CampManager.instance;
		InitializeSquadriglias(campManager.camp.settings.gender, campManager.possibleFemaleSqs, campManager.possibleMaleSqs, campManager.camp.settings.femaleSqs, campManager.camp.settings.maleSqs, campManager.camp.settings.playerSqIndex);
		SetStatus(SaveSystem.instance.LoadData<Status>(SaveSystem.instance.squadrigliaManagerFileName, false));
		InstantiateStuff();
	}

	public Status SendStatus()
	{
		var s = new ConcreteSquadriglia.Status[squadriglieInGioco.Length];
		for (int i = 0; i < squadriglieInGioco.Length; i++)
			s[i] = squadriglieInGioco[i].SendStatus();
		return new Status
		{
			squadriglieInGioco = s
		};
	}
	void SetStatus(Status status)
	{
		if (status != null)
		{
			for (int i = 0; i < squadriglieInGioco.Length; i++)
			{
				squadriglieInGioco[i].SetStatus(status.squadriglieInGioco[i]);
			}
		}
	}
	public class Status
	{
		public ConcreteSquadriglia.Status[] squadriglieInGioco;
	}
	void InstantiateStuff()
	{
		AIsManager.instance.allSquadriglieri = new Squadrigliere[squadriglieInGioco.Length * 5 - 1];
		bool hasDestroyed = false;
		for (int s = 0; s < squadriglieInGioco.Length; s++)
		{
			var sq = squadriglieInGioco[s];
			for (int p = 0; p < sq.ruoli.Length; p++)
			{
				if (sq.baseSq.femminile) { Instantiate(squadriglieriAIFemalePrefabs[sq.AIPrefabTypes[p]], sq.angolo.position, Quaternion.identity, AIcontainers[s].transform); }
				else { Instantiate(squadriglieriAIMalePrefabs[sq.AIPrefabTypes[p]], sq.angolo.position, Quaternion.identity, AIcontainers[s].transform); }
			}

			var squadriglieri = AIcontainers[s].GetComponentsInChildren<Squadrigliere>(true);
			AIcontainers[s].transform.position = sq.angolo.transform.position;

			for (int p = 0; p < squadriglieri.Length; p++)
			{
				squadriglieri[p].objectName = sq.nomi[p];
				squadriglieri[p].objectSubName = sq.ruoli[p] + " " + sq.baseSq.name;
				squadriglieri[p].sq = sq.baseSq;
				squadriglieri[p].SetMissingPriorityTarget("Tenda", sq.tenda.position);

				if (sq.baseSq == Player.instance.squadriglia && p == 0)
				{
					Destroy(sq.angolo.GetComponent<AngoloDiAltraSquadriglia>().clickListener);
					Destroy(sq.angolo.GetComponent<AngoloDiAltraSquadriglia>());
					Destroy(squadriglieri[p].gameObject);
					hasDestroyed = true;
				}
				else
				{
					AIsManager.instance.allSquadriglieri[p + s * sq.ruoli.Length - (hasDestroyed ? 1 : 0)] = squadriglieri[p];
				}
			}
		}
		foreach (var e in AIsManager.instance.events)
			e.ResetEditableInfo();
		AIsManager.instance.SetStatus(SaveSystem.instance.LoadData<AIsManager.Status>(SaveSystem.instance.aisManagerFileName, false));
	}

	public void InitializeSquadriglias(Gender gender, Squadriglia[] possibleFemaleSqs, Squadriglia[] possibleMaleSqs, int[] femaleSqs, int[] maleSqs, int playerSqIndex)
	{
		squadriglieInGioco = new ConcreteSquadriglia[6];
		Player.instance.squadriglia = gender == Gender.Femmina ? possibleFemaleSqs[femaleSqs[playerSqIndex]] : possibleMaleSqs[maleSqs[playerSqIndex]];
		int currentNum = 1;

		for (int s = 0; s < squadriglieInGioco.Length; s++)
		{
			squadriglieInGioco[s] = new ConcreteSquadriglia();
			var sq = squadriglieInGioco[s];
			sq.nomi = new string[5];
			sq.angolo = angoli[s].transform;
			if (s < femaleSqs.Length)
				sq.baseSq = possibleFemaleSqs[femaleSqs[s]];
			else
				sq.baseSq = possibleMaleSqs[maleSqs[s - femaleSqs.Length]];

			for (int p = 0; p < sq.nomi.Length; p++)
			{
				sq.nomi[p] = sq.baseSq.femminile ? nomiF[Random.Range(0, nomiF.Length)] : nomiM[Random.Range(0, nomiM.Length)];
				sq.nomi[p] += $" {cognomi[Random.Range(0, cognomi.Length)]}";
			}

			if (sq.baseSq == Player.instance.squadriglia)
			{
				sq.nomi[0] = CampManager.instance.camp.settings.playerName + " (Tu)";
				sq.baseSq.num = squadriglieInGioco.Length;
				playerAngoloPos.position = sq.angolo.transform.position;
			}
			else
			{
				sq.baseSq.num = currentNum;
				currentNum++;
				sq.angolo.GetComponent<AngoloDiAltraSquadriglia>().objectName = "Angolo " + sq.baseSq.name;
				sq.angolo.GetComponent<AngoloDiAltraSquadriglia>().squadriglia = sq.baseSq;
			}

			sq.buildings = new SpriteRenderer[otherSqBuildingsPrefabs.Length];
			for (int b = 0; b < sq.buildings.Length; b++)
			{
				sq.buildings[b] = Instantiate(sq.baseSq == Player.instance.squadriglia ? playerBuildingPrefabs[b].GetComponent<SpriteRenderer>() : otherSqBuildingsPrefabs[b], nAngoli[s].oggetti[b].position, Quaternion.identity, sq.angolo).GetComponent<SpriteRenderer>();
			}
			foreach (var sp in sq.angolo.GetComponentsInChildren<SpriteRenderer>(true))
			{
				if (sp.gameObject.name == "Tenda(Clone)")
				{
					sq.tenda = sp.transform;
				}
			}

			sq.ruoli = new Ruolo[5] { Ruolo.Capo, Ruolo.Vice, Ruolo.Terzino, Ruolo.Novizio, Ruolo.Novizio };

			sq.AIPrefabTypes = new int[sq.ruoli.Length];
			for (int p = 0; p < sq.AIPrefabTypes.Length; p++)
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
		for (int i = 0; i < squadriglieInGioco.Length; i++)
		{
			Transform[] costruzioni = nAngoli[i].oggetti;
			var sq = squadriglieInGioco[i];
			if (sq.baseSq != Player.instance.squadriglia)
			{
				if (Random.Range(0, 100) >= 50)
				{
					int b = Random.Range(0, sq.buildings.Length);
					sq.buildings[b].gameObject.SetActive(true);
					//sq.buildings[b].transform.position = costruzioni[b].position;
					Debug.Log($"Built building {sq.buildings[b].name} for sq {sq.baseSq.name}.");
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