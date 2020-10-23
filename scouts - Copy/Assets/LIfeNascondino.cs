using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LIfeNascondino : MonoBehaviour
{
    public float life = 100f,spillo=5f,dinamite=10f;
    public Vector2 force;
    labirintoManager lb;
    Rigidbody2D rb;
    public void Start()
    {
        lb = GameObject.Find("/GameManager").GetComponent<labirintoManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();

    }

    public void Spillo()
    {
        life -= spillo;
        rb.AddForce(force);
        if (life <= 0)
        {
            lb.StartCoroutine("Sconfitta");
        }
    }

    public void Dinamite()
    {
        life -= dinamite;
        if (life <= 0)
        {
            lb.StartCoroutine("Sconfitta");
        }
    }


}
