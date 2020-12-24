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

	public TextMeshProUGUI masterValue, musicValue, soundsValue;
    public Toggle fullscreenUI;
    public TMP_Dropdown qualityUI, resUI;

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

        generalVolume = 80;
		musicVolume = 80;
		soundsVolume = 80;
		resIndex = 0;
		qualityIndex = 0;
		fullscreen = false;
        SetStatus(SaveSystem.instance.LoadData<Status>(SaveSystem.instance.impostazioniFileName, true));
        RefreshUI();
    }

    [HideInInspector] [System.NonSerialized]
    public float generalVolume;
    [HideInInspector] [System.NonSerialized]
    public float musicVolume;
    [HideInInspector] [System.NonSerialized]
    public float soundsVolume;
    [HideInInspector] [System.NonSerialized]
    public int resIndex;
    [HideInInspector] [System.NonSerialized]
    public int qualityIndex;
    [HideInInspector] [System.NonSerialized]
    public bool fullscreen;


	#region stuff

	public void SetVolumeMaster()
    {
        generalVolume = masterValue.transform.parent.GetComponentInChildren<Slider>().value;
        mixer.SetFloat("master", generalVolume);
        masterValue.text = Mathf.Round(generalVolume / 80 * 100) + "%";
    }


    public void SetVolumeMusic()
    {
        musicVolume = musicValue.transform.parent.GetComponentInChildren<Slider>().value;
        mixer.SetFloat("music", musicVolume);
        musicValue.text = Mathf.Round(musicVolume / 80 * 100) + "%";
    }

    public void SetVolumeSounds()
    {
        soundsVolume = soundsValue.transform.parent.GetComponentInChildren<Slider>().value;
        mixer.SetFloat("sounds", soundsVolume);
        soundsValue.text = Mathf.Round(soundsVolume / 80 * 100) + "%";
    }


    public void SetQuality()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");
        qualityIndex = qualityUI.value;
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");
        fullscreen = fullscreenUI.isOn;
        Screen.fullScreen = fullscreen;
    }

    public void SetRes()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");
        resIndex = resUI.value;
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    void RefreshUI()
	{
        masterValue.text = Mathf.Round(generalVolume / 80 * 100) + "%";
        masterValue.transform.parent.GetComponentInChildren<Slider>().value = generalVolume;
        musicValue.text = Mathf.Round(musicVolume / 80 * 100) + "%";
        musicValue.transform.parent.GetComponentInChildren<Slider>().value = musicVolume;
        soundsValue.text = Mathf.Round(soundsVolume / 80 * 100) + "%";
        soundsValue.transform.parent.GetComponentInChildren<Slider>().value = soundsVolume;
        fullscreenUI.isOn = fullscreen;
        qualityUI.value = qualityIndex;
        resUI.value = resIndex;
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
            soundsVolume = soundsVolume,
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
            soundsVolume = status.soundsVolume;
            qualityIndex = status.qualityIndex;
            resIndex = status.resIndex;
            fullscreen = status.fullscreen;
        }
    }
    public class Status
    {
        public float generalVolume;
        public float musicVolume;
        public float soundsVolume;
        public int qualityIndex;
        public int resIndex;
        public bool fullscreen;
    }


}
