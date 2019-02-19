using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IngameOptionsMenu : MonoBehaviour
{

    //arrays for respective gameObjects
    GameObject[] musicSources;
    GameObject[] lights;
    GameObject[] interiorObjects;
    GameObject[] wayfinding;

    private bool audioActive = false;
    private bool lightsActive = false;
    private bool interiorObjectsActive = false;
    private bool wayfindingActive = false;

    private float masterVolume;
    private float musicVolume;
    private float soundVolume;


    private GameObject[] soundSources;


    GameObject musicSlider;
    GameObject soundSlider;

    Slider slider;

    void Awake()
    {
        masterVolume = 1;
        musicVolume = 1;
        soundVolume = 1;
        // masterVolume = datacarrier.instance.masterVolume;
        // musicVolume = datacarrier.instance.musicVolume;
        //  soundVolume = datacarrier.instance.soundVolume;


    }
    void Start()
    {
        musicSources = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        lights = GameObject.FindGameObjectsWithTag("Lights");
        interiorObjects = GameObject.FindGameObjectsWithTag("InteriorHolder");
        wayfinding = GameObject.FindGameObjectsWithTag("WayfindingHolder");
        soundSources = GameObject.FindGameObjectsWithTag("Sound");

        MainCameraConfigurator mCC = GameObject.Find("Main Camera").GetComponent<MainCameraConfigurator>();

        mCC.setMasterVolume();
        setMusicVolume();
        setSoundVolume();
    }

    public void closeScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "StartMenu")
        {
            Debug.Log("Quit");
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene("Assets/Main/Scenes/StartMenu.unity");
        }
    }

    public void startGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "StartMenu")
        {
            SceneManager.LoadScene("Assets/Main/Scenes/Animation/EntranceAnimation.unity");
        }
    }

    public void toggleAudio()
    {
        //toggle audio
        for (int i = 0; i < musicSources.Length; i++)
        {
            {
                musicSources[i].SetActive(audioActive);
            }
        }
        audioActive = !audioActive;
    }

    public void toggleLights()
    {
        //toggle lights
        for (int i = 0; i < lights.Length; i++)
        {
            {
                lights[i].SetActive(lightsActive);
            }
        }
        lightsActive = !lightsActive;
    }

    public void toggleInterior()
    {
        //toggle interior objects
        for (int i = 0; i < interiorObjects.Length; i++)
        {
            {
                interiorObjects[i].SetActive(interiorObjectsActive);
            }
        }
        interiorObjectsActive = !interiorObjectsActive;
    }

    public void toggleWayfinding()
    {
        //toggle wayfinding
        for (int i = 0; i < wayfinding.Length; i++)
        {
            {
                wayfinding[i].SetActive(wayfindingActive);
            }
        }
        wayfindingActive = !wayfindingActive;
    }

    public void setMusicVolumeValue()
    {
        if (!musicSlider)
            musicSlider = GameObject.Find("MusicSlider");

        if (musicSlider)
        {
            slider = musicSlider.GetComponent<Slider>();
            musicVolume = slider.value;
        }

        setMusicVolume();
    }

    public void setMusicVolume()
    {
        for (int i = 0; i < musicSources.Length; i++)
        {
            {
                musicSources[i].GetComponent<AudioSource>().volume = musicVolume;
            }
        }


    }
    public void setSoundVolumeValue()
    {
        if (!soundSlider)
            soundSlider = GameObject.Find("SoundSlider");

        if (soundSlider)
        {
            slider = soundSlider.GetComponent<Slider>();
            soundVolume = slider.value;
        }
        setSoundVolume();
    }

    public void setSoundVolume()
    {
        soundSources = GameObject.FindGameObjectsWithTag("Sound");
        for (int i = 0; i < soundSources.Length; i++)
        {
            {
                soundSources[i].GetComponent<AudioSource>().volume = soundVolume;
            }
        }
    }
}
