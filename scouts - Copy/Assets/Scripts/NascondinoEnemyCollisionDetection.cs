using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NascondinoEnemyCollisionDetection : MonoBehaviour
{
     nascondinoManager manager;

    
    // Start is called before the first frame update
    void Start()
    {
         manager= GameObject.Find("GameManager").gameObject.GetComponent<nascondinoManager>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name== "playerNascondino")
        {
            //Debug.Log("hai perso");

            manager.SconfittaVoid(); 
                
        }
       
    }
}
