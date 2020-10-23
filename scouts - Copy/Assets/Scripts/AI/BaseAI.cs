using UnityEngine;
using Pathfinding;

public abstract class BaseAI : InGameObject
{
	public Vector3[] randomTarget;
	public float speed;
	public float minWayPointDistance;
	protected Path currentPath;
	protected int currentWayPointIndex;
	protected Seeker seeker;
	protected Rigidbody2D rb;
	Vector3 currentTarget;

	public PriorityTarget[] priorityTargets;

	bool extremePriority;

	protected override void Start()
	{
		base.Start();
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		CreateNewPath(null);

		InvokeRepeating(nameof(CheckPriorityPathConditions), 1f, 1f);

	}

	public virtual void SetMissingPriorityTarget(string targetName, Vector3 pos) { }



	protected virtual void CreateNewPath(Vector3? priorityTarget)
	{
		if (priorityTarget == null)
		{
			currentTarget = randomTarget[Random.Range(0, randomTarget.Length)];
			seeker.StartPath(rb.position, currentTarget, VerifyPath);
		}
		else if (!extremePriority)
		{
			seeker.StartPath(rb.position, priorityTarget.Value, VerifyPath);
			currentTarget = priorityTarget.Value;
		}
	}
	protected void VerifyPath(Path p)
	{
		if (!p.error)
		{
			currentPath = p;
			currentWayPointIndex = 0;
		}
		else
		{
			CreateNewPath(null);
		}
	}
	protected void ChangeAnimation()
	{
		float xMovement = rb.velocity.x, yMovement = rb.velocity.y;
		animator.SetFloat("Speed", (xMovement > 0.01 || yMovement > 0.01) ? 1 : 0);
		animator.SetFloat("XMovement", xMovement);
		animator.SetFloat("YMovement", yMovement);
	}
	protected virtual void PathCompleted()
	{
		var max = -1;
		Vector3? target = null;
		extremePriority = false;
		foreach (var p in priorityTargets)
		{
			if (p.waitEndOfCurrentPath && p.priority > max)
			{
				if (FindNotVerified(p.conditions) == null)
				{
					max = p.priority;
					target = p.target;
				}
			}
		}
		CreateNewPath(target);
	}

	protected override void Update()
	{
		base.Update();
		ChangeAnimation();
		if (currentPath == null)
			return;
		var nextWayPoint = currentPath.vectorPath[currentWayPointIndex];
		if (Vector2.Distance(nextWayPoint, rb.position) < minWayPointDistance)
		{
			if (currentWayPointIndex == currentPath.vectorPath.Count - 1)
			{
				//Path completata
				currentPath = null;
				currentWayPointIndex = 0;
				PathCompleted();
				return;
			}
			currentWayPointIndex++;
			nextWayPoint = currentPath.vectorPath[currentWayPointIndex];
		}
		var nextMovement = ((Vector2)nextWayPoint - rb.position).normalized;
		rb.AddForce(nextMovement * speed * Time.deltaTime);
	}

	void CheckPriorityPathConditions()
	{
		var max = -1;
		Vector3? target = null;
		foreach (var p in priorityTargets)
		{
			if (!p.waitEndOfCurrentPath && p.priority > max)
			{
				if (FindNotVerified(p.conditions) == null)
				{
					max = p.priority;
					target = p.target;
				}
			}
		}
		if (target != currentTarget)
			CreateNewPath(target);
	}

	public void ForceTarget(Vector3 target)
	{
		CreateNewPath(target);
		extremePriority = true;
	}
	public void ForceTarget(string priorityTargetName)
	{
		foreach (var p in priorityTargets)
		{
			if (p.name == priorityTargetName)
			{
				CreateNewPath(p.target);
				extremePriority = true;
			}
		}
	}
}


[CreateAssetMenu(fileName = "New Priority Target", menuName = "PriorityAITarget")]
public class PriorityTarget : ScriptableObject
{
	public new string name;
	public Vector3 target;
	public int priority;
	public bool waitEndOfCurrentPath;
	public Condition[] conditions;
}
