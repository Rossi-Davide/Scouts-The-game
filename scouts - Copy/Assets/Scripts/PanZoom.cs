using UnityEngine;

public class PanZoom : MonoBehaviour
{
	Vector3 t1Pos, previousCamPos;
	public float zoomSpeed, panSpeed;
	public float minCameraSize, maxCameraSize;
	public float leftLimit, rightLimit, bottomLimit, topLimit;
	public bool panningOrZooming;


	#region Singleton
	public static PanZoom instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("PanZoom non è un singlton");
		}
		instance = this;
	}
	#endregion

	private void Update()
	{
		if (!Joystick.instance.isUsingJoystick)
		{
			//scroll
			if (Input.touchCount >= 1)
			{
				Pan();
				panningOrZooming = true;
			}
			else
			{
				panningOrZooming = false;
			}
			//zoom			
			if (Input.touchCount >= 2)
			{
				Zoom();
			}
		}
		else
		{
			//scroll
			if (Input.touchCount >= 2)
			{
				Pan();
				panningOrZooming = true;
			}
			else
			{
				panningOrZooming = false;
			}
			//zoom			
			if (Input.touchCount >= 3)
			{
				Zoom();
			}
		}
		if (Camera.main.transform.position != previousCamPos)
		{
			Vector3 camPos = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, leftLimit, rightLimit), Mathf.Clamp(Camera.main.transform.position.y, bottomLimit, topLimit), Camera.main.transform.position.z);
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