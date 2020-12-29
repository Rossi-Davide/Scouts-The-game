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
}
