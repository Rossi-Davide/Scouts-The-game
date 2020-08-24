using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class impostazioni : MonoBehaviour
{
    Resolution[] resolutions;
    public AudioMixer mixer;
    public Dropdown resDrop;

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
        mixer.SetFloat("master",volume);
    }


    public void SetVolumeMusic(float volume)
    {
        mixer.SetFloat("music", volume);
    }

    public void SetVolumeSounds(float volume)
    {
        mixer.SetFloat("sounds", volume);
    }


    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool screen)
    {
        Screen.fullScreen = screen;
    }

    public void SetRes(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void tornaAlMenu()
    {
        SceneManager.LoadScene(0);
    }
}
