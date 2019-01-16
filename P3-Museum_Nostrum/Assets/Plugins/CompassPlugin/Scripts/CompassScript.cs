using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassScript : MonoBehaviour {

    /// <summary>
    /// Conceptual idea: rotate compass image "direction needle" according to main camera rotation. 
    /// Since the main camera is a child of player GO, the player GO's transform.rotation.y value has 
    /// to be taken for evaluating in which direction the player is currently looking at (N, NE, E, SE,...).
    /// 
    /// Default values:
    /// 0° = North
    /// 90° = East
    /// 100° = South
    /// 270° = West
    /// 
    /// Since n * 360° + 90° = East, the calculation has to be made using modulo.
    /// E.g. 
    /// 450° mod 360° = 90°
    /// 900° mod 360° = 180°
    /// ...
    /// For negative values:
    /// -90° mod 360° = -90° => 270°
    /// -450° mod 360° = -90° => 270°
    /// ...these values have to be substracted from 360° to evaluate the positive value.
    /// 
    /// 
    /// Customization feature: Manually define what angle is considered north, east, south and west:
    /// 0° + Offset = North
    /// 90° + Offset = East
    /// 100° + Offset = South
    /// 270° + Offset = West
    /// 
    /// The image "direction needles" angle can be set by setting its RectTransform.rotation.z value.
    /// See RotateCompass() for its implementation. Unity offers a transformation between Quaternions and Euler Angles thus
    /// enabling us to rotate using degrees.
    /// When using Euler angles, the modulo calculation will be handled by Unity.
    /// </summary>
    /// 

    //the source of the rotation.
    private Transform playerTransform;

    //the target of the rotation.
    private RectTransform directionNeedleRT;

    [Tooltip("Defines how many degrees north offsets from 0°.")]
    public float Offset = 0f;

    void Start () {
        //Get the source of the rotation, namely the player GameObject.
        playerTransform = Camera.main.GetComponentInParent<Transform>();

        //Find RectTransform of image named "Direction needle"...
        RectTransform[] rectTransforms = GetComponentsInChildren<RectTransform>();
        int i = 0;
        while (i < rectTransforms.Length && rectTransforms[i].name != "Direction needle")
        {
            ++i;
        }
        //...and allocate the target for the rotation.
        directionNeedleRT = rectTransforms[i];
	}
	
	void Update () {
        RotateCompass();
    }

    private void RotateCompass()
    {
        Vector3 compassRotation = directionNeedleRT.rotation.eulerAngles;
        compassRotation.z = CalculateAngle();
        directionNeedleRT.rotation = Quaternion.Euler(compassRotation);
    }
    
    private float CalculateAngle()
    {
        return playerTransform.rotation.eulerAngles.y + Offset;
    }    
}
