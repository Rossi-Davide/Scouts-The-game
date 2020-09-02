using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    public LayerMask player;
    public LayerMask coll;
    public float viewRadius;
    /*[HideInInspector]
    public List<Transform> visibleTarget = new List<Transform>();*/

    [Range(0,360)]
    public float viewAngle;


    private void Start()
    {
        StartCoroutine("callingVision", .2f);
    }





    public Vector2 DirFromAngle(float angle,bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angle += transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    }


    void FindVisibleTarget()
    {
        //visibleTarget.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, player);
        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            Vector2 direct = dirToTarget;
            if (Vector2.Angle(transform.right, direct) < viewAngle / 2)
            {
                float disToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, direct, disToTarget, coll))
                {
                    //visibleTarget.Add(target);
                    AImaster AIBrain = GetComponent<AImaster>();
                    AIBrain.playerFound = true;
                    //Debug.Log("giocatore trovato");
                }
            }
        }
            
        
        
    }

    IEnumerator callingVision(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTarget();
        }
    }
}
