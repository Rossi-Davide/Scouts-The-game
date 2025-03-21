﻿using UnityEngine;
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

	[HideInInspector]
	[System.NonSerialized]
	public Squadriglia squadriglia;
	public ControllerArr[] controllers;
	Animator animator;
	Joystick joystick;

	private void Start()
    {
        animator = GetComponent<Animator>();
		SetStatus(SaveSystem.instance.LoadData<Status>(SaveSystem.instance.plFileName, false));
		animator.runtimeAnimatorController = controllers[(int)CampManager.instance.camp.settings.gender].controllers[(int)CampManager.instance.camp.settings.hair];
		joystick = Joystick.instance;
	}

	[System.Serializable]
	public class ControllerArr
	{
		public RuntimeAnimatorController[] controllers;
	}

	public Status SendStatus()
	{
		return new Status
		{
			active = gameObject.activeSelf,
			position = transform.position,
		};
	}
	void SetStatus(Status status)
	{
		if (status != null)
		{
			gameObject.SetActive(status.active);
			transform.position = status.position;
		}
	}
	[System.Serializable]
	public class Status
	{
		public bool active;
		public Vector3 position;
	}



	public float playerSpeed; 
    float lastX, lastY;
	public bool isMoving;
	void FixedUpdate()
	{
		Vector3 movement = joystick.direction;
		if (Input.GetKey(KeyCode.W))
		{
			movement.y = 1;
		}
		if (Input.GetKey(KeyCode.S))
		{
			movement.y = -1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			movement.x = 1;
		}
		if (Input.GetKey(KeyCode.A))
		{
			movement.x = -1;
		}

		transform.position = Vector3.Lerp(transform.position, transform.position + movement, Time.deltaTime * playerSpeed);
		if (isMoving)
		{
			transform.Find("WaterParticle").GetComponent<ParticleSystem>().Play();
		}

		animator.SetFloat("Speed", movement.sqrMagnitude);
		if (movement.sqrMagnitude >= 0.01) 
		{
			
			Invoke("StepSounds", 0.5f);
			isMoving = true;
			animator.SetFloat("XMovement", movement.x);
			animator.SetFloat("YMovement", movement.y);
			lastX = movement.x;
			lastY = movement.y;
		}
		else
		{
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Stop("walking");

			isMoving = false;
			animator.SetFloat("XMovement", lastX);
			animator.SetFloat("YMovement", lastY);
		}




	}

	public void StepSounds()
	{
		
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("walking");

	}
}
