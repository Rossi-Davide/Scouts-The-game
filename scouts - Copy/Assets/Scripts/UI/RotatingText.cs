using UnityEngine;
using UnityEngine.UIElements;

public class RotatingText : MonoBehaviour
{
	float distance;
	public float rotateConstant;
	private void Update()
	{
		distance = transform.position.x - Player.instance.transform.position.x;
		transform.rotation = Quaternion.Euler(0, rotateConstant * distance, 0);
		if (transform.eulerAngles.y >= 270 || transform.eulerAngles.y <= 90)
		{
			GetComponent<MeshRenderer>().enabled = true;
		}
		else
		{
			GetComponent<MeshRenderer>().enabled = false;
		}
	}
}
