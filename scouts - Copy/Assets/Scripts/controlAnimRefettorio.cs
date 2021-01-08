using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlAnimRefettorio : MonoBehaviour
{
    public GameObject col1, col2, col3;
    // Start is called before the first frame update
    public void Liv1()
    {
        col1.SetActive(true);
        col2.SetActive(false);
        col3.SetActive(false);
    }

    public void Liv2()
    {
        col1.SetActive(false);
        col2.SetActive(true);
        col3.SetActive(false);
    }

    public void Liv3()
    {
        col1.SetActive(false);
        col2.SetActive(false);
        col3.SetActive(true);
    }

    private void Start()
    {
        InvokeRepeating("CheckModificaBase", 0.1f, 0.5f);
    }

    void CheckModificaBase()
    {
        //Debug.Log("called");
        if (ModificaBaseTrigger.instance.isModifying)
        {
            gameObject.GetComponent<Animator>().enabled = false;
            foreach(Transform t in gameObject.transform)
            {

                t.gameObject.SetActive(false);
            }

            gameObject.GetComponent<BoxCollider2D>().enabled = true;

            EdgeCollider2D edgeColl = gameObject.GetComponent<EdgeCollider2D>();

            if (edgeColl != null)
            {
                edgeColl.enabled = false;
            }
        }
        else
        {
            gameObject.GetComponent<Animator>().enabled = true;

            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            EdgeCollider2D edgeColl = gameObject.GetComponent<EdgeCollider2D>();

            if (edgeColl != null)
            {
                edgeColl.enabled = true;
            }
        }
    }
}
