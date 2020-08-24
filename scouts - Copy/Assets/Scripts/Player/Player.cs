using UnityEngine;
using Pathfinding;

public class Player : MonoBehaviour
{
    #region Singleton 
    public static Player instance;
	private void Awake()
	{
        if (instance != null)
		{
            throw new System.Exception("Player is a singleton but it has been created more than once");
		}
        instance = this;
	}
    #endregion
    public string playerName;
	private void Start()
    {
        animator = GetComponent<Animator>();
    }
	#region Movement
	public float playerSpeed; 
    float lastX, lastY;
    Animator animator;
	bool isMoving;
	void FixedUpdate()
	{
		Vector3 movement = Joystick.instance.direction; 
		transform.position = Vector3.Lerp(transform.position, transform.position + movement, Time.deltaTime * playerSpeed);
		if (isMoving)
		{
			transform.Find("WaterParticle").GetComponent<ParticleSystem>().Play();
		}

		animator.SetFloat("Speed", movement.sqrMagnitude);
		if (movement.sqrMagnitude >= 0.01) 
		{
			isMoving = true;
			animator.SetFloat("XMovement", movement.x);
			animator.SetFloat("YMovement", movement.y);
			lastX = movement.x;
			lastY = movement.y;
		}
		else
		{
			isMoving = false;
			animator.SetFloat("XMovement", lastX);
			animator.SetFloat("YMovement", lastY);
		}




	}
	Vector3 tPos;
	Touch t;



	#endregion
	#region General
	public GameManager.Squadriglia squadriglia;
	#endregion
}
