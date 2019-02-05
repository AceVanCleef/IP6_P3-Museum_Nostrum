using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraConfigurator : MonoBehaviour, ITagEnsurance
{
    AudioOptionsManager audioOptionsManager;

    void Start()
    {
        InitializeTag();
        getMasterVolume();
    }

    public void InitializeTag()
    {
        if (gameObject.tag != "MainCamera")
        {
            gameObject.tag = "MainCamera";
        }
    }
    private void getMasterVolume()
    {
        GameObject audioManager = GameObject.Find("AudioManager");
        if (audioManager)
        {
            audioOptionsManager = audioManager.GetComponent<AudioOptionsManager>();
            AudioListener.volume = audioOptionsManager.masterVolume;
        }
    }
}
