using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleInfo : MonoBehaviour {

    public Direction direction;
    //[HideInInspector]
    public Material material;

	// Use this for initialization
	void Awake () {
        material = GetComponent<Renderer>().material;
	}
}