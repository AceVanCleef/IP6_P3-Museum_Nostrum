using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InputManager : AbstractInputManager {

    public static InputManager Instance = null;

	// Use this for initialization
	void Start () {
		if (Instance == null)
        {
            Instance = this;
        }
	}
	
	// Update is called once per frame
	void Update () {
		//Todo: Zwischen Tap, Swipe und Drag unterscheiden.

        if (Input.touchCount != 1)
        {
            return;
        }

        if (IsTap()) Debug.Log("Tapping");
	}

    private bool IsTap()
    {

        return false;
    }
}
