using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRooms : MonoBehaviour
{
    public LayerMask WhatISRoom;
    public LevelGenerator level;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, WhatISRoom);
        if (roomDetection == null&&level.stopGeneration==true)
        {
            int rand = Random.Range(0, level.rooms.Length);
            Instantiate(level.rooms[rand], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
