using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spillo : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            Collider2D cl = collision.collider;
            cl.GetComponent<LIfeNascondino>().Spillo();
        }
    }
}
