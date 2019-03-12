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

    GameObject compass;
    GameObject mapOfGameLevel;

    private bool audioBreadcrumbsActive = true;
    private bool lightsActive = true;
    private bool interiorFurnishingActive = true;
    private bool signPostingActive = true;
    private bool compassActive = true;
    private bool mapActive = true;

    private float masterVolume;
    private float musicVolume;
    private float soundVolume;


    private GameObject[] soundSources;

    [Header("Child GameObject called 'Options'")]
    public GameObject optionsGO;

    [Space(10)]

    //GameObject references from Inspector
    [Header("Volume Sliders")]
    public GameObject masterVolumeSliderGO;
    public GameObject musicSliderGO;
    public GameObject soundSliderGO;

    [Space(10)]

    [Header("Test Factor Toggles")]
    public GameObject audioToggleGO;
    public GameObject lightsToggleGO;
    public GameObject interiorToggleGO;
    public GameObject signPostingsToggleGO;
    public GameObject compassToggleGO;
    public GameObject mapToggleGO;

    //script references
    Slider masterVolumeSlider;
    Slider musicSlider;
    Slider soundSlider;

    Toggle audioToggle;
    Toggle lightsToggle;
    Toggle interiorToggle;
    Toggle signPostingsToggle;
    Toggle compassToggle;
    Toggle mapToggle;

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
            compassActive = AppData.TestFactorSettings.compassAvailable;
            mapActive = AppData.TestFactorSettings.mapAvailable;
        }

    }
    void Start()
    {
        FindObjectsInCurrentScene();    //Must come first.
        InitGameObjectValues();         //Must come second.
        InitGUIElements();              //Must come third.
        RegisterToggleEventHandlers();  //Must come fourth.
    }

    #region Initialization

    private void FindObjectsInCurrentScene()
    {
        musicSources        = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        lights              = GameObject.FindGameObjectsWithTag("Lights");
        interiorFurnishingHolders = GameObject.FindGameObjectsWithTag("InteriorHolder");
        signPostingHolders  = GameObject.FindGameObjectsWithTag("WayfindingHolder");
        soundSources        = GameObject.FindGameObjectsWithTag("Sound");
        CompassScript cs    = UnityEngine.Object.FindObjectOfType<CompassScript>();
        Map mpm    = UnityEngine.Object.FindObjectOfType<Map>();

        if (cs)             compass = cs.gameObject;
        if (mpm)            mapOfGameLevel = mpm.gameObject;
    }

    private void InitGameObjectValues()
    {
        MainCameraConfigurator mCC = Camera.main.GetComponent<MainCameraConfigurator>(); //replaced Gameobject.Find("Main Camera"), da fehleranfällig.

        mCC.setMasterVolume();
        SetMusicVolume();
        SetSoundVolume();
        SetInteriorVisbility();
        SetAudioBreadcrumsAudability();
        SetLights();
        SetWayfindingVisibility();
        SetCompassVisibility();
        SetMapVisibility();
    }

    private void InitGUIElements()
    {
        //retrieve UI elements
        masterVolumeSlider  = masterVolumeSliderGO.GetComponent<Slider>();
        musicSlider         = musicSliderGO.GetComponent<Slider>();
        soundSlider         = soundSliderGO.GetComponent<Slider>();

        audioToggle         = audioToggleGO.GetComponent<Toggle>();
        lightsToggle        = lightsToggleGO.GetComponent<Toggle>();
        interiorToggle      = interiorToggleGO.GetComponent<Toggle>();
        signPostingsToggle    = signPostingsToggleGO.GetComponent<Toggle>();
        compassToggle       = compassToggleGO.GetComponent<Toggle>();
        mapToggle           = mapToggleGO.GetComponent<Toggle>();

        //set values
        masterVolumeSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;

        //Note: setting a value into isOn property will trigger a value change event.
        //      Therefore, event handlers have to be registered afterwards.
        //      Otherwise, state and UI representation will be out of sync.
        //Read more: https://forum.unity.com/threads/change-the-value-of-a-toggle-without-triggering-onvaluechanged.275056/

        audioToggle.onValueChanged.RemoveAllListeners();
        lightsToggle.onValueChanged.RemoveAllListeners();
        interiorToggle.onValueChanged.RemoveAllListeners();
        signPostingsToggle.onValueChanged.RemoveAllListeners();
        compassToggle.onValueChanged.RemoveAllListeners();
        mapToggle.onValueChanged.RemoveAllListeners();


        audioToggle.isOn = audioBreadcrumbsActive;
        lightsToggle.isOn = lightsActive;
        interiorToggle.isOn = interiorFurnishingActive;
        signPostingsToggle.isOn = signPostingActive;
        compassToggle.isOn = compassActive;
        mapToggle.isOn = mapActive;

        //Options panel is expected to be active when scene is being loaded.
        optionsGO.SetActive(false);
    }

    private void RegisterToggleEventHandlers()
    {
        //Must be done via code after toggle.isOn values are set!
        //Read more: https://forum.unity.com/threads/change-the-value-of-a-toggle-without-triggering-onvaluechanged.275056/

        audioToggle.onValueChanged.AddListener(delegate { toggleAudio(); });

        lightsToggle.onValueChanged.AddListener(delegate { toggleLights(); });

        interiorToggle.onValueChanged.AddListener(delegate { toggleInterior(); });

        signPostingsToggle.onValueChanged.AddListener(delegate { toggleWayfinding(); });

        compassToggle.onValueChanged.AddListener(delegate { toggleCompassVisibility(); });

        mapToggle.onValueChanged.AddListener( delegate { toggleMapVisibility(); });
    }

    #endregion Initialization



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
            if (DataLogger.Instance)
            {
                DataLogger.Instance.Log("endGame", "Thank you for playing ;-)");
                DataLogger.Instance.PrintDataList();
            }
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


    #region TestFactorCallbacks

    public void toggleAudio()
    {
        //toggle audio
        audioBreadcrumbsActive = !audioBreadcrumbsActive;
        SetAudioBreadcrumsAudability();
    }

    private void SetAudioBreadcrumsAudability()
    {
        if (DataLogger.Instance)
            DataLogger.Instance.Log("setAudioBreadcrumsAudability", audioBreadcrumbsActive.ToString());
        for (int i = 0; i < musicSources.Length; i++)
        {
            {
                musicSources[i].SetActive(audioBreadcrumbsActive);
            }
        }
    }

    public void toggleLights()
    {
        lightsActive = !lightsActive;
        SetLights();
    }

    private void SetLights()
    {
        if (DataLogger.Instance)
            DataLogger.Instance.Log("setLights", lightsActive.ToString());
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
        interiorFurnishingActive = !interiorFurnishingActive;
        SetInteriorVisbility();
    }

    private void SetInteriorVisbility()
    {
        if (DataLogger.Instance)
            DataLogger.Instance.Log("setInteriorVisbility", interiorFurnishingActive.ToString());
        for (int i = 0; i < interiorFurnishingHolders.Length; i++)
        {
            {
                interiorFurnishingHolders[i].SetActive(interiorFurnishingActive);
            }
        }
    }

    public void toggleWayfinding()
    {
        signPostingActive = !signPostingActive;
        SetWayfindingVisibility();
    }

    private void SetWayfindingVisibility()
    {
        if (DataLogger.Instance)
            DataLogger.Instance.Log("setWayfindingVisibility", signPostingActive.ToString());
        for (int i = 0; i < signPostingHolders.Length; i++)
        {
            {
                signPostingHolders[i].SetActive(signPostingActive);
            }
        }
    }

    public void toggleCompassVisibility()
    {
        compassActive = !compassActive;
        SetCompassVisibility();
    }

    public void SetCompassVisibility()
    {
        if (DataLogger.Instance)
            DataLogger.Instance.Log("setCompassVisibility", compassActive.ToString());
        if (compass)
            compass.SetActive(compassActive);
    }

    public void toggleMapVisibility()
    {
        mapActive = !mapActive;
        SetMapVisibility();
    }

    public void SetMapVisibility()
    {
        if (DataLogger.Instance)
            DataLogger.Instance.Log("setMapVisibility", mapActive.ToString());
        if (mapOfGameLevel)
            mapOfGameLevel.SetActive(mapActive);
    }

    #endregion TestFactorCallbacks

    #region AudioCallBacks

    public void SetMusicVolumeValue()
    {
        musicVolume = musicSlider.value;

        SetMusicVolume();
    }

    public void SetMusicVolume()
    {
        if (DataLogger.Instance)
            DataLogger.Instance.Log("setMusicVolume", musicVolume.ToString());
        for (int i = 0; i < musicSources.Length; i++)
        {
            {
                musicSources[i].GetComponent<AudioSource>().volume = musicVolume;
            }
        }


    }
    public void SetSoundVolumeValue()
    {
        soundVolume = soundSlider.value;
        SetSoundVolume();
    }

    public void SetSoundVolume()
    {
        if (DataLogger.Instance)
            DataLogger.Instance.Log("setSoundVolume", soundVolume.ToString());
        soundSources = GameObject.FindGameObjectsWithTag("Sound");
        for (int i = 0; i < soundSources.Length; i++)
        {
            {
                soundSources[i].GetComponent<AudioSource>().volume = soundVolume;
            }
        }
    }

    #endregion AudioCallBacks

    private void OnDisable()
    {
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
            AppData.TestFactorSettings.compassAvailable = compassActive;
            AppData.TestFactorSettings.mapAvailable = mapActive;
        }
    }
}
