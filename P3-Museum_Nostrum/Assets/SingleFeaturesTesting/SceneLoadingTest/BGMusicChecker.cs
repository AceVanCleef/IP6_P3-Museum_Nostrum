using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicChecker : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        Debug.Log("DataCarrier.Instance found? " + (DataCarrier.Instance != null));
        if (DataCarrier.Instance)
        {
            GetComponent<AudioSource>().mute = !DataCarrier.Instance.BoolValue;
        }
	}
}
