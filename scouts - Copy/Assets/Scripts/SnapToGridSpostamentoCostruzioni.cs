using UnityEngine;

public class SnapToGridSpostamentoCostruzioni : MonoBehaviour
{
	float grid = 0.8f;
	[HideInInspector]
	public bool componentEnabled;

	void Update()
	{
		if (componentEnabled)
		{
			float reciprocalGrid = 1f / grid;
			float x = Mathf.Round(transform.position.x * reciprocalGrid) / reciprocalGrid;
			float y = Mathf.Round(transform.position.y * reciprocalGrid) / reciprocalGrid;
			transform.position = new Vector3(x, y, 0);
		}
	}
}
