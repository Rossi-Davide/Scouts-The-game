using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNascondino : MonoBehaviour
{
    public Joystick joy;
    public float playSpeed = 40f;
    private void FixedUpdate()
    {
        Vector3 movement = joy.direction;
        transform.position = Vector3.Lerp(transform.position, transform.position + movement, Time.deltaTime*playSpeed);
    }
}
