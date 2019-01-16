using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerBodySizeConfigurator : MonoBehaviour {

    [SerializeField]
    [Range(1.4f, 2.0f)][Tooltip("Define how tall the player character is (in meters).")]
    private float bodyHeight = 1.75f;
	

	void Update () {
        if (Application.isPlaying)
        {
            // code executed in play mode
        }
        else
        {
            // code executed in edit mode
            Vector3 cameraPos = Camera.main.transform.position;
            cameraPos.y = bodyHeight;
            Camera.main.transform.position = cameraPos;
        }
        
    }
}
