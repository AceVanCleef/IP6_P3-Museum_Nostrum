using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LampRotationConfigurator : MonoBehaviour, ITagEnsurance
{

    private Transform movingParts;

    //[SerializeField]
    [Range(-130, 130)]
    [Tooltip("Defines angle of the spotlight's MovingParts within the allowed angle range.")]
    private float angle = 0f;
    private Vector3 currentRotation;

	void Start () {
        InitializeTag();

        movingParts = transform.Find("MovingParts");
        currentRotation = new Vector3(0f, 0f, 0f);
    }

    public void InitializeTag()
    {
        GameObject spotlight = GetComponentInChildren<Light>().gameObject;

        if (spotlight.tag != "Lights")
        {
            spotlight.tag = "Lights";
        }
    }


    void Update()
    {
        if (Application.isPlaying)
        {
            // code executed in play mode
        }
        else
        {
            // code executed in edit mode

        //Todo: Issue: When parent Lamp_(Default).transform.z != 0, the rotation of the MovingParts behaves awkward.
           // AdjustSpotlightAngle();
        }
    }

    private void AdjustSpotlightAngle()
    {
        currentRotation.x = angle + transform.localRotation.eulerAngles.x;
        currentRotation.y = transform.localRotation.eulerAngles.y;
        //currentRotation.z = transform.localRotation.eulerAngles.z;

        Debug.Log(currentRotation);

        movingParts.localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z);
    }
}
