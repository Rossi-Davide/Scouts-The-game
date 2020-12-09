using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fineMondo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogError("confine mondo superati,errore");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
