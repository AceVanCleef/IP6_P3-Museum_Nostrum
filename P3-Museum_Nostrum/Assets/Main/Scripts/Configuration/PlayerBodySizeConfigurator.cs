using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerBodySizeConfigurator : MonoBehaviour, ITagEnsurance {

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
            Transform cam = Camera.main.transform;
            Vector3 cameraPos = cam.position;
            if (cam.parent != null)
            {
                cameraPos.y = bodyHeight + cam.parent.transform.position.y;

            }
            else
            {
                cameraPos.y = bodyHeight;
            }
            Camera.main.transform.position = cameraPos;
        }
        
    }

    void Start()
    {
        InitializeTag();
    }

    public void InitializeTag()
    {
        if (gameObject.tag != "Player")
        {
            gameObject.tag = "Player";
        }
    }
}
