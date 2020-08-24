using System.Collections;
using UnityEngine;

public class Buttons : MonoBehaviour
{
	public void ClickAnimation(Animator a)
	{
		StartCoroutine(OnClick(a));
	}
	IEnumerator OnClick(Animator a)
	{
		a.Play("Click");
		Debug.Log("hjhj");
		yield return new WaitForSeconds(0.4f);
		
	}
}
