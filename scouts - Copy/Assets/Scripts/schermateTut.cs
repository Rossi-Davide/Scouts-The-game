using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class schermateTut : MonoBehaviour
{
    public VideoClip[] videi;
    public Sprite[] photos;
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

    public void AggiornaImage(int contatore)
    {
        switch (contatore)
        {
            case 1:
                photosOut.GetComponent<Image>().sprite = photos[0];
                videoOut.SetActive(false);
                photosOut.SetActive(true);
                break;
            case 2:
                photosOut.SetActive(false);
                videoOut.SetActive(true);
                player.clip = videi[0];
                break;
            case 3:
                photosOut.SetActive(false);
                videoOut.SetActive(true);
                player.clip = videi[1];
                break;

            case 4:
                photosOut.GetComponent<Image>().sprite = photos[1];
                videoOut.SetActive(false);
                photosOut.SetActive(true);
                break;
            case 5:
                photosOut.GetComponent<Image>().sprite = photos[2];
                break;
            case 6:
                photosOut.GetComponent<Image>().sprite = photos[3];
                break;
            case 7:
                videoOut.SetActive(false);
                photosOut.GetComponent<Image>().sprite = photos[4];
                //photosOut.GetComponent<Image>().SetNativeSize();
                photosOut.SetActive(true);
                break;
            case 8:
                photosOut.SetActive(false);
                player.clip = videi[2];
                videoOut.SetActive(true);
                break;
            case 9:
                photosOut.SetActive(false);
                videoOut.SetActive(true);
                player.clip = videi[6];
                break;
            case 10:
                photosOut.SetActive(false);
                player.clip = videi[3];
                videoOut.SetActive(true);
                break;
            case 11:
                photosOut.SetActive(false);
                player.clip = videi[4];
                videoOut.SetActive(true);
                button.SetActive(false);
                break;
            case 12:
                photosOut.SetActive(false);
                player.clip = videi[5];
                videoOut.SetActive(true);
                button.SetActive(true);
                break;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

}
