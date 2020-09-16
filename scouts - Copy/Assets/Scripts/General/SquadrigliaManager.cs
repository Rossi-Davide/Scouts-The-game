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
		DontDestroyOnLoad(this);
		instance = this;
		squadriglieInGioco = new ConcreteSquadriglia[6];
		for (int s = 0; s < squadriglieInGioco.Length; s++)
			squadriglieInGioco[s] = new ConcreteSquadriglia();
	}

	#endregion


	public Transform playerAngoloPos;
	[HideInInspector]
	public ConcreteSquadriglia[] squadriglieInGioco;
	public Transform[] tents;
	public AngoloDiAltraSquadriglia[] angoli;
	string[] nomiF = { "Sara", "Sofia", "Beatrice", "Federica", "Caterina", "Emma", "Vera", "Lucia", "Martina", "Matilde", "Letizia", "Giada", "Chiara", "Annalisa", "Francesca", "Victoria", "Giulia", "Ginevra", "Viola", "Greta", "Aurora", "Simona", "Monica", "Bianca", "Miriam", "Gloria", "Greta", "Ilaria", "Bianca", "Anita", "Gilda" };
	string[] nomiM = { "Simone", "Lorenzo", "Nicola", "Francesco", "Michele", "Giovanni", "Fabio", "Nicolò", "Davide", "Matteo", "Tommaso", "Samuele", "Raffaele", "Giulio", "Pietro", "Luca", "Andrea", "Giacomo", "Gianluca", "Riccardo", "Filippo", "Lukas", "Gabriele" };
	string[] cognomi = { "Rossi", "Ferrari", "Russo", "Bianchi", "Romano", "Gallo", "Costa", "Fontana", "Conti", "Esposito", "Ricci", "Bruno", "Rizzo", "Moretti", "De Luca", "Marino", "Greco", "Barbieri", "Lombardi", "Giordano", "Rinaldi", "Colombo", "Mancini", "Longo", "Leone", "Martinelli", "Marchetti", "Martini", "Galli", "Gatti", "Mariani", "Ferrara", "Santoro", "Marini", "Bianco", "Conte", "Serra", "Farina", "Gentile", "Caruso", "Morelli", "Ferri", "Testa", "Ferraro", "Pellegrini", "Grassi", "Rossetti", "D'Angelo", "Bernardi", "Mazza", "Rizzi", "Silvestri", "Vitale", "Franco", "Parisi", "Martino", "Valentini", "Castelli", "Bellini", "Monti", "Lombardo", "Fiore", "Grasso", "Ferro", "Carbone", "Orlando", "Guerra", "Palmieri", "Milani", "Villa", "Viola", "Ruggeri", "De Santis", "D'Amico", "Negri", "Battaglia", "Sala", "Palumbo", "Benedetti", "Olivieri", "Giuliani", "Rosa", "Amato", "Molinari", "Alberti", "Barone", "Pellegrino", "Piazza", "Moro", "Vitali", "Spinelli", "Sartori", "Fabbri", "Vaccari", "Massari", "Medici", "Sarti", "Venturi", "Montanari", "Cappelli" };

	public GameObject[] AIcontainers;


	#region Metodi per dare informazioni sulle squadriglie
	public string GetSquadrigliaName(int n)
	{
		foreach (Squadriglia sq in squadriglieInGioco)
		{
			if (sq.num == n)
			{
				if (Player.instance.squadriglia == sq)
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
		foreach (ConcreteSquadriglia sq in squadriglieInGioco)
		{
			if (sq.num == n)
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
			if (sq.num == n)
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
		foreach (ConcreteSquadriglia sq in squadriglieInGioco)
		{
			for (int p = 0; p < sq.nomi.Length; p++)
			{
				if (sq.femminile)
				{
					sq.nomi[p] = nomiF[Random.Range(0, nomiF.Length)];
				}
				else
				{
					sq.nomi[p] = nomiM[Random.Range(0, nomiM.Length)];
				}
				sq.nomi[p] += $" {cognomi[Random.Range(0, cognomi.Length)]}";
			}
			if (Player.instance.squadriglia == sq)
			{
				sq.nomi[0] = Player.instance.playerName + " (Tu)";
			}
		}
		AssegnazioneAI();
	}



	#endregion

	#region Counters and buildings
	private void Start()
	{
		GameManager.instance.OnMaterialsChange += RefreshPlayerMaterials;
		GameManager.instance.OnPointsChange += RefreshPlayerPoints;
		InvokeRepeating("ChangeOtherSqCounters", 30, Random.Range(10, 30));
		InvokeRepeating("OtherSQBuildBuildings", 30, Random.Range(30, 60));
		CampManager.instance.InitializeSquadrigliaManager();
	}
	private void OnDestroy()
	{
		GameManager.instance.OnMaterialsChange -= RefreshPlayerMaterials;
		GameManager.instance.OnPointsChange -= RefreshPlayerPoints;
	}
	void RefreshPlayerMaterials(int newValue)
	{
		foreach (ConcreteSquadriglia sq in squadriglieInGioco)
		{
			if (sq == Player.instance.squadriglia)
			{
				sq.materials = newValue;
			}
		}
	}
	void RefreshPlayerPoints(int newValue)
	{
		foreach (ConcreteSquadriglia sq in squadriglieInGioco)
		{
			if (sq == Player.instance.squadriglia)
			{
				sq.points = newValue;
			}
		}
	}

	void ChangeOtherSqCounters()
	{
		foreach (ConcreteSquadriglia sq in squadriglieInGioco)
		{
			if (sq != Player.instance.squadriglia)
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
			if (sq != Player.instance.squadriglia)
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
			if (!(sq == Player.instance.squadriglia))
			{
				sq.num = currentNum;
				currentNum++;
			}
			else
			{
				sq.num = squadriglieInGioco.Length;
			}
		}
	}
	void AssegnazioneAI()
	{
		for (int i = 0; i < squadriglieInGioco.Length; i++)
		{
			var sq = squadriglieInGioco[i];
			AIcontainers[i].transform.Find("Female").gameObject.SetActive(sq.femminile);
			AIcontainers[i].transform.Find("Male").gameObject.SetActive(!sq.femminile);
			var squadriglieri = AIcontainers[i].GetComponentsInChildren<Squadrigliere>();
			AIcontainers[i].transform.position = sq.angolo.transform.position;
			for (int p = 0; p < squadriglieri.Length; p++)
			{
				if (sq == Player.instance.squadriglia)
				{
					if (p == 0)
					{
						squadriglieri[p].gameObject.SetActive(false);
					}
					p++;
				}
				squadriglieri[p].name = sq.nomi[p];
				squadriglieri[p].nomeRuolo = sq.ruoli[p];
				squadriglieri[p].sqText.text = sq.name;
				squadriglieri[p].ruolo.text = squadriglieri[p].nomeRuolo.ToString();
				squadriglieri[p].sq = sq;
				squadriglieri[p].tent = sq.tenda;
			}
		}
	}


	void AssegnazioneSquadriglieInGioco()
	{
		for (int s = 0; s < squadriglieInGioco.Length; s++)
		{
			var sq = squadriglieInGioco[s];
			if (sq != Player.instance.squadriglia)
			{
				sq.angolo = angoli[s];
				sq.angolo.name = "Angolo " + sq.name;
				sq.angolo.squadriglia = sq;
			}
			else
			{
				sq.angolo.enabled = false;
				playerAngoloPos.position = sq.angolo.transform.position;
			}
			sq.buildings = sq.angolo.GetComponentsInChildren<SpriteRenderer>();
			foreach (var b in sq.buildings)
				b.GetComponent<BuildingsActionsAbstract>().enabled = sq == Player.instance.squadriglia;
			sq.tenda = tents[s];
			sq.nomi = new string[5] { "", "", "", "", ""};
			sq.ruoli = new GameManager.Ruolo[5] { GameManager.Ruolo.Capo, GameManager.Ruolo.Vice, GameManager.Ruolo.Terzino, GameManager.Ruolo.Novizio, GameManager.Ruolo.Novizio };
		}
		GetRandomNames();
		SetSqNums();
	}



	public void InitializeSquadrigliaManager(Gender gender, Squadriglia[] possibleFemaleSqs, Squadriglia[] possibleMaleSqs, int[] femaleSqs, int[] maleSqs, int playerSqIndex)
	{
		Player.instance.squadriglia = gender == Gender.Femmina ? possibleFemaleSqs[femaleSqs[playerSqIndex]] : possibleMaleSqs[maleSqs[playerSqIndex]];
		for (int s = 0; s < femaleSqs.Length; s++)
		{
			squadriglieInGioco[s].name = possibleFemaleSqs[femaleSqs[s]].name;
			squadriglieInGioco[s].femminile = possibleFemaleSqs[femaleSqs[s]].femminile;
		}
		for (int s = femaleSqs.Length - 1; s < femaleSqs.Length + femaleSqs.Length; s++)
		{
			int index = femaleSqs.Length - 1;
			instance.squadriglieInGioco[s].name = possibleMaleSqs[maleSqs[index]].name;
			instance.squadriglieInGioco[s].femminile = possibleMaleSqs[maleSqs[index]].femminile;
		}
		AssegnazioneSquadriglieInGioco();
	}
	#endregion
}