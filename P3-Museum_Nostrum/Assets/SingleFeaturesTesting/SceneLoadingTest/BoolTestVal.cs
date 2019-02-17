using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BoolTestVal : MonoBehaviour {

    Toggle t;

	// Use this for initialization
	void Start () {
        t = GetComponent<Toggle>();
	}
	
	// Update is called once per frame
	void Update () {
        DataCarrier.Instance.BoolValue = t.isOn;
	}
}
