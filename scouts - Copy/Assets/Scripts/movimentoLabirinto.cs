using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimentoLabirinto : MonoBehaviour
{
    public float playerSpeed;
    float lastX, lastY;
    Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    //copied from the 'real' player script
    void FixedUpdate()
    {
        Vector3 movement = Joystick.instance.direction;
        transform.position = Vector3.Lerp(transform.position, transform.position + movement, Time.deltaTime * playerSpeed);


        animator.SetFloat("Speed", movement.sqrMagnitude);
        if (movement.sqrMagnitude >= 0.01)
        {

            Invoke("StepSounds", 0.5f);
            animator.SetFloat("XMovement", movement.x);
            animator.SetFloat("YMovement", movement.y);
            lastX = movement.x;
            lastY = movement.y;
        }
        else
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Stop("walking");

            animator.SetFloat("XMovement", lastX);
            animator.SetFloat("YMovement", lastY);
        }
    }
}
