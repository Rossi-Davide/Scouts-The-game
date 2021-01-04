using UnityEngine;
using System.Collections;

public class MiniMap : MonoBehaviour
{

    public RenderTexture renderTexture;
    public Material material;

    public FilterMode filterMode = FilterMode.Point;

    public Camera miniMapCamera;
    public Vector2 mapTextureSize = new Vector2(256, 256);

    void Start()
    {

        renderTexture = new RenderTexture((int)mapTextureSize.x, (int)mapTextureSize.y, 16);
        miniMapCamera.targetTexture = renderTexture;

        renderTexture.filterMode = filterMode;

    }


    void OnGUI()
    {

        GUI.depth = 4;

        if (miniMapCamera.enabled == true)
        {

            if (Event.current.type == EventType.Repaint)
            {
                Graphics.DrawTexture(new Rect(Screen.width - mapTextureSize.x, Screen.height - mapTextureSize.y, mapTextureSize.x, mapTextureSize.y), renderTexture, material);
            }
        }


    }

}
