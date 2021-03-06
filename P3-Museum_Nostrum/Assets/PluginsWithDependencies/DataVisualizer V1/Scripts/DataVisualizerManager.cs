﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// handels data visualisation of player behavior.
/// </summary>
[RequireComponent(typeof(GUIDataVisualizer))]
public class DataVisualizerManager : MonoBehaviour {


    private static DataVisualizerManager instance;
    /// <summary>
    /// grants access to this DataVisualizerManager.
    /// </summary>
    public static DataVisualizerManager Instance
    {
        get
        {
            return instance;
        }
    }

    private static GUIDataVisualizer guiDV;
    /// <summary>
    /// grants access to user input visualisation tools.
    /// </summary>
    public static GUIDataVisualizer GUI
    {
        get
        {
            return guiDV;
        }
    }


    private List<GameObject> allRooms = new List<GameObject>();
    private GameObject CurrentRoom;
    private GameObject player;

    #region ViewDirectionHeatmapFields
    public GameObject ViewDirectionHeatmapPrefab;
    private HeatMapGradient HMG;
    private int totalViewDirectionChanges = 0;
    //boundary count values of triangles.
    private int highest = 0;
    private int lowest = 0;
    #endregion ViewDirectionHeatmapFields

    #region LineRendererFields
    public GameObject LineRendererPrefab;
    [Tooltip("Sets the transform.position.y value of lines.")]
    public float elevationOfLinesAboveFloor = 0.25f;
    private Transform lineRendererHolder;
    private List<LineRendererScript> allLineRenderers = new List<LineRendererScript>();
    //                  start   list of end pos (neighbouring rooms)
    private Dictionary<Vector3, List<Vector3>> lineRendererIndex = new Dictionary<Vector3, List<Vector3>>();
    #endregion LineRendererFields


    #region RoomHeatNodeFields
    public GameObject RoomHeatNodePrefab;
    public float elevationOfRoomHeatNodesAboveFloor = 0.25f;
    #endregion RoomHeatNodeFields

    void Awake()
    {
        //ensure singleton.
        if (instance == null)
        {
            instance = this;
            guiDV = GetComponent<GUIDataVisualizer>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        //Find the player.
        player = GameObject.FindGameObjectWithTag("Player");

        //Find all rooms in the current scene.
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        for(int i = 0; i < rooms.Length; ++i)
        {
            allRooms.Add(rooms[i]);
        }
        //find the room where the player is positioned in.
        CurrentRoom = GetCurrentRoom();
        Debug.Log("CurrentRoom: " + CurrentRoom.name);

        //Prepare components for line renderers.
        HMG = new HeatMapGradient();
        lineRendererHolder = new GameObject("LineRendererHolder").transform;


        InstantiateFloorHeatmaps();
        InstantiateRoomHeatNodes();

        //Prepare connection to GUIDataVisualizer
        gdv = GetComponent<GUIDataVisualizer>();
    }

    /// <summary>
    /// finds the current room where the player is positioned at (transform.position based comparison).
    /// </summary>
    /// <returns></returns>
    private GameObject GetCurrentRoom()
    {
        return allRooms.Find(room => Vector3.Distance(player.transform.position, room.transform.position) < 0.1f);
        //Note: if you get null pointer exceptionts due to the distance comparison, you might want to adjust 
        //      the comparison value (of 0.1f).
        //Note 2: This might be the case when the values logged by DataLogger are rounded.
        //Note 3: This might be the case when you log vectore values using .ToString(). E.g. transform.position.ToString();
    }

    //todo: remove redundant code:
    #region GUIHeatmap

    private GUIDataVisualizer gdv;
    public GUIDataVisualizer GetGUIDataVisualizer()
    {
        return gdv;
    }

    #endregion GUIHeatmap

    // ---------------------------- LineRenderer API -----------------------
    #region LineRendererAPI

    /// <summary>
    /// draws a line between start and end or increases its width if it already exists.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void TraceLineBetween(Vector3 start, Vector3 end)
    {
        //ensure line is visible above floor.
        start.y += elevationOfLinesAboveFloor;
        end.y += elevationOfLinesAboveFloor;

        bool lineAdded = AddLineBetween(start, end);
        if (!lineAdded)
        {
            IncrementWidthBetween(start, end);
        }
    }

    /// <summary>
    /// draws a line and ensures each is unique.
    /// </summary>
    /// <param name="start">player's original position</param>
    /// <param name="end">player's end position</param>
    private bool AddLineBetween(Vector3 start, Vector3 end)
    {
        //contains check (bidirectionl). Note: for unidirectional, remove '&& IsUniqeuLine(end, start)'.
        if (IsUniqeuLine(start, end) && IsUniqeuLine(end, start))
        {
            GameObject lrObj = Instantiate(LineRendererPrefab, lineRendererHolder);
            LineRendererScript lrs = lrObj.GetComponent<LineRendererScript>();
            lrs.Initialize(start, end);
            allLineRenderers.Add(lrs);
            //add to lineRendererIndex
            if (lineRendererIndex.ContainsKey(start))
            {
                lineRendererIndex[start].Add(end);
            }
            else
            {
                List<Vector3> neighbouringRooms = new List<Vector3>();
                neighbouringRooms.Add(end);
                lineRendererIndex.Add(start, neighbouringRooms);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// checks whether start is already used and if so, whether end is already used in combination with it.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private bool IsUniqeuLine(Vector3 start, Vector3 end)
    {
        return !lineRendererIndex.ContainsKey(start) || !lineRendererIndex[start].Contains(end);
    }

    private bool IncrementWidthBetween(Vector3 startPos, Vector3 endPos)
    {
        //bidirectionl check. Note: for unidirectional, remove '^ lr.IsLineBetween(endPos, startPos)'.
        LineRendererScript lrs = allLineRenderers.Find(lr => lr.IsLineBetween(startPos, endPos) ^ lr.IsLineBetween(endPos, startPos));
        if (lrs != null) lrs.IncrementWidth();
        return lrs != null;
    }
    #endregion LineRendererAPI

    // ---------------------------- ViewDirection Floor Heatmap API -----------------------
    #region ViewDirectionFloorHeatmapAPI

    private void InstantiateFloorHeatmaps()
    {
        for (int i = 0; i < allRooms.Count; ++i)
        {
            GameObject vdh = Instantiate(ViewDirectionHeatmapPrefab, allRooms[i].transform);
            RoomConfigurator currentRC = allRooms[i].GetComponent<RoomConfigurator>();
            TriangleInfo[] ti = vdh.GetComponentsInChildren<TriangleInfo>();
            for (int j = 0; j < ti.Length; ++j)
            {
                ti[j].AdjustTriangleDimensionsTo(currentRC);
            }
        }
    }

    private ViewDirectionHeatmap GetCurrentViewDirectionHeatmap()
    {
        return CurrentRoom.GetComponentInChildren<ViewDirectionHeatmap>();
    }

    /// <summary>
    /// updates the ViewDirectionHeatmaps.
    /// </summary>
    public void AfterViewDirectionChange()
    {
        totalViewDirectionChanges++;
        int changedCount = GetCurrentViewDirectionHeatmap().IncrementDirectionCount();
        if (changedCount > highest) highest = changedCount;
        UpdateLowest();
        //Update all heatmaps.
        foreach (GameObject room in allRooms)
        {
            ViewDirectionHeatmap vdh = room.GetComponentInChildren<ViewDirectionHeatmap>();
            vdh.UpdateColors(lowest, highest, totalViewDirectionChanges, HMG);
        }
    }
    
    private void UpdateLowest()
    {
        lowest = allRooms[0].GetComponentInChildren<ViewDirectionHeatmap>().GetLowestCountOfThisRoom();
        for (int i = 1; i < allRooms.Count; ++i)
        {
            ViewDirectionHeatmap vdh = allRooms[1].GetComponentInChildren<ViewDirectionHeatmap>();
            int localLowest = vdh.GetLowestCountOfThisRoom();
            if (localLowest < lowest) lowest = localLowest;
        }
    }

    #endregion ViewDirectionFloorHeatmapAPI


    // ---------------------------- RoomHeatNode API -----------------------
    #region RoomHeatNodeAPI
    private void InstantiateRoomHeatNodes()
    {
        for (int i = 0; i < allRooms.Count; ++i)
        {
            GameObject rhn = Instantiate(RoomHeatNodePrefab, allRooms[i].transform);
            //ensure line is visible above floor.
            Vector3 pos = rhn.transform.localPosition;
            pos.y += elevationOfRoomHeatNodesAboveFloor;
            rhn.transform.localPosition = pos;
        }
    }


    private RoomFrequencyNode GetCurrentRoomFrequenceyNode()
    {
        return CurrentRoom.GetComponentInChildren<RoomFrequencyNode>();
    }


    //used in ReplayManager (or DoorScript)
    /// <summary>
    /// updates the DataVisualizer regarding the player's current position.
    /// </summary>
    public void PlayerEnteredNewRoom()
    {
        UpdateCurrentRoom();
        IncrementRoomCount();
    }

    private void UpdateCurrentRoom()
    {
        CurrentRoom = GetCurrentRoom();
    }

    private void IncrementRoomCount()
    {
        GetCurrentRoomFrequenceyNode().PlayerEnteredRoom();
    }
    #endregion RoomHeatNodeAPI

    // ---------------------------- Custom Inspector Functionality -----------------------
    public void ToggleVisibilityInCamera()
    {
        Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer("DataVisualization");
    }

}

[CustomEditor(typeof(DataVisualizerManager))]
public class DataVisualizerManagerCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DataVisualizerManager dvm = (DataVisualizerManager)target;
        if (GUILayout.Button("Hide/Show DataVisualisation from player"))
        {
            dvm.ToggleVisibilityInCamera();
        }
        
    }
}