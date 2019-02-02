using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePointMap : MonoBehaviour
{
    Dictionary<string, float[]> dictionary = new Dictionary<string, float[]>();
    private RectTransform rect;

    public Image mapPoint;

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

    //inverts the alpha channel of the map point. map point can not be disabled like the map itself, because it needs to moved, eventhough its not visible.
    public void invertVisibilityOfMapPoint()
    {
        var tempColor = mapPoint.color;
        tempColor.a = ((mapPoint.color.a == 100) ? 0f : 100f);
        mapPoint.color = tempColor;
    }


        public void movePointMap(string parentName)
    {
        //gets the coordinates from the dictionary
        Vector3 pos;
        pos.x = dictionary[parentName][0];
        pos.y = dictionary[parentName][1];
        pos.z = 0f;
        
        //sets the new position
        rect.anchoredPosition = pos;
    }
}
