using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Joystick : MonoBehaviour
{
	GameObject circle;
	const float maxCircleRadius = 50, maxTouchRadius = 80; 
	[HideInInspector]
	public Vector2 direction;
	[HideInInspector]
	public bool isUsingJoystick;

	#region Singleton
	public static Joystick instance;
	private void Awake()
	{
		circle = transform.Find("Circle").gameObject;
		if (instance != null)
		{
			throw new System.Exception("Joystick non è un singleton");
		}
		instance = this;
	}
	#endregion
	private void Update()
	{
		if (Input.touchCount >= 1)
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began)
			{
				isUsingJoystick = (t.position - (Vector2)circle.transform.position).magnitude < maxTouchRadius;
			}
			if (t.phase == TouchPhase.Moved && isUsingJoystick)
			{
				circle.transform.position = t.position;
				var relativePos = circle.GetComponent<RectTransform>().anchoredPosition;
				var m = relativePos.magnitude;
				if (m > maxCircleRadius)
				{
					var scale = maxCircleRadius / m;
					relativePos *= scale;
					circle.GetComponent<RectTransform>().anchoredPosition = relativePos;
				}
				direction = relativePos / maxCircleRadius;
			}
			else if (t.phase == TouchPhase.Ended)
			{
				circle.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				isUsingJoystick = false;
				direction = Vector2.zero;
			}
		}
		else
			isUsingJoystick = false;
	}
}
