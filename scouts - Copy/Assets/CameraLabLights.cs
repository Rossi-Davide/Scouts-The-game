using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal; 

public class CameraLabLights : MonoBehaviour
{
    public Light2D lights;

    private void Start()
    {


        if (lights == null)
        {
            Debug.LogError("no luci");
        }
    
    }

    private void OnPreCull()
    {

        Debug.Log("called on pre");
            lights.enabled = false;
        
    }

  
    private void OnPostRender()
    {
        Debug.Log("called on pre");

        lights.enabled = true;
    }
}
