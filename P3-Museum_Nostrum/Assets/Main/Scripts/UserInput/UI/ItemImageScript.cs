using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemImageScript : MonoBehaviour, IOnTap {

    public void OnTap()
    {
        Debug.Log("Tapped on " + transform.name);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
