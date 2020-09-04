using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
	#region Singleton 
	public static Build instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("Build non è un singleton: doppia instance");
		}
		instance = this;
	}
	#endregion

	public enum Objects
	{
		montana,
		cambusa,
		alzabandiera,
		fuocoDaCampo,
		latrina,
		lavaggi,
		tenda, 
		refettorio, 
		stendipanni, 
		portalegna, 
		pianoBidoni, 
		amaca, 
		cassaDiPioneristica, 
		cassaDiInfermieristica, 
		cassaDiCucina, 
		cassaDiTopografia, 
		cassaDiEspressione, 
		cassaDelFurfante,
		tree,
		bush,
	}
}
