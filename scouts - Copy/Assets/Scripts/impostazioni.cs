using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;

public class impostazioni : MonoBehaviour
{
    Resolution[] resolutions;
    public AudioMixer mixer;
    public TMP_Dropdown resDrop;

    public TextMeshProUGUI masterValue, musicValue, effectsValue;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resDrop.ClearOptions();

        List<string> options = new List<string>();

        int defResolution = 0;

        
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                defResolution = i;
            }
        }
        resDrop.AddOptions(options);
        resDrop.value = defResolution;
        resDrop.RefreshShownValue();
    }

    public void SetVolumeMaster(float volume)
    {
        mixer.SetFloat("master", volume - 80);
        masterValue.text = Mathf.Round(volume / 80 * 100) + "%";
    }


    public void SetVolumeMusic(float volume)
    {
        mixer.SetFloat("music", volume - 80);
        musicValue.text = Mathf.Round(volume / 80 * 100) + "%";
    }

    public void SetVolumeSounds(float volume)
    {
        mixer.SetFloat("sounds", volume - 80);
        effectsValue.text = Mathf.Round(volume / 80 * 100) + "%";
    }


    public void SetQuality(int qualityIndex)
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool screen)
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

        Screen.fullScreen = screen;
    }

    public void SetRes(int resIndex)
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void tornaAlMenu()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");

        SceneManager.LoadScene(0);
    }
}
