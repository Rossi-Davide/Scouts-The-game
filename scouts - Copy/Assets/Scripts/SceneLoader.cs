using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	#region Singleton
	public static SceneLoader instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);    
        instance = this;
        DontDestroyOnLoad(this);
        Screen.orientation = ScreenOrientation.Landscape;
    }

    

    #endregion

    public void QuitApplication()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");
        Application.Quit();
    }
    public void LoadSettingsScene()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

        SceneManager.LoadScene("impostazioni");
    }
    public void LoadTutorialScene()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadMainMenuScene()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");

        SceneManager.LoadScene("StartMenu");
    }

    public void LoadCreditsScene()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

        SceneManager.LoadScene("Crediti");
    }
    public void LoadCampCreateScene()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

        SceneManager.LoadScene("CreateCamp");
    }
}
