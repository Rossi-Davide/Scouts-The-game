using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform pl;
	public GameObject goToTargetButton;
	public Vector3 camOffset;
	public float camSpeed, zoomSpeed, roundSize;
	float standardZoom = 3.6f, zoomDelta = 0;

	public Transform target;
	bool isFollowing;
	private void Awake()
	{
		isFollowing = true;
		pl = Player.instance.transform;
		standardZoom = 3.6f;
	}
	private void Start()
	{
		SetTarget(pl, 0);
	}
	void FixedUpdate()
    {
		if (isFollowing)
			GoToTarget();
    }

	public void GoToTarget()
	{
		transform.position = Vector3.Lerp(transform.position, target.position + camOffset, camSpeed * Time.deltaTime);
		if (Camera.main.orthographicSize > standardZoom + zoomDelta + roundSize)
			Camera.main.orthographicSize -= zoomSpeed * Time.deltaTime;
		else if (Camera.main.orthographicSize < standardZoom + zoomDelta - roundSize)
			Camera.main.orthographicSize += zoomSpeed * Time.deltaTime;
	}
	public void EnableFollow()
	{
        if (!Player.instance.isMoving)
        {
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("whoosh");
		}

		isFollowing = true;
		goToTargetButton.SetActive(!isFollowing);
		GoToTarget();
	}
	public void DisableFollow()
	{
		isFollowing = false;
		goToTargetButton.SetActive(!isFollowing);
	}

	public void SetTarget(Transform t, float zoom)
	{
		target = t;
		zoomDelta = zoom;
	}
}
