using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    void Awake()
    {
        InitializeTag();
    }

    public void InitializeTag()
    {
        if (gameObject.tag != "Map")
        {
            gameObject.tag = "Map";
        }
    }
}
