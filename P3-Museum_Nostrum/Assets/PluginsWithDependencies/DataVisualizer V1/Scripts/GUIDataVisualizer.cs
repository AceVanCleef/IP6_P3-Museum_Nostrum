﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// handles user input visualisation such as drag and drop, swipes and touches.
/// </summary>
[RequireComponent(typeof(DataVisualizerManager))]
public class GUIDataVisualizer : MonoBehaviour {

    private static GUIDataVisualizer instance = null;
    /// <summary>
    /// grants access to GUIDataVisualizer
    /// </summary>
    public static GUIDataVisualizer Instance
    {
        get
        {
            return instance;
        }
    }

    private Transform uiInteractionHolder;

    #region SwipeDrawFields
    public GameObject DrawnSwipePrefab;
    private Transform swipeHolder;
    private static int swipeCounter = 0;
    [SerializeField]
    private bool Heatmap2DForSwipes = true;
    [SerializeField]
    private Color swipeColor = Color.yellow;
    #endregion SwipeDrawFields

    #region TouchDrawFields
    public GameObject DrawnTouchPrefab;
    private Transform touchHolder;
    private static int touchCounter = 0;
    [SerializeField]
    private bool Heatmap2DForTouches = true;
    [SerializeField]
    private Color touchColor = Color.cyan;
    #endregion TouchDrawFields

    #region DragFields
    private Transform dragHolder;
    private static int dragCounter = 0;
    [SerializeField]
    private bool Heatmap2DForDrags = true;
    [SerializeField]
    private Color dragColor = Color.magenta;
    #endregion DragFields

    private void Awake()
    {
        //ensure singleton.
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start () {
        uiInteractionHolder = new GameObject("UIInteractionHolder").transform;
        swipeHolder = new GameObject("Swipe Holder").transform;
        swipeHolder.parent = uiInteractionHolder;
        touchHolder = new GameObject("Touch Holder").transform;
        touchHolder.parent = uiInteractionHolder;
        dragHolder = new GameObject("Drag Holder").transform;
        dragHolder.parent = uiInteractionHolder;

        /*
        TestDrawSwipes();
        TestDrawnTouches();
        TestDrawnDrags();*/
        
    }

    #region DrawDrag

    /// <summary>
    /// draws a drag and drop gesture.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public void DrawDrag(Vector2 from, Vector2 to)
    {
        dragCounter++;
        Transform composite = new GameObject("Drag " + dragCounter).transform;
        composite.parent = dragHolder;

        GameObject start = Instantiate(DrawnTouchPrefab, composite);
        start.name = "Start";
        start.GetComponent<DrawnTouchScript>().DrawTouch(from, dragColor);
        start.GetComponent<DrawnTouchScript>().HasToReposition = Heatmap2DForDrags;

        GameObject line = Instantiate(DrawnSwipePrefab, composite);
        line.name = "Dragging line";
        line.GetComponent<DrawnSwipeScript>().DrawSwipe(new SwipeData()
        {
            Direction = SwipeDirection.Down,
            StartPosition = new Vector2(from.x, from.y),
            EndPosition = new Vector2(to.x, to.y)
        }, dragColor);
        line.GetComponent<DrawnSwipeScript>().HasToReposition = Heatmap2DForDrags;

        GameObject end = Instantiate(DrawnTouchPrefab, composite);
        end.name = "End";
        end.GetComponent<DrawnTouchScript>().DrawTouch(to, dragColor);
        end.GetComponent<DrawnTouchScript>().SetRadiusTo(0.1f);
        end.GetComponent<DrawnTouchScript>().HasToReposition = Heatmap2DForDrags;
    }

    public void TestDrawnDrags()
    {
        for (int i = 0; i < 4; ++i)
        {
            float x1 = Random.Range(150f, 270f);
            float y1 = Random.Range(150f, 270f);
            float x2 = Random.Range(175f, 295f);
            float y2 = Random.Range(175f, 295f);
            DrawDrag(new Vector2(x1, y1), new Vector2(x2, y2));
        }
    }

    public void ClearDrags()
    {
        for (int i = dragHolder.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(dragHolder.transform.GetChild(i).gameObject);
        }
    }

    public void ToggleModeOnDrags()
    {
        Heatmap2DForDrags = !Heatmap2DForDrags;
        for (int i = dragHolder.childCount - 1; i >= 0; i--)
        {
            Transform composite = dragHolder.transform.GetChild(i);
            //Must be in same order as instantiated in DrawApproximatedDrag()
            for (int j = 0; j < composite.childCount; ++j)
            {
                GameObject currentChild = composite.GetChild(j).gameObject;
                if (currentChild.name == "Start" ^ currentChild.name == "End")
                {
                    Debug.Log("End or Start?");
                    currentChild.GetComponent<DrawnTouchScript>().HasToReposition = Heatmap2DForDrags;
                }
                else if (currentChild.name == "Dragging line")
                {
                    Debug.Log("dragging line?");
                    currentChild.GetComponent<DrawnSwipeScript>().HasToReposition = Heatmap2DForDrags;
                }
            }
            
        }

    }
    #endregion DrawDrag

    #region Touches

    /// <summary>
    /// draws a touch gesture.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public GameObject DrawTouch(Vector2 pos)
    {
        touchCounter++;
        GameObject go = Instantiate(DrawnTouchPrefab, touchHolder);
        go.name = "Touch " + touchHolder;
        GameObject touch = go.GetComponent<DrawnTouchScript>().DrawTouch(pos, touchColor);
        go.GetComponent<DrawnTouchScript>().HasToReposition = Heatmap2DForTouches;
        return touch;
    }

    public void TestDrawnTouches()
    {
        for (int i = 0; i < 4; ++i)
        {
            float x1 = Random.Range(10f, 120f);
            float y1 = Random.Range(10f, 120f);
            DrawTouch(new Vector2(x1, y1));
        }
    }

    public void ClearTouches()
    {
        for (int i = touchHolder.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(touchHolder.transform.GetChild(i).gameObject);
        }
    }

    public void ToggleModeOnTouches()
    {
        Heatmap2DForTouches = !Heatmap2DForTouches;
        for (int i = touchHolder.childCount - 1; i >= 0; i--)
        {
            touchHolder.transform.GetChild(i).GetComponent<DrawnTouchScript>().HasToReposition = Heatmap2DForTouches;
        }

    }
    #endregion Touches

    #region Swipes

    /// <summary>
    /// draws a swipe gesture.
    /// </summary>
    /// <param name="data"></param>
    public void DrawSwipe(SwipeData data)
    {
        swipeCounter++;
        GameObject go = Instantiate(DrawnSwipePrefab, swipeHolder);
        go.name = "Swipe " + swipeCounter;
        go.GetComponent<DrawnSwipeScript>().DrawSwipe(data, swipeColor);
        go.GetComponent<DrawnSwipeScript>().HasToReposition = Heatmap2DForSwipes;
    }

    public void TestDrawSwipes()
    {
        for(int i = 0; i < 4; ++i)
        {
            float x1 = Random.Range(10f, 120f);
            float y1 = Random.Range(10f, 120f);
            float x2 = Random.Range(25f, 155f);
            float y2 = Random.Range(25f, 155f);
            DrawSwipe(new SwipeData()
            {
                Direction = SwipeDirection.Right,
                StartPosition = new Vector2(x1, y1),
                EndPosition = new Vector2(x2, y2)
            });
        }
    }

    public void ClearSwipes()
    {
        for (int i = swipeHolder.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(swipeHolder.transform.GetChild(i).gameObject);
        }
    }

    public void ToggleModeOnSwipes()
    {
        Heatmap2DForSwipes = !Heatmap2DForSwipes;
        for (int i = swipeHolder.childCount - 1; i >= 0; i--)
        {
            swipeHolder.transform.GetChild(i).GetComponent<DrawnSwipeScript>().HasToReposition = Heatmap2DForSwipes;
        }

    }

    #endregion Swipes

}


[CustomEditor(typeof(GUIDataVisualizer))]
public class GUIDataVisualizerCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUIDataVisualizer myScript = (GUIDataVisualizer)target;
        if (GUILayout.Button("Toggle GUIMode for TouchPoints"))
        {
            myScript.ToggleModeOnTouches();
        }
        if (GUILayout.Button("Toggle GUIMode for Swipes"))
        {
            myScript.ToggleModeOnSwipes();
        }
        if (GUILayout.Button("Toggle GUIMode for Drags"))
        {
            myScript.ToggleModeOnDrags();
        }

        if (GUILayout.Button("Clear TouchPoints"))
        {
            myScript.ClearTouches();
        }
        if (GUILayout.Button("Clear Swipes"))
        {
            myScript.ClearSwipes();
        }
        if (GUILayout.Button("Clear Drags"))
        {
            myScript.ClearDrags();
        }
    }
}