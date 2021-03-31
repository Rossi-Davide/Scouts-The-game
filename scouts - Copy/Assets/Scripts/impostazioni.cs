using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class impostazioni : MonoBehaviour
{
    //Resolution[] resolutions;
    public AudioMixer mixer;
    //public TMP_Dropdown resDrop;


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
    public TMP_Dropdown qualityUI;
    //public TMP_Dropdown resUI;

    private void Start()
    {
        //resolutions = Screen.resolutions;
        //resDrop.ClearOptions();
        //int defResolution = 0;
        //for(int i = 0; i < resolutions.Length; i++)
        //{
        //    string option = resolutions[i].width + "x" + resolutions[i].height;
        //    options.Add(option);
        //    if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
        //    {
        //        defResolution = i;
        //    }
        //}
        //resDrop.AddOptions(options);
        //resDrop.value = defResolution;
        //resDrop.RefreshShownValue();
        RefreshUI();
    }


	#region stuff

	public void SetVolumeMaster()
    {
        ImpostazioniMaster.instance.generalVolume = masterValue.transform.parent.GetComponentInChildren<Slider>().value;
        mixer.SetFloat("master", ImpostazioniMaster.instance.generalVolume);
        masterValue.text = Mathf.Round(ImpostazioniMaster.instance.generalVolume/4*5+100) + "%";
    }


    public void SetVolumeMusic()
    {
        ImpostazioniMaster.instance.musicVolume = musicValue.transform.parent.GetComponentInChildren<Slider>().value;
        mixer.SetFloat("music", ImpostazioniMaster.instance.musicVolume);
        musicValue.text = Mathf.Round(ImpostazioniMaster.instance.musicVolume / 4 * 5 + 100) + "%";
    }

    public void SetVolumeSounds()
    {
        ImpostazioniMaster.instance.soundsVolume = soundsValue.transform.parent.GetComponentInChildren<Slider>().value;
        mixer.SetFloat("sounds", ImpostazioniMaster.instance.soundsVolume);
        soundsValue.text = Mathf.Round(ImpostazioniMaster.instance.soundsVolume / 4 * 5 + 100) + "%";
    }


    public void SetQuality()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");
        ImpostazioniMaster.instance.qualityIndex = qualityUI.value;
        QualitySettings.SetQualityLevel(ImpostazioniMaster.instance.qualityIndex);
    }

    public void Setfullscreen()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");
        ImpostazioniMaster.instance.fullscreen = fullscreenUI.isOn;
        Screen.fullScreen = ImpostazioniMaster.instance.fullscreen;
    }

    //public void SetRes()
    //{
    //    GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");
    //    resIndex = resUI.value;
    //    Resolution resolution = resolutions[resIndex];
    //    Screen.SetResolution(resolution.width, resolution.height, Screen.ImpostazioniMaster.instance.fullscreen);
    //}


    void RefreshUI()
	{
        masterValue.text = Mathf.Round(ImpostazioniMaster.instance.generalVolume / 4 * 5 + 100) + "%";
        masterValue.transform.parent.GetComponentInChildren<Slider>().value = ImpostazioniMaster.instance.generalVolume;
        musicValue.text = Mathf.Round(ImpostazioniMaster.instance.musicVolume / 4 * 5 + 100) + "%";
        musicValue.transform.parent.GetComponentInChildren<Slider>().value = ImpostazioniMaster.instance.musicVolume;
        soundsValue.text = Mathf.Round(ImpostazioniMaster.instance.soundsVolume / 4 * 5 + 100) + "%";
        soundsValue.transform.parent.GetComponentInChildren<Slider>().value = ImpostazioniMaster.instance.soundsVolume;
        fullscreenUI.isOn = ImpostazioniMaster.instance.fullscreen;
        qualityUI.value = ImpostazioniMaster.instance.qualityIndex;
        //resUI.value = resIndex;
    }
    #endregion

    public void TornaAlMenu()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("clickDepitched");
        SaveSystem.instance.SaveData(ImpostazioniMaster.instance.SendStatus(), SaveSystem.instance.impostazioniMasterFileName, true);
        SceneLoader.instance.LoadMainMenuScene();
    }
}
