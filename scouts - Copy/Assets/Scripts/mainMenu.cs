using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public static mainMenu instance;
	private void Awake()
	{
        if (instance != null)
            throw new System.Exception("Main menu non è un singleton");
        instance = this;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
	}

	public void Quit()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

        Debug.Log("QUIT");
        Application.Quit();
    }

    public void Options()
    {
        //GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

        SceneManager.LoadScene(2);
    }


    public void GoToMenu()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");

        SceneManager.LoadScene(0);
    }

    public void OpenCredits()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

        SceneManager.LoadScene(3);
    }
}
