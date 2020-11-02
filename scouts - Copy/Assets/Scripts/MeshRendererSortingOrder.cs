using UnityEngine;

public class MeshRendererSortingOrder : MonoBehaviour
{
    int orderInLayer = 11;
    MeshRenderer meshRenderer;
	void Start()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.sortingOrder = orderInLayer;
	}
}
