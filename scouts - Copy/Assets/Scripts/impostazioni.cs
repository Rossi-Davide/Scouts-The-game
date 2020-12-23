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
	private void Awake()
	{
		if (instance != null)
			throw new System.Exception("impostazioni non è un singleton!");
		instance = this;
	}
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

        generalVolume = 100;
		musicVolume = 100;
		effectsVolume = 100;
		resIndex = 0;
		qualityIndex = 0;
		fullscreen = false;
        SetStatus(SaveSystem.instance.LoadData<Status>(SaveSystem.instance.impostazioniFileName, true));
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


	#region stuff

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
    #endregion

    public void TornaAlMenu()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");
        SaveSystem.instance.SaveData(SendStatus(), SaveSystem.instance.impostazioniFileName, true);
        SceneLoader.instance.LoadMainMenuScene();
    }

    public Status SendStatus()
    {
        return new Status
        {
            generalVolume = generalVolume,
            musicVolume = musicVolume,
            effectsVolume = effectsVolume,
            qualityIndex = qualityIndex,
            resIndex = resIndex,
            fullscreen = fullscreen,
        };
    }
    void SetStatus(Status status)
    {
        if (status != null)
        {
            generalVolume = status.generalVolume;
            musicVolume = status.musicVolume;
            effectsVolume = status.effectsVolume;
            qualityIndex = status.qualityIndex;
            resIndex = status.resIndex;
            fullscreen = status.fullscreen;
        }
    }
    public class Status
    {
        public int generalVolume;
        public int musicVolume;
        public int effectsVolume;
        public int qualityIndex;
        public int resIndex;
        public bool fullscreen;
    }


}
