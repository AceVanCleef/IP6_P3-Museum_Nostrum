using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//per room
public class ViewDirectionHeatmap : MonoBehaviour {

    private Dictionary<Direction, Material> triangles = new Dictionary<Direction, Material>();
    private Dictionary<Direction, int> directionCounts = new Dictionary<Direction, int>();

    //debugging: visible values in inspector
    public int north = 0;
    public int east = 0;
    public int south = 0;
    public int west = 0;


    // Use this for initialization
    void Start () {
        TriangleInfo[] ti = GetComponentsInChildren<TriangleInfo>();
        for (int i = 0; i < ti.Length; ++i)
        {
            triangles.Add( ti[i].direction, ti[i].material );
            directionCounts.Add(ti[i].direction, 0);
        }
    }
	

    public int IncrementDirectionCount()
    {
        Direction currentDirection = CameraViewDirection.Instance.GetCurrentState().GetDirectionIdentifier();
        directionCounts[currentDirection] += 1;
        UpdateForDebugging();
        return directionCounts[currentDirection];
    }

    private void UpdateForDebugging()
    {
        north = directionCounts[Direction.North];
        east = directionCounts[Direction.East];
        south = directionCounts[Direction.South];
        west = directionCounts[Direction.West];
    }

    public int GetLowestCountOfThisRoom()
    {
        int lowest = directionCounts[Direction.North];
        foreach(KeyValuePair<Direction, int> kvp in directionCounts)
        {
            if (directionCounts[kvp.Key] < lowest) lowest = directionCounts[kvp.Key];
        }
        //Debug.Log("Local lowest: " + lowest + " dirCounts.count: " + directionCounts.Count);
        return lowest;
    }

    public void UpdateColors(int low, int high, int total, HeatMapGradient hmg)
    {
        //source: https://stackoverflow.com/questions/17821828/calculating-heat-map-colours
        //Debug.Log("low: " + low + " - high: " + high);
        int range = high - low;
        //int range = total - high;
        //Debug.Log("range: " + range + " total " + total + " high " + high + " low " + low);
        foreach (KeyValuePair<Direction, Material> kvp in triangles)
        {
            if (directionCounts[kvp.Key] > 0)
            {
                float percent = (directionCounts[kvp.Key] * 1.0f) / range;
               // Debug.Log(percent + " color: " + hmg.Evaluate(percent));
                triangles[kvp.Key].color = hmg.Evaluate(percent);             
            }
        }
    }

    public Dictionary<Direction, int> GetDirectionCounts()
    {
        return directionCounts;
    }
}
