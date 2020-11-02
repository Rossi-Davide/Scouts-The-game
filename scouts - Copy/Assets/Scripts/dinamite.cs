using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dinamite : MonoBehaviour
{
    Collider2D[] colls;
    public float larghezzaCerchio = 1f;
    public Animator an;
    LIfeNascondino life;
    // Start is called before the first frame update
    private void Start()
    {
        life = GameObject.Find("/Player").GetComponent<LIfeNascondino>();
    }



    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            Collider2D cl = collision.collider;
            cl.GetComponent<LIfeNascondino>().Dinamite();
        }
    }*/

    private void Update()
    {
        colls = Physics2D.OverlapCircleAll(transform.position, larghezzaCerchio);

        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].name == "Player")
            {
                an.SetBool("ka-boom", true);
                Debug.Log("ka-boom");
                break;
            }
        }
    }


    void Esplosione()
    {
        life.Dinamite();
        Destroy(gameObject);
    }
}
