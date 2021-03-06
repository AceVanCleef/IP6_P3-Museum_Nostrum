﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    }
}