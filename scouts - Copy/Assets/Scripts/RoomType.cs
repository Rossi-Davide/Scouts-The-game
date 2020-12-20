using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public int type;

    public void RoomDestruction()
    {
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("CheckEndGeneration", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void CheckEndGeneration()//il metodo non è chimato nell'update per evitare il sovraccarico
    {
        if (GameObject.Find("/LevelGenerator").GetComponent<LevelGenerator>().stopGeneration == true)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            Invoke("CheckEndGeneration", 1f);
        }
    }

}
