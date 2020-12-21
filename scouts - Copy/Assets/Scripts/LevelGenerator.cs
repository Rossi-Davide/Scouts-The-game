using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Transform[] positions;
    public GameObject[] rooms;
    int direction;
    public float moveAmount;
    public float startTime;
    float timeBTWrooms;
    public int MaxX;
    public int MinX;
    public int MinY;
    public bool stopGeneration = false;
     int downCounter;
    public LayerMask LayerRoom;

  
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, positions.Length);
        transform.position = positions[rand].position;
        direction = Random.Range(0, 6);
        Instantiate(rooms[0], transform.position, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        if (timeBTWrooms <= 0&&stopGeneration==false)
        {
        
            Move();
           
            timeBTWrooms = startTime;
        }
        else
        {
            timeBTWrooms -= Time.deltaTime;
        }
    }

    void Move()
    {

        if (direction == 1 || direction == 2)//move right
        {
            downCounter = 0;
            if (transform.position.x < MaxX)
            {
                Vector2 newPosition = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPosition;
                direction = Random.Range(1, 6);
                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                if (direction == 3)
                {
                    direction = 2;
                }
                else if (direction == 4)
                {
                    direction = 5;
                }
            }
            else
            {
                direction = 5;
            }
            
           

        }else if (direction == 3 || direction == 4)//move left
        {
            downCounter = 0;

            if (transform.position.x > MinX)
            {
                Vector2 newPosition = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPosition;
                direction = Random.Range(3, 6);
                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

            }
            else
            {
                direction = 5;
            }
           
        }
        else if (direction == 5)//move down
        {
            downCounter++;
            if (transform.position.y > MinY)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position,1,LayerRoom);

                if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3)
                {
                    if (downCounter >= 2)
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();
                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();
                        int RandomBottom = Random.Range(1, 4);
                        if (RandomBottom == 2)
                        {
                            RandomBottom = 1;
                        }
                        Instantiate(rooms[RandomBottom], transform.position, Quaternion.identity);
                    }
                    
                }
                Vector2 newPosition = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPosition;
                direction = Random.Range(1, 6);
                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

            }
            else
            {
                stopGeneration = true;
            }
           
        }


    }
}
