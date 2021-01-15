using UnityEngine;
using Pathfinding;
using System.Collections;

public abstract class BaseAI : InGameObject
{
	protected float speed = 50;

	protected float minWayPointDistance = 4;
	protected Seeker seeker;
	protected Rigidbody2D rb;

	//current stuff
	protected Vector3 currentTarget;
	protected Path currentPath;
	protected int currentWayPointIndex;
	System.Action currentEndMethod;

	public PriorityTarget[] priorityTargets;

	bool disable, stayUntil;
	int keepTarget;



	protected override void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		seeker = GetComponent<Seeker>();
		base.Start();
		animator.SetBool("move", true);

		CreateNewPath(null);

		InvokeRepeating(nameof(CheckPriorityPathConditions), 1f, 1f);
		InvokeRepeating(nameof(UpdateSlowed), 0.05f, 0.05f);
	}

	public override void Select()
	{
		base.Select();
		StartCoroutine(ForceTarget(Player.instance.transform.position, true, false));
	}
	public override void Deselect()
	{
		base.Deselect();
		StartCoroutine(Unlock());
	}

	public virtual void SetMissingPriorityTarget(string targetName, Vector3 pos) { }

	protected virtual void CreateNewPath(Vector3? priorityTarget)
	{
		int n1, n2;
		n1 = Random.Range(-45, 45);
		n2 = Random.Range(-45, 30);
		//evita target su collinette

		Vector3 a = new Vector3(n1, n2, 0);
		currentTarget = priorityTarget != null ? priorityTarget.Value : a; //aggiorno la posizione dell'IA con un random
		seeker.StartPath(rb.position, currentTarget, VerifyPath);
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
		if (currentEndMethod != null)
		{
			currentEndMethod();
			currentEndMethod = null;
		}
		if (keepTarget > 0)
		{
			StartCoroutine(GameManager.instance.Wait(keepTarget, Unlock()));
			animator.SetBool("move", false);
		}
		if (disable)
		{
			gameObject.SetActive(false);
			animator.SetBool("move", false);
			ToggleClickListener(false);
			ToggleNameAndSubName(false);
		}
		if (stayUntil)
		{
			animator.SetBool("move", false);
		}
		else
			CheckPriorityTargetsThatWait();
	}
	
	public IEnumerator Unlock() //call method from another script if stayUntil is true
	{
		yield return new WaitForEndOfFrame();
		gameObject.SetActive(true);
		keepTarget = 0;
		stayUntil = false;
		disable = false;
		animator = GetComponent<Animator>();
		animator.SetBool("move", true);
		CheckPriorityTargetsThatWait();
		ToggleClickListener(true);
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
		rb = GetComponent<Rigidbody2D>();
		seeker = GetComponent<Seeker>();
		CreateNewPath(target);
	}

	protected void UpdateSlowed()
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
		Vector3 target = currentTarget;
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

	public IEnumerator ForceTarget(Vector3 target, int stay, bool setInactive)
	{
		yield return new WaitForEndOfFrame();
		CreateNewPath(target);
		keepTarget = stay;
		disable = setInactive;
	}
	public IEnumerator ForceTarget(Vector3 target, bool stayUntil, bool setInactive)
	{
		yield return new WaitForEndOfFrame();
		CreateNewPath(target);
		this.stayUntil = stayUntil;
		disable = setInactive;
	}
	public IEnumerator ForceTarget(Vector3 target, bool setInactive, System.Action onEnd)
	{
		yield return new WaitForEndOfFrame();
		CreateNewPath(target);
		disable = setInactive;
		currentEndMethod = onEnd;
	}
	public IEnumerator ForceTarget(string priorityTargetName, int stay, bool setInactive)
	{
		yield return new WaitForEndOfFrame();
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
	public IEnumerator ForceTarget(string priorityTargetName, bool stayUntil, bool setInactive)
	{
		yield return new WaitForEndOfFrame();
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


	public void ReEnableMovementAnim()
    {
		animator.SetBool("move", true);
	}

}
