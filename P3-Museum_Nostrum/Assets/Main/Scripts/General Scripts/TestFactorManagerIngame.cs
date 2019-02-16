using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestFactorManagerIngame : MonoBehaviour
{

    //arrays for respective gameObjects
    GameObject[] audioSources;
    GameObject[] lights;
    GameObject[] interiorObjects;
    GameObject[] wayfinding;

    private bool audioActive = false;
    private bool lightsActive = false;
    private bool interiorObjectsActive = false;
    private bool wayfindingActive = false;

    private void Awake()
    {
    }

    //fill the arrays with GameObjects
    void Start()
    {
        audioSources = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        lights = GameObject.FindGameObjectsWithTag("Lights");
        //interiorObjects = GameObject.FindGameObjectsWithTag("interiorObjects");
        //wayfinding = GameObject.FindGameObjectsWithTag("wayfinding");
    }

    public void toggleAudio()
    {
        //toggle audio
        for (int i = 0; i < audioSources.Length; i++)
        {
            {
                audioSources[i].SetActive(audioActive);
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
        Debug.Log("toggleLIgth" + lightsActive);
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
        lightsActive = !lightsActive;
        Debug.Log("toggleLIgth" + lightsActive);
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
                /*for (int i = 0; i < interiorObjects.Length; i++)
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
                }*/

    }

}
