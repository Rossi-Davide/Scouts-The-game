using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
  

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void Options()
    {
        SceneManager.LoadScene(3);
    }
}
