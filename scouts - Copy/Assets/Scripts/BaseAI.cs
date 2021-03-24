using UnityEngine;
using Pathfinding;
using System.Collections;

public abstract class BaseAI : InGameObject
{
	protected float speed = 60;

	protected float minWayPointDistance = 1;
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

	protected Vector2 pos;
	protected bool toggleCheckBlocco = true;
	protected short cont = 0;
	protected bool ai = false;
	protected string[] nomiAI = { "SquadrigliereF1(Clone)", "SquadrigliereF2(Clone)", "SquadrigliereM1(Clone)", "SquadrigliereM2(Clone)" };
	protected Vector3[] posizioneOggettiPrinc = new[] {new Vector3(16.82f,-2.69f,0f), new Vector3(13.93f,-0.84f,0f), new Vector3(21.24f,-2.97f,0f), new Vector3(2.21f,-3.67f,0f), new Vector3(11.2f,-10.79f,0f) };
	protected short timeSpentAtAngle = 0,contTimeSpentAtAngle=9;
	protected short prob,stuckInCollinetta=0;

	private void OnDisable()
	{
		Debug.Log("trying to disable");
		//bool nearPlayer = Vector2.Distance(transform.position, Player.instance.transform.position) < 100f;
		//if (nearPlayer)
		//{
		//	UnlockNow();
		//}
	}

	protected override void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		seeker = GetComponent<Seeker>();
		base.Start();
		animator.SetBool("move", true);

		pos = rb.position;

		foreach(string s in nomiAI)
        {
            if (gameObject.name == s)
            {
				ai = true;
            }
        }


		CreateNewPath(null, false);

		InvokeRepeating(nameof(CheckPriorityPathConditions), 1f, 1f);
		InvokeRepeating(nameof(UpdateSlowed), 0.05f, 0.05f);
		InvokeRepeating(nameof(CheckStop), 5f, 5f);
	}


	void CheckStop()
    {
		if((pos.x-0.2f)<rb.position.x&&rb.position.x<(pos.x+0.2f)&& (pos.y - 0.2f) < rb.position.y && rb.position.y < (pos.y + 0.2f))
        {
			//Debug.LogError("CheckStop");
			CreateNewPath(null, false);
		}
		pos = rb.position;
    }


	public override void Select()
	{
		base.Select();
		StartCoroutine(ForceTarget(Player.instance.transform.position, true, false, true));
	}
	public override void Deselect()
	{
		base.Deselect();
		UnlockAndCreateExPlayerPath();
	}

	public virtual void SetMissingPriorityTarget(string targetName, Vector3 pos) { }

	protected  virtual void CreateNewPath(Vector3? priorityTarget, bool isPlayer)
	{
		//Debug.LogError(priorityTarget+"priority");
		//Debug.Log(transform.parent.name);
		int n1, n2;
        if (ai)
        {
            if (contTimeSpentAtAngle <= timeSpentAtAngle)
            {
				prob = 20;
				contTimeSpentAtAngle++;
            }
            else
            {
				 prob =(short) Random.Range(1, 101);
			}

			if (prob <= 40)
            {
				//probabilità che vadano al proprio angolo
				n1 =(int) Random.Range(transform.parent.position.x - 10, transform.parent.position.x + 10);
				n2 = (int)Random.Range(transform.parent.position.y - 10, transform.parent.position.y + 10);
				timeSpentAtAngle = (short)Random.Range(0, 9);
				contTimeSpentAtAngle = 0;
            }else if (prob > 40 && prob <= 60)
            {
				//probabilità che vadano al centro
				n1 = Random.Range(-20, 21);
				n2 = Random.Range(-20, 21);
			}
			else if (prob > 60 && prob <= 63)
            {
				//probabilità che vadano ai punti di interesse
				short p = (short)Random.Range(0,posizioneOggettiPrinc.Length);
				n1 =(int) Random.Range(posizioneOggettiPrinc[p].x - 8, posizioneOggettiPrinc[p].x + 8);
				n2 = (int)posizioneOggettiPrinc[p].y;
            }
            else
            {
				n1 = Random.Range(-45, 45);
				n2 = Random.Range(-45, 30);
			}
        }
        else
        {
			n1 = Random.Range(-45, 45);
			n2 = Random.Range(-45, 30);
		}

		

		Vector3 a = new Vector3(n1, n2, 0);
		currentTarget = priorityTarget != null ? priorityTarget.Value : a; //aggiorno la posizione dell'IA con un random
		//Debug.LogError(currentTarget+ "current target");
		seeker.StartPath(rb.position, currentTarget, VerifyPath);
		if (!isPlayer)
		{
			stuckInCollinetta++; cont++;
			if (stuckInCollinetta >= 5) ResetPos();
			if (toggleCheckBlocco)
			{
				if (gameObject.activeSelf) StartCoroutine(CheckBlocco());
				toggleCheckBlocco = !toggleCheckBlocco;
			}
		}
	}


	public void ResetPos()
    {
		int n1, n2;
		n1 = Random.Range(-45, 45);
		n2 = Random.Range(-45, 30);

		Vector3 a = new Vector3(n1, n2, 0);
		rb.position = a;
		Debug.LogWarning($"I, {objectName} {objectSubName}, have been reset");
	}


	IEnumerator CheckBlocco()
    {
		yield return new WaitForSeconds(5f);
        if (cont >= 5)
        {
			ResetPos();
		}
		cont = 0;
		toggleCheckBlocco = !toggleCheckBlocco;
		yield return null;
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
			//Debug.LogError("verify path");
			CreateNewPath(null, false);
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
			//Debug.LogWarning(disable);
		}
		if (stayUntil)
		{
			animator.SetBool("move", false);
		}
		if (keepTarget <= 0 && !disable && !stayUntil)
		{
			CheckPriorityTargetsThatWait();
		}
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
	public void UnlockNow() //call method from another script if stayUntil is true
	{
		gameObject.SetActive(true);
		keepTarget = 0;
		stayUntil = false;
		disable = false;
		animator = GetComponent<Animator>();
		animator.SetBool("move", true);
		CheckPriorityTargetsThatWait();
		ToggleClickListener(true);
	}

	public void UnlockAndCreateExPlayerPath()
	{
		UnlockNow();
		CreateNewPath(null, true);
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
		//Debug.LogError("CheckPriorityTargetsThatWait");
		if (target != null) CreateNewPath(target, false);
	}

	protected void UpdateSlowed()
	{
		if (currentPath == null) return;
		var nextWayPoint = currentPath.vectorPath[currentWayPointIndex];
		if (Vector2.Distance(nextWayPoint, rb.position) < minWayPointDistance)
		{
			if (currentWayPointIndex == currentPath.vectorPath.Count - 1)
			{
				//Path completata
				currentPath = null;
				currentWayPointIndex = 0;
				PathCompleted();
				stuckInCollinetta = 0;
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
        {
			CreateNewPath(target, false);
			Debug.LogError("CheckPriorityPathConditions");
		}
	}

	public IEnumerator ForceTarget(Vector3 target, int stay, bool setInactive, bool isPlayerTarget)
	{
		yield return new WaitForEndOfFrame();
		CreateNewPath(target, isPlayerTarget);
		keepTarget = stay;
		disable = setInactive;
	}
	public IEnumerator ForceTarget(Vector3 target, bool stayUntil, bool setInactive, bool isPlayerTarget)
	{
		yield return new WaitForEndOfFrame();
		//Debug.LogError(target);
		CreateNewPath(target, isPlayerTarget);
		this.stayUntil = stayUntil;
		disable = setInactive;
	}
	public IEnumerator ForceTarget(Vector3 target, bool setInactive, System.Action onEnd, bool isPlayerTarget)
	{
		yield return new WaitForEndOfFrame();
		CreateNewPath(target, isPlayerTarget);
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
				CreateNewPath(p.target, false);
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
				CreateNewPath(p.target, false);
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
