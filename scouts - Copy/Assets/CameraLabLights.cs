using UnityEngine;
using System.Collections;

public class CameraLabLights : MonoBehaviour
{

    public Shader unlitShader;

    void Start()
    {
        //unlitShader = Shader.Find("Unlit/Texture");
        GetComponent<Camera>().SetReplacementShader(unlitShader, "");
    }
}