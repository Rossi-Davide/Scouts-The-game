using System.Collections;
using UnityEngine;
using Pathfinding;


public class GenericAI : MonoBehaviour
{
	public Vector3[] randomTarget;
	public float speed;
	public float minWayPointDistance;
	protected Vector3 target;
	protected Path currentPath;
	protected int currentWayPointIndex;
	protected Seeker seeker;
	protected Rigidbody2D rb;
	protected Animator animator;

	protected virtual void Start()
	{
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		CreateNewPath();

	}
	protected virtual void CreateNewPath()
	{
		target = randomTarget[Random.Range(0, randomTarget.Length)];
		seeker.StartPath(rb.position, target, OnPathCreated);
	}
	protected void OnPathCreated(Path p)
	{
		if (!p.error)
		{
			currentPath = p;
			currentWayPointIndex = 0;
		}
		else
		{
			CreateNewPath();
		}
	}
	protected void ChangeAnimation()
	{
		float xMovement = rb.velocity.normalized.x, yMovement = rb.velocity.normalized.y;
		animator.SetFloat("Speed", (xMovement > 0.1 || yMovement > 0.1) ? 1 : 0);
		animator.SetFloat("XMovement", xMovement);
		animator.SetFloat("YMovement", yMovement);
	}
	protected virtual void OnPathCompleted()
	{
		CreateNewPath();
	}
	protected virtual void Update()
	{
		ChangeAnimation();
		if (currentPath == null)
			return;
		var nextWayPoint = currentPath.vectorPath[currentWayPointIndex];
		var d = Vector2.Distance(rb.position, nextWayPoint);
		if (d < minWayPointDistance)
		{
			if (currentWayPointIndex == currentPath.vectorPath.Count)
			{
				//Path completata
				currentPath = null;
				currentWayPointIndex = 0;
				OnPathCompleted();
				return;
			}
			else
			{
				currentWayPointIndex++;
				nextWayPoint = currentPath.vectorPath[currentWayPointIndex];
			}
		}

		var nextMovement = ((Vector2)nextWayPoint - rb.position).normalized;
		rb.AddForce(nextMovement * speed * Time.deltaTime);
	}
}
