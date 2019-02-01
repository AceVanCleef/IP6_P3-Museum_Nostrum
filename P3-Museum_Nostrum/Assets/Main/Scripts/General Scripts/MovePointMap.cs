using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePointMap : MonoBehaviour
{
    Dictionary<string, float[]> dictionary = new Dictionary<string, float[]>();
    public RectTransform rect;

    void Start()
    {
        //coordindates uf each room on the ground view
        //the name of each room is used as key
        dictionary.Add("Entrance Hall", new float[] { 130f, 500f });
        dictionary.Add("U Shaped Wing", new float[] { 130f, 400f });
        dictionary.Add("Left Gallery Room", new float[] { 130f, 300f });

        //initialises the RectTransfrom of the point, which indicates the position on the map
        GameObject mapWrapper = GameObject.Find("MapPoint");
        rect = mapWrapper.GetComponent<RectTransform>();
    }

    public void movePointMap(string parentName)
    {
        //gets the coordinates from the dictionary
        Vector3 pos;
        pos.x = dictionary[parentName][0];
        pos.y = dictionary[parentName][1];
        pos.z = 0f;

        //moves the point to the coordinates
        rect.anchoredPosition = pos;
    }
}
