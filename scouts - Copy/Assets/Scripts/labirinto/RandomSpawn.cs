using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public int minX, maxX, minY, maxY;
    // Start is called before the first frame update
    void Start()
    {
        PosizioneUpdate();
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    void PosizioneUpdate()
    {
        Vector2 posizione;
        posizione.x = Random.Range(minX,maxX+1);
        posizione.y = Random.Range(minY,maxY+1);
        transform.position = transform.position + (Vector3)posizione;
        Collider2D coll = Physics2D.OverlapCircle(transform.position, 1);

        if (coll == null)
        {
            PosizioneUpdate();
        }
    }
}
