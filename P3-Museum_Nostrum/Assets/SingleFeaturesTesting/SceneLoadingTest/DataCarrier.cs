using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataCarrier : MonoBehaviour {

    public static DataCarrier Instance;

    [SerializeField]
    private int intValue = 1;

    public int IntValue { set; get; }

    [SerializeField]
    private bool boolValue;

    public bool BoolValue { set; get; }

    // Use this for initialization
    void Awake () {
		if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // called first
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        InitCurrentSceneValues(scene);
    }

    public void InitCurrentSceneValues(Scene scene)
    {
        if(scene.name == "A")
        {
            InitSceneA();
        }
        if (scene.name == "B")
        {
            InitSceneB();
        }
        if (scene.name == "C")
        {
            InitSceneC();
        }

        //in all scenes expected. If there are several of the same type, use a for loop to iterate over them:
        //Object.FindObjectOfType<AudioSource>().mute = !BoolValue; //prevents constant checking in BGMusicChecker's Update(). But 
                                                                    //...changes in music volume options still have to be updated.
    }

    public void InitSceneA()
    {
        Object.FindObjectOfType<IntSlider>().SetSliderVal(IntValue) ; // slider can't be reached. Execution order: OnEnable() before STart().
    }

    public void InitSceneB()
    {
        Object.FindObjectOfType<IntVal>().SetIntVal(intValue);
    }

    public void InitSceneC()
    {
        Object.FindObjectOfType<IntVal>().SetIntVal(intValue);
    }
}
