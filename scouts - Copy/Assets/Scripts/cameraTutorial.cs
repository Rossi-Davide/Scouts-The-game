using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTutorial : MonoBehaviour
{
    public Vector3 pos;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = pos;
        cam.orthographicSize = 5f;
    }

  }
