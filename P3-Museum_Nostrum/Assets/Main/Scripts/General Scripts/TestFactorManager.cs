using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFactorManager : MonoBehaviour
{

    //arrays for respective gameObjects
    GameObject[] audioSources;
    GameObject[] lights;
    GameObject[] interiorObjects;
    GameObject[] wayfinding;

    [SerializeField]
    private bool audioActive = true;
    [SerializeField]
    private bool lightsActive = true;
    [SerializeField]
    private bool interiorObjectsActive = true;
    [SerializeField]
    private bool wayfindingActive = true;


    private static TestFactorManager instance;

    private void Awake()
    {
        //guarantee singleton.
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            instance.gameObject.name = "TestFactorManager";
        }
    }

    //fill the arrays with GameObjects
    void Start()
    {
        audioSources = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        lights = GameObject.FindGameObjectsWithTag("Lights");
        //interiorObjects = GameObject.FindGameObjectsWithTag("interiorObjects");
        //wayfinding = GameObject.FindGameObjectsWithTag("wayfinding");
    }

    //is executed when button "apply" in the inspector is clicked
    public void Apply()
    {
        //toggle audio
        for (int i = 0; i < audioSources.Length; i++)
        {
            {
                audioSources[i].SetActive(audioActive);
            }
        }

        //toggle lights
        for (int i = 0; i < lights.Length; i++)
        {
            {
                lights[i].SetActive(lightsActive);
            }
        }

/*  Todo: null pointers if no object with required tag exists in scene.
  
        //toggle interior objects
        for (int i = 0; i < interiorObjects.Length; i++)
        {
            {
                interiorObjects[i].SetActive(interiorObjectsActive);
            }
        }

        //toggle wayfinding
        for (int i = 0; i < wayfinding.Length; i++)
        {
            {
                wayfinding[i].SetActive(wayfindingActive);
            }
        }
        */

    }
}
