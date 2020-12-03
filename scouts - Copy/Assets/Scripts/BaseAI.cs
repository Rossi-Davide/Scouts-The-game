using UnityEngine;
using Pathfinding;
using System.Collections;

public abstract class BaseAI : InGameObject
{
	protected Vector3[] randomTarget = {
		new Vector3(-1, -1, 0),
		new Vector3(5, 5, 0),
	}; //set all the different targets
	protected float speed = 50;

	protected float minWayPointDistance = 4;
	protected Seeker seeker;
	protected Rigidbody2D rb;

	//current stuff
	Vector3 currentTarget;
	protected Path currentPath;
	protected int currentWayPointIndex;

	public PriorityTarget[] priorityTargets;

	bool disable, stayUntil;
	int keepTarget;

	protected override void Start()
	{
		base.Start();
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		animator.SetBool("move", true);

		CreateNewPath(null);

		InvokeRepeating(nameof(CheckPriorityPathConditions), 1f, 1f);
	}

	public virtual void SetMissingPriorityTarget(string targetName, Vector3 pos) { }

	protected virtual void CreateNewPath(Vector3? priorityTarget)
	{
		currentTarget = priorityTarget != null ? priorityTarget.Value : randomTarget[Random.Range(0, randomTarget.Length)];
		seeker.StartPath(rb.position, currentTarget, VerifyPath);
		Debug.Log("I'm going to " + currentTarget);
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
		animator.SetFloat("XMovement", xMovement);
		animator.SetFloat("YMovement", yMovement);
	}
	protected void PathCompleted()
	{
		if (keepTarget > 0)
		{
			StartCoroutine(GameManager.Wait(keepTarget, Unlock));
			animator.SetBool("move", false);
		}
		if (disable)
		{
			gameObject.SetActive(false);
			animator.SetBool("move", false);
			ToggleUI(false);
		}
		if (stayUntil)
		{
			animator.SetBool("move", false);
		}
		else
			CheckPriorityTargetsThatWait();
	}
	
	public void Unlock() //call method from another script if stayUntil is true
	{
		gameObject.SetActive(true);
		ToggleUI(true);
		keepTarget = 0;
		stayUntil = false;
		disable = false;
		animator.SetBool("move", true);
		CheckPriorityTargetsThatWait();
	}

	void CheckPriorityTargetsThatWait()
	{
		var max = -1;
		Vector3? target = null;
		foreach (var p in priorityTargets)
		{
			if (p.waitEndOfCurrentPath && p.automatic && p.priority > max)
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

	protected void FixedUpdate()
	{
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
		rb.velocity = nextMovement * speed * Time.deltaTime;
		ChangeAnimation();
	}

	void CheckPriorityPathConditions()
	{
		var max = -1;
		Vector3? target = null;
		foreach (var p in priorityTargets)
		{
			if (!p.waitEndOfCurrentPath && p.automatic && p.priority > max)
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

	public void ForceTarget(Vector3 target, int stay, bool setInactive)
	{
		CreateNewPath(target);
		keepTarget = stay;
		disable = setInactive;
	}
	public void ForceTarget(Vector3 target, bool stayUntil, bool setInactive)
	{
		CreateNewPath(target);
		this.stayUntil = stayUntil;
		disable = setInactive;
	}
	public void ForceTarget(string priorityTargetName, int stay, bool setInactive)
	{
		foreach (var p in priorityTargets)
		{
			if (p.name == priorityTargetName)
			{
				CreateNewPath(p.target);
				keepTarget = stay;
				disable = setInactive;
			}
		}
	}
	public void ForceTarget(string priorityTargetName, bool stayUntil, bool setInactive)
	{
		foreach (var p in priorityTargets)
		{
			if (p.name == priorityTargetName)
			{
				CreateNewPath(p.target);
				this.stayUntil = stayUntil;
				disable = setInactive;
			}
		}
	}
}
