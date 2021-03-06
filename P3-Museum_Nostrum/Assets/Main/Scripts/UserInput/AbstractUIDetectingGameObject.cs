﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// enables a GameObject to detect GUI elements.
/// </summary>
/// <remarks>3D GameObjects require access to the canvas' GraphicRaycaster in order to detect GUI elements.</remarks>
public class AbstractUIDetectingGameObject : AbstractInteractiveGameObject
{
    //the required components to raycast GUI elements.
    private static GraphicRaycaster m_Raycaster;
    private static PointerEventData m_PointerEventData;
    private static EventSystem m_EventSystem;

    protected new virtual void Start()
    {
        base.Start();

        InitializeUIDetectionTools();
        //Debug.Log("AbstractUIDetectingGameObject started");

    }

    private void InitializeUIDetectionTools()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GameObject.Find("HUD").GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    /// <summary>
    /// returns alls RaycastResults.
    /// </summary>
    /// <returns></returns>
    protected List<RaycastResult> GetAllUIRaycastResults()
    {
        //source: https://docs.unity3d.com/ScriptReference/UI.GraphicRaycaster.Raycast.html

        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);

        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        return results;
    }

    /// <summary>
    /// returns the all detected GameObjects marked by "yourTag".
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    protected List<GameObject> GetAllUIElementsWith(string tag)
    {
        List<RaycastResult> allRayCastHits = GetAllUIRaycastResults();
        List<GameObject> results = new List<GameObject>();
        for (int i = 0; i < allRayCastHits.Count; ++i)
        {
            // Debug.Log("Hit " + result.gameObject.name);
            if (allRayCastHits[i].gameObject.tag == tag)
            {
                results.Add(allRayCastHits[i].gameObject);
            }

        }
        return results;
    }

    /// <summary>
    /// returns the first detected GameObject marked by "yourTag".
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    protected GameObject GetFirstUIElementWith(string tag)
    {
        List<RaycastResult> allRayCastHits = GetAllUIRaycastResults();
        for (int i = 0; i < allRayCastHits.Count; ++i)
        {
            // Debug.Log("Hit " + result.gameObject.name);
            if (allRayCastHits[i].gameObject.tag == tag)
            {
                return allRayCastHits[i].gameObject;
            }

        }
        return null;
    }
}
