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
    GameObject[] interiorFurnishingHolders;
    GameObject[] signPostingHolders;

    private bool audioBreadcrumbsActive = false;
    private bool lightsActive = false;
    private bool interiorFurnishingActive = false;
    private bool signPostingActive = false;

    private float masterVolume;
    private float musicVolume;
    private float soundVolume;


    private GameObject[] soundSources;

    Slider masterVolumeSlider;
    Slider musicSlider;
    Slider soundSlider;

    Toggle audioToggle;
    Toggle lightsToggle;
    Toggle interiorToggle;
    Toggle wayfindingToggle;

    void Awake()
    {
        //default values
        masterVolume = 1;
        musicVolume = 1;
        soundVolume = 1;
        //Get values from other scenes set by player.
        if (AppData.Instance)
        {
            //audio start values.
            masterVolume = AppData.AudioSettings.masterVolume;
            musicVolume = AppData.AudioSettings.musicVolume;
            soundVolume = AppData.AudioSettings.soundVolume;
            //test factor start values.
            audioBreadcrumbsActive = AppData.TestFactorSettings.audioBreadcrumsAudible;
            lightsActive = AppData.TestFactorSettings.lightsShining;
            interiorFurnishingActive = AppData.TestFactorSettings.interiorFurnishingVisible;
            signPostingActive = AppData.TestFactorSettings.signPostsVisible;
        }



    }
    void Start()
    {
        musicSources = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        lights = GameObject.FindGameObjectsWithTag("Lights");
        interiorFurnishingHolders = GameObject.FindGameObjectsWithTag("InteriorHolder");
        signPostingHolders = GameObject.FindGameObjectsWithTag("WayfindingHolder");
        soundSources = GameObject.FindGameObjectsWithTag("Sound");

        Slider[] sliders = GetComponentsInChildren<Slider>();
        Debug.Log(sliders.Length);
        for (int i = 0; i < sliders.Length; ++i)
        {
            if (sliders[i].name == "MasterSlider") masterVolumeSlider = sliders[i];
            if (sliders[i].name == "MusicSlider") musicSlider = sliders[i];
            if (sliders[i].name == "SoundSlider") soundSlider = sliders[i];
        }

        Toggle[] toggles = GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; ++i)
        {
            if (toggles[i].name == "Audio") audioToggle = toggles[i];
            if (toggles[i].name == "Light") lightsToggle = toggles[i];
            if (toggles[i].name == "Interior") interiorToggle = toggles[i];
            if (toggles[i].name == "WayFinding") wayfindingToggle = toggles[i];
        }

        InitGameObjectValues(); //Must come first.
        InitGUIElements();      //Must come second.
    }

    private void InitGameObjectValues()
    {
        MainCameraConfigurator mCC = Camera.main.GetComponent<MainCameraConfigurator>(); //replaced Gameobject.Find("Main Camera"), da fehleranfällig.

        mCC.setMasterVolume();
        setMusicVolume();
        setSoundVolume();
        SetInteriorVisbility();
        SetAudioBreadcrumsAudability();
        SetLights();
        SetWayfindingVisibility();
    }

    private void InitGUIElements()
    {
        if (AppData.Instance)
        {
            masterVolumeSlider.value = AppData.AudioSettings.masterVolume;
            musicSlider.value = AppData.AudioSettings.musicVolume;
            soundSlider.value = AppData.AudioSettings.soundVolume;

            audioToggle.isOn = AppData.TestFactorSettings.audioBreadcrumsAudible;
            lightsToggle.isOn = AppData.TestFactorSettings.lightsShining;
            interiorToggle.isOn = AppData.TestFactorSettings.interiorFurnishingVisible;
            wayfindingToggle.isOn = AppData.TestFactorSettings.signPostsVisible;
        }
        //Options panel is expected to be active when scene is being loaded.
        GameObject.Find("Options").SetActive(false);
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
            LoadScene("StartMenu");
        }
    }

    public void startGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "StartMenu")
        {
            LoadScene("EntranceAnimation");
        }
    }


    #region LoadingScenes
    /// <summary>
    /// loads a scene asynchronously.
    /// </summary>
    /// <param name="scenename">The name of the scene which has to be loaded.</param>
    public void LoadScene(string scenename)
    {
        StartCoroutine(LoadSceneAsync(scenename));
    }

    IEnumerator LoadSceneAsync(string scenename)
    {
        // "The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens."
        // Source: https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadSceneAsync.html

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenename);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    #endregion LoadingScenes

    public void toggleAudio()
    {
        //toggle audio
        SetAudioBreadcrumsAudability();
        audioBreadcrumbsActive = !audioBreadcrumbsActive;
    }

    private void SetAudioBreadcrumsAudability()
    {
        Debug.Log("SetAudioBC: " + audioBreadcrumbsActive);
        for (int i = 0; i < musicSources.Length; i++)
        {
            {
                musicSources[i].SetActive(audioBreadcrumbsActive);
            }
        }
    }

    public void toggleLights()
    {
        //toggle lights
        SetLights();
        lightsActive = !lightsActive;
    }

    private void SetLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            {
                lights[i].SetActive(lightsActive);
            }
        }
    }

    public void toggleInterior()
    {
        //toggle interior objects
        SetInteriorVisbility();
        interiorFurnishingActive = !interiorFurnishingActive;
    }

    private void SetInteriorVisbility()
    {
        for (int i = 0; i < interiorFurnishingHolders.Length; i++)
        {
            {
                interiorFurnishingHolders[i].SetActive(interiorFurnishingActive);
            }
        }
    }

    public void toggleWayfinding()
    {
        //toggle wayfinding
        SetWayfindingVisibility();
        signPostingActive = !signPostingActive;
    }

    private void SetWayfindingVisibility()
    {
        for (int i = 0; i < signPostingHolders.Length; i++)
        {
            {
                signPostingHolders[i].SetActive(signPostingActive);
            }
        }
    }

    public void setMusicVolumeValue()
    {
        musicVolume = musicSlider.value;

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
        soundVolume = soundSlider.value;
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

    private void OnDisable()
    {
        Debug.Log("OnDisable in IngameOptionsManager");
        if (AppData.Instance)
        {
            //store audio settings.
            AppData.AudioSettings.masterVolume = masterVolume;
            AppData.AudioSettings.musicVolume = musicVolume;
            AppData.AudioSettings.soundVolume = soundVolume;
            //store test factor settings.
            AppData.TestFactorSettings.audioBreadcrumsAudible = audioBreadcrumbsActive;
            AppData.TestFactorSettings.lightsShining = lightsActive;
            AppData.TestFactorSettings.interiorFurnishingVisible = interiorFurnishingActive;
            AppData.TestFactorSettings.signPostsVisible = signPostingActive;
        }
        Debug.Log(AppData.AudioSettings.masterVolume + " - " + AppData.AudioSettings.musicVolume + " - " + AppData.AudioSettings.soundVolume);
        Debug.Log(AppData.TestFactorSettings.audioBreadcrumsAudible + " - " + AppData.TestFactorSettings.lightsShining + " - " + AppData.TestFactorSettings.interiorFurnishingVisible + " - " + AppData.TestFactorSettings.signPostsVisible);
    }
}
