using UnityEngine;

public class PanZoom : MonoBehaviour
{
	Vector3 t1Pos, previousCamPos;
	public float zoomSpeed, panSpeed;
	public float minCameraSize, maxCameraSize;
	float leftLimit, rightLimit, bottomLimit, topLimit, horzExtent, vertExtent;
	public bool panningOrZooming;
	public bool canDo;

	#region Singleton
	public static PanZoom instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("PanZoom non è un singlton");
		}
		canDo = true;
		instance = this;
	}
	#endregion

	private void FixedUpdate()
	{
		if (!Joystick.instance.isUsingJoystick)
		{
			//scroll
			if (Input.touchCount >= 1 && canDo)
			{
				Pan();
				panningOrZooming = true;
			}
			else
			{
				panningOrZooming = false;
			}
			//zoom			
			if (Input.touchCount >= 2 && canDo)
			{
				Zoom();
			}
		}
		else
		{
			//scroll
			if (Input.touchCount >= 2 && canDo)
			{
				Pan();
				panningOrZooming = true;
			}
			else
			{
				panningOrZooming = false;
			}
			//zoom			
			if (Input.touchCount >= 3 && canDo)
			{
				Zoom();
			}
		}
		if (Camera.main.transform.position != previousCamPos)
		{
			vertExtent = Camera.main.orthographicSize;
			horzExtent = vertExtent * Camera.main.aspect;

			leftLimit = horzExtent - 68;
			rightLimit = 85 - horzExtent;
			bottomLimit = vertExtent - 69;
			topLimit = 39 - vertExtent;

			var camX = Camera.main.transform.position.x;
			var camY = Camera.main.transform.position.y;
			Vector3 camPos = new Vector3(Mathf.Clamp(camX, leftLimit, rightLimit), Mathf.Clamp(camY, bottomLimit, topLimit), Camera.main.transform.position.z);
			Camera.main.transform.position = camPos;
		}
		previousCamPos = Camera.main.transform.position;
	}

	void Pan()
	{
		Touch t1 = Input.GetTouch(Input.touchCount - 1);
		if (t1.phase == TouchPhase.Began)
		{
			t1Pos = Camera.main.ScreenToWorldPoint(t1.position);
		}
		if (t1.phase == TouchPhase.Moved)
		{
			FindObjectOfType<FollowPlayer>().DisableFollow();
			Vector3 movement = t1Pos - Camera.main.ScreenToWorldPoint(t1.position);
			Camera.main.transform.Translate(movement * panSpeed * Time.deltaTime);
			t1Pos = Camera.main.ScreenToWorldPoint(t1.position);
			panningOrZooming = true;
		}
		
	}
	void Zoom()
	{
		Touch t1 = Input.GetTouch(Input.touchCount - 1);
		Touch t2 = Input.GetTouch(Input.touchCount - 2);
		if (t1.phase == TouchPhase.Moved && t2.phase == TouchPhase.Moved)
		{
			Vector3 currentDistance = Camera.main.ScreenToWorldPoint(t1.position) - Camera.main.ScreenToWorldPoint(t2.position);
			Vector3 previousDistance = Camera.main.ScreenToWorldPoint(t1.position - t1.deltaPosition) - Camera.main.ScreenToWorldPoint(t2.position - t2.deltaPosition);
			float delta = currentDistance.magnitude - previousDistance.magnitude;
			if (currentDistance.magnitude > previousDistance.magnitude)
			{
				Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize -= delta * zoomSpeed * Time.deltaTime, minCameraSize);
			}
			else
			{
				Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize -= delta * zoomSpeed * Time.deltaTime, maxCameraSize);
			}
			panningOrZooming = true;
			FindObjectOfType<FollowPlayer>().DisableFollow();
		}
	}
}