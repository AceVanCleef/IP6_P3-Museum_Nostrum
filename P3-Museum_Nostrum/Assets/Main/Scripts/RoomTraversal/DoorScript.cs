using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, IOnTap{

    [Tooltip("To allocate the camera target position when clickin onto a door, drag the target room's CameraPositionNode into this variable.")]
    public GameObject TargetCameraPositionNode;
    private CameraPositionInfo targetPositionInfo;

    private GameObject player;

    void Start()
    {
        targetPositionInfo = TargetCameraPositionNode.GetComponent<CameraPositionInfo>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void WarpToNextRoom()
    {
        //WholeScreenFadeInOut.Instance.FadeIn();
        GetComponent<AudioSource>().Play();

        //for DataVisualizerManager:
        Vector3 previousPos = player.transform.position;

        player.transform.position = targetPositionInfo.GetWorldPosition();
        /*
        if (DataVisualizerManager.Instance != null) DataVisualizerManager.Instance.PlayerEnteredNewRoom();
        if (DataVisualizerManager.Instance != null) DataVisualizerManager.Instance.AfterViewDirectionChange();
        if (DataVisualizerManager.Instance != null) DataVisualizerManager.Instance.TraceLineBetween(previousPos, player.transform.position);
        */
        //WholeScreenFadeInOut.Instance.FadeOut();
    }

    public void OnTap()
    {
        WarpToNextRoom();
    }
}
