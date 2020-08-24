using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class levelLoader : MonoBehaviour
{
    public GameObject menu;
    public Slider loadingBar;
    public TextMeshProUGUI progressText;
  public void LoadLevel()
    {
        StartCoroutine(caricamento());
        

    }
    private void Start()
    {

    }

    IEnumerator caricamento()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        menu.SetActive(true);
        while (operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingBar.value = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
        }
    }
}
