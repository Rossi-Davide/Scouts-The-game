using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candy : MonoBehaviour
{
    LIfeNascondino lf;
    GameObject player;
    public Animator an;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            an.SetBool("taken", true);
        }
    }


    void AggiornaCounter()
    {
        lf.CaramelleCounter();

    }


    private void Start()
    {
        player = GameObject.Find("/Player");
        lf = player.GetComponent<LIfeNascondino>();
    }
}
