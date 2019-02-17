using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntSlider : MonoBehaviour {

    Slider s;
    bool loading = true;

	// Use this for initialization
	void Awake () {
        s = GetComponent<Slider>();
        Debug.Log("Found datacarrier?" + (DataCarrier.Instance != null));

    }

    // Update is called once per frame
    void Update () {
        DataCarrier.Instance.IntValue = (int) s.value;
	}

    public void SetSliderVal(int val)
    {
        s.value = val;
    }
}
