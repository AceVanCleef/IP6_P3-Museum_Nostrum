using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassScript : MonoBehaviour {

    /// <summary>
    /// stores the source of the rotation.
    /// </summary>
    private Transform playerTransform;

    //the target of the rotation.
    /// <summary>
    /// stores the target of the rotation.
    /// </summary>
    private RectTransform directionMarker;

    [Tooltip("Defines how many degrees north offsets from 0°.")]
    public float Offset = 0f;

    [Tooltip("Adjust the rotation to clockwise or counter-clockwise. " +
        "Note: must be set up before launching the game.")]
    public bool invertRotationDirection = true;
    private float invertValue;  //either 1f or -1f.


    void Start () {
        //Get the source of the rotation, namely the player GameObject.
        playerTransform = Camera.main.GetComponent<Transform>();

        //Find RectTransform of image named "Direction needle"...
        RectTransform[] rectTransforms = GetComponentsInChildren<RectTransform>();
        int i = 0;
        while (i < rectTransforms.Length && rectTransforms[i].name != "Background")
        {
            ++i;
        }
        //...and allocate the target for the rotation.
        directionMarker = rectTransforms[i];

        if (invertRotationDirection)
        {
            invertValue = -1f;
        }
        else
        {
            invertValue = 1f;
        }
	}
	
	void Update () {
        RotateCompass();
    }

    private void RotateCompass()
    {
        Vector3 compassRotation = directionMarker.rotation.eulerAngles;
        compassRotation.z = CalculateAngle() * invertValue;
        directionMarker.rotation = Quaternion.Euler(compassRotation);
    }
    
    private float CalculateAngle()
    {
        return playerTransform.rotation.eulerAngles.y + Offset;
    }    
}
