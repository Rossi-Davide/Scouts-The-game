using UnityEngine;
using Pathfinding;
using UnityEngine.UI;


public abstract class BaseAI : InGameObject
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

	protected Transform priorityTarget;


	public GameObject buttonCanvas;
	public Button clickListenerButton;
	GameObject instanceOfListener;


	public event System.Action OnPathCreated;
	public event System.Action OnPathCompleted;

	protected override void Start()
	{
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		instanceOfListener = Instantiate(clickListenerButton.gameObject, transform.position, Quaternion.identity, buttonCanvas.transform);
		instanceOfListener.GetComponent<Button>().onClick.AddListener(OnClick);
		CreateNewPath();
		base.Start();

		OnPathCreated += PriorityPath;
		OnPathCompleted += AfterPathCompletion;
	}
	protected virtual void CreateNewPath()
	{
		target = randomTarget[Random.Range(0, randomTarget.Length)];
		seeker.StartPath(rb.position, target, VerifyPath);
		OnPathCreated?.Invoke();
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
			CreateNewPath();
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
		if (priorityTarget != null && target == priorityTarget.position)
			priorityTarget = null;
		CreateNewPath();
		OnPathCompleted?.Invoke();
	}


	public void PriorityPath()
	{
		if (priorityTarget != null)
		{
			target = priorityTarget.position;
			seeker.StartPath(rb.position, target, VerifyPath);
		}
	}

	public virtual void AfterPathCompletion()
	{

	}


	protected void Update()
	{
		instanceOfListener.transform.position = transform.position;
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
}
