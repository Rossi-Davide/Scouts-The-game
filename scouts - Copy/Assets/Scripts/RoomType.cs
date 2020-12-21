using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public int type;
    public labirintoManager man;
    public void RoomDestruction()
    {
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("CheckEndGeneration", 20f);
        man = GameObject.Find("/GameManager").GetComponent<labirintoManager>();
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
            Debug.Log("collider disabled");
            man.CollsDisabled();
            
        }
        else
        {
            Invoke("CheckEndGeneration", 1f);
        }
    }



    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Debug.Log("collider diaìsabled");
    }*/

}
