using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRooms : MonoBehaviour
{
    public LayerMask WhatISRoom;
    public LevelGenerator level;
    labirintoManager man;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnRoom",.2f);
        man = GameObject.Find("/GameManager").GetComponent<labirintoManager>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void SpawnRoom()
    {

        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, WhatISRoom);
        if (roomDetection == null && level.stopGeneration == true&&man.endGen!=true)
        {
            int rand = Random.Range(0, level.rooms.Length);
            Instantiate(level.rooms[rand], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            Invoke("SpawnRoom", .2f);
        }
    }
}
