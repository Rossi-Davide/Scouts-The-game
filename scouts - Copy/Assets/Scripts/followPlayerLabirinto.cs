using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayerLabirinto : MonoBehaviour
{
    public Transform p;
    public Vector3 camOff;
    public float camSpeed;
    // Start is called before the first frame update
    void Start()
    {
        //transform.position = Vector3.Lerp(transform.position, p.position + camOff,camSpeed*Time.deltaTime );

    }

    // Update is called once per frame
    void Update()
    {
      
        transform.position = Vector3.Lerp(transform.position, p.position+camOff, camSpeed*Time.deltaTime);

    }
}
