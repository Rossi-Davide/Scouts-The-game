using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class schermateTut : MonoBehaviour
{
    //public VideoClip[] videi;
    //public Sprite[] photos;
    //public RawImage videoImageOut;
    public GameObject photosOut;
    public GameObject videoOut;
    public VideoPlayer player;
    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AggiornaImage(TutorialSentence sentence)
    {
        videoOut.SetActive(sentence.clip != null);
        photosOut.SetActive(sentence.photo != null);
        if (sentence.clip != null) player.clip = sentence.clip;
        if (sentence.photo != null) photosOut.GetComponent<Image>().sprite = sentence.photo;
    }
    public void EnableButton(bool active)
	{
        button.SetActive(active);
	}

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }

}
