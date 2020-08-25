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
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void Options()
    {
        SceneManager.LoadScene(2);
    }


    public void GoToMenu()
	{
        SceneManager.LoadScene(0);
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene(3);
    }
}
