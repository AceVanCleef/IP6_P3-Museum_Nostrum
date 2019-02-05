using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMasterVolumeStartmenu : MonoBehaviour {

    public AudioOptionsManager audioOptionsManager;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        AudioListener.volume = audioOptionsManager.masterVolume;
    }
}
