using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class schermateTut : MonoBehaviour
{
    public VideoClip[] videi;
    public Sprite[] photos;
    //public RawImage videoImageOut;
    public GameObject photosOut;
    public GameObject videoOut;
    public VideoPlayer player;
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
            case 2:
                photosOut.SetActive(false);
                break;
            case 3:
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
                photosOut.GetComponent<Image>().sprite = photos[4];
                break;
            case 8:
                photosOut.SetActive(false);
                player.clip = videi[2];
                videoOut.SetActive(true);
                break;
            case 9:
                videoOut.SetActive(false);
                photosOut.GetComponent<Image>().sprite = photos[5];
                photosOut.GetComponent<Image>().SetNativeSize();
                photosOut.SetActive(true);
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
                break;
        }
    }

}
