using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlAnimRefettorio : MonoBehaviour
{
    public GameObject col1, col2, col3;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CheckAnim",0.1f, 0.5f);
    }

    void CheckAnim()
    {
        
    }
}
