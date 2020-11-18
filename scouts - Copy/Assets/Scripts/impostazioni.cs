using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class impostazioni : MonoBehaviour
{
    Resolution[] resolutions;
    public AudioMixer mixer;
    public TMP_Dropdown resDrop;


    #region Singleton
    public static impostazioni instance;
	/*private void Awake()
	{
        if (instance != null)
            throw new System.Exception("impostazioni non è un singleton!");
        instance = this;
        DontDestroyOnLoad(instance);
	}*/
	#endregion

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

        generalVolume = CampManager.instance.appSettings.generalVolume;
        musicVolume = CampManager.instance.appSettings.musicVolume;
        effectsVolume = CampManager.instance.appSettings.effectsVolume;
        qualityIndex = CampManager.instance.appSettings.qualityIndex;
        resIndex = CampManager.instance.appSettings.resIndex;
        fullscreen = CampManager.instance.appSettings.fullscreen;

    }

    [HideInInspector] [System.NonSerialized]
    public int generalVolume;
    [HideInInspector] [System.NonSerialized]
    public int musicVolume;
    [HideInInspector] [System.NonSerialized]
    public int effectsVolume;
    [HideInInspector] [System.NonSerialized]
    public int resIndex;
    [HideInInspector] [System.NonSerialized]
    public int qualityIndex;
    [HideInInspector] [System.NonSerialized]
    public bool fullscreen;



    public void SetVolumeMaster(int volume)
    {
        generalVolume = volume - 80;
        mixer.SetFloat("master", generalVolume);
        masterValue.text = Mathf.Round(volume / 80 * 100) + "%";
    }


    public void SetVolumeMusic(int volume)
    {
        musicVolume = volume - 80;
        mixer.SetFloat("music", musicVolume);
        musicValue.text = Mathf.Round(volume / 80 * 100) + "%";
    }

    public void SetVolumeSounds(int volume)
    {
        effectsVolume = volume - 80;
        mixer.SetFloat("sounds", effectsVolume);
        effectsValue.text = Mathf.Round(volume / 80 * 100) + "%";
    }


    public void SetQuality(int qi)
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");
        qualityIndex = qi;
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool screen)
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");
        fullscreen = screen;
        Screen.fullScreen = fullscreen;
    }

    public void SetRes(int ri)
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");
        resIndex = ri;
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void TornaAlMenu()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");

        CampManager.instance.appSettings = new AppSettings { generalVolume = this.generalVolume, musicVolume = this.musicVolume, effectsVolume = this.effectsVolume, qualityIndex = this.qualityIndex, resIndex = this.resIndex, fullscreen = this.fullscreen };

        SceneLoader.instance.LoadMainMenuScene();
    }
}
