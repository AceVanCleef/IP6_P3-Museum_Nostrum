using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionInfo : MonoBehaviour {

    /// <summary>
    /// returns the position for the camera in this room.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetWorldPosition()
    {
        return transform.parent.transform.position;
    }


    public void UpdatePlayerCameraRendering()
    {
        //passing call from DoorScript to RoomConfigurator which holds the information about room dimensions.
        transform.parent.GetComponent<RoomConfigurator>().AdjustRenderingRangeOfPlayerCamera();
    }
}
