using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioOptionsManager : MonoBehaviour
{
    //public because they need to be set from another script
    public float masterVolume;
    public float musicVolume;
    public float soundVolume;

    private GameObject[] musicSources;
    private GameObject[] soundSources;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //sets all volumes to 1 on start
        masterVolume = 1;
        musicVolume = 1;
        soundVolume = 1;
    }

    //needed for OnSceneLoaded()
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //sets the volume in the level with the values set in the startmenue
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //check if game scene
        if (scene.name != "StartMenu" && scene.name != "EntranceAnimation")
        {
            musicSources = GameObject.FindGameObjectsWithTag("BackgroundMusic");
            for (int i = 0; i < musicSources.Length; i++)
            {
                {
                    musicSources[i].GetComponent<AudioSource>().volume = musicVolume;
                }
            }

            soundSources = GameObject.FindGameObjectsWithTag("Sound");
            for (int i = 0; i < soundSources.Length; i++)
            {
                {
                    soundSources[i].GetComponent<AudioSource>().volume = soundVolume;
                }
            }
        }
    }
}
