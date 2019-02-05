using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetAudioVolumes : MonoBehaviour {

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;

    AudioOptionsManager audioOptionsManager;
    AudioListener audioListener;

    public AudioSource musicSrc;
    public AudioSource soundSrc;

    //the music volume needs to be lowered, otherwise the soundeffects are to quiet
    private float soundDowntoner = 0.25f;
    

    void Start () {
        GameObject audioManager = GameObject.Find("AudioManager");
        audioOptionsManager = audioManager.GetComponent<AudioOptionsManager>();
        GameObject caw = GameObject.Find("CameraAnimationWrapper");
        audioListener = caw.GetComponentInChildren<AudioListener>();
    }
	
	void Update ()
    {
        //sets the values in the Audio-Manager
        audioOptionsManager.masterVolume = masterVolumeSlider.value;
        audioOptionsManager.musicVolume = musicVolumeSlider.value * soundDowntoner;
        audioOptionsManager.soundVolume = soundVolumeSlider.value;

        //sets the volume in the Startmenu
        //audioListener.volume = masterVolumeSlider.value;
        musicSrc.volume = musicVolumeSlider.value * soundDowntoner;
        soundSrc.volume = soundVolumeSlider.value;
    }

}
