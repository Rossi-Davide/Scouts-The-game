using System.Collections;
using UnityEngine;

public class ClickAnimation : MonoBehaviour
{
	public Vector3 scaleDelta, scaleChange;
	private void OnMouseDown()
	{
		if (!ClickedObjects.instance.ClickedOnUI)
		{
			StartCoroutine(OnClick());
		}
	}

	IEnumerator OnClick()
	{
		while (transform.localScale.magnitude < (transform.localScale + scaleDelta).magnitude)
		{
			transform.localScale += scaleChange;
		}
		yield return new WaitForEndOfFrame();
		while (transform.localScale.magnitude > (transform.localScale - scaleDelta).magnitude)
		{
			transform.localScale -= scaleChange;
		}
	}
}
