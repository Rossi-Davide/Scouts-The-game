using UnityEngine;

public class SnapToGridSpostamentoCostruzioni : MonoBehaviour
{
	[HideInInspector]
	public float grid = 0.8f;

	void Update()
	{
		if (grid > 0)
		{
			float reciprocalGrid = 1f / grid;
			float x = Mathf.Round(transform.position.x * reciprocalGrid) / reciprocalGrid;
			float y = Mathf.Round(transform.position.y * reciprocalGrid) / reciprocalGrid;
			transform.position = new Vector3(x, y, 0);
		}
	}
}
