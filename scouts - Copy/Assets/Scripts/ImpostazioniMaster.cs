using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ImpostazioniMaster : MonoBehaviour
{
    #region Singleton
    public static ImpostazioniMaster instance;
    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        instance = this;
        DontDestroyOnLoad(instance);
    }
    #endregion

    public AudioMixer mixer;

    [HideInInspector] [System.NonSerialized]
    public float generalVolume;
    [HideInInspector] [System.NonSerialized]
    public float musicVolume;
    [HideInInspector] [System.NonSerialized]
    public float soundsVolume;
    [HideInInspector] [System.NonSerialized]
    public int qualityIndex;
    [HideInInspector] [System.NonSerialized]
    public bool fullscreen;

	private void Start()
	{
        //generalVolume = 5;
        //musicVolume = 5;
        //soundsVolume = 5;
        //resIndex = 0;
        qualityIndex = 0;
        fullscreen = false;
        SetStatus(SaveSystem.instance.LoadData<Status>(SaveSystem.instance.impostazioniMasterFileName, true));
        mixer.SetFloat("master", generalVolume);
        mixer.SetFloat("music", musicVolume);
        mixer.SetFloat("sounds", soundsVolume);
    }


	public Status SendStatus()
    {
        return new Status
        {
            generalVolume = generalVolume,
            musicVolume = musicVolume,
            soundsVolume = soundsVolume,
            qualityIndex = qualityIndex,
            //resIndex = resIndex,
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
            //resIndex = status.resIndex;
            fullscreen = status.fullscreen;
        }
    }
    public class Status
    {
        public float generalVolume;
        public float musicVolume;
        public float soundsVolume;
        public int qualityIndex;
        //public int resIndex;
        public bool fullscreen;
    }
}
