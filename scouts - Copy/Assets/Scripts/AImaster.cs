using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AImaster : MonoBehaviour
{
    GameObject targetObj;
    public Transform target;
    public float speed = 200f;
    public float nextWayPointDistance = 3f;


    Path path;
    int currentWayPoint = 0;
    //bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rb;
    Vector3 meta;
    [HideInInspector]
    [System.NonSerialized]
   public bool playerFound=false;

    // Start is called before the first frame update
    void Start()
    {
        targetObj = GameObject.Find("playerNascondino");
        if (targetObj == null)
        {
            Debug.Log("giocatore non trovato");
        }
        else
        {
            target = targetObj.transform;
        }
        AggiornaPosizione();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f,0.5f);
        InvokeRepeating("AggiornaPosizione",0f,10f);
    }

    void UpdatePath()
    {
        if (playerFound == true)
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);

            }
        }
        else
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(rb.position, meta, OnPathComplete);

            }
        }
        
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }
        if (currentWayPoint >= path.vectorPath.Count)
        {
            //reachedEndOfPath = true;

            AggiornaPosizione();
            return;
        }
        //else
        //{

        //    reachedEndOfPath = false;
        //}

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;



        rb.AddForce(force);
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }
    }


    void AggiornaPosizione()
    {
        if (playerFound == false)
        {
            nascondinoManager manager = GameObject.Find("GameManager").GetComponent<nascondinoManager>();


            if (manager.aumentoDifficoltà == false)
            {
                meta.y = (Random.Range(-428, 383)) / 100;
                meta.x = (Random.Range(-1101, 987)) / 100;
                meta.z = 0;
            }
            else
            {
                //Debug.Log("restringimento area");
                do
                {
                    meta.y = Random.Range(target.position.y - 10, target.position.y + 10);

                } while (meta.y<(-428/100)||meta.y>=(383/100));

                do
                {
                    meta.x = Random.Range(target.position.x - 10, target.position.x + 10);

                } while (meta.x < (-1101 / 100) || meta.x >= (987 / 100));
                meta.z = 0;

            }

        }

    }
}
