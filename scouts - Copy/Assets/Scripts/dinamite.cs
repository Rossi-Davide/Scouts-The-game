using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dinamite : MonoBehaviour
{
    Collider2D[] colls;
    public float larghezzaCerchio = 1f;
    public Animator an;
    LIfeNascondino life;
    bool exTrigger = false;
    ParticleSystem dynamite;
    // Start is called before the first frame update
    private void Start()
    {
        life = GameObject.Find("/Player").GetComponent<LIfeNascondino>();
        dynamite = GetComponent<ParticleSystem>();
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

        if (!exTrigger)
        {
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].name == "Player")
                {
                    an.SetBool("ka-boom", true);
                    Debug.Log("ka-boom");
                    exTrigger = true;
                    //Invoke("Boom", 7f);
                    GameObject audio= GameObject.Find("/AudioManager");

                    if (audio != null)
                    {
                        audio.GetComponent<AudioManager>().Play("ticchettio dinamite");
                    }
                    else
                    {
                        Debug.Log("audio comp not found");
                    }
                    break;
                }
            }
        }    
    }

  

    IEnumerator EsplosioneCor()
    {
        dynamite.Play();     
        Debug.Log("avvenuta esplosione");

        colls = Physics2D.OverlapCircleAll(transform.position, larghezzaCerchio);// controllo che il player sia ancora nel raggio d'azione per ricevere i danni

        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].name == "Player")
            {
                Transform dinPos = this.gameObject.GetComponent<Transform>();
                life.Dinamite(dinPos);
                break;
            }
        }

        SpriteRenderer s = this.gameObject.GetComponent<SpriteRenderer>();
        s.enabled = false;
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        yield return null;
    }
}
