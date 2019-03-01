using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoorScript : AbstractInteractiveGameObject
{

    [Tooltip("To allocate the camera target position when clickin onto a door, drag the target room's CameraPositionNode into this variable.")]
    public GameObject TargetCameraPositionNode;
    private CameraPositionInfo targetPositionInfo;

    private GameObject player;
    private GameObject hud;

    private string doorAnimationName = "DoorAnimation";
    private float doorAnimationLength = 0f; 
    private float doorAnimationAdjustment = 0.75f;
    private Animator doorAnimator;
    private Animator fadeAnimator;

    private MovePointMap movePointMap;
    
    protected override void Start()
    {
        targetPositionInfo = TargetCameraPositionNode.GetComponent<CameraPositionInfo>();
        
        player = GameObject.FindGameObjectWithTag("Player");

        InitializeWalkAnimation();

        //initialisations for map
        GameObject mapPoint = GameObject.Find("MapWrapper");
        if (mapPoint)
           movePointMap = (MovePointMap)mapPoint.GetComponent(typeof(MovePointMap));
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (DataLogger.Instance) DataLogger.Instance.Log("touch", "On Door", eventData.position.ToString() );
        StartCoroutine(StartDoorTransition());
    }

    public void WarpToNextRoom()
    {
        GetComponent<AudioSource>().Play();

        //for DataVisualizerManager:
        Vector3 previousPos = player.transform.position;

        player.transform.position = targetPositionInfo.GetWorldPosition();
        targetPositionInfo.UpdatePlayerCameraRendering();
        

        if (DataLogger.Instance)
        {
            DataLogger.Instance.Log("goToRoom", targetPositionInfo.transform.root.name, targetPositionInfo.GetWorldPosition().ToString(), previousPos.ToString());
        }
    }

    public IEnumerator StartDoorTransition()
    {
        //play animations
        doorAnimator.Play("DoorAnimation");
        fadeAnimator.Play("DoorAnimationFade");

        //wait till animation has ended, to continue with warp to next room
        yield return new WaitForSeconds(doorAnimationLength - doorAnimationAdjustment);
        WarpToNextRoom();

        //moves the point on the map
        if (movePointMap)
            movePointMap.moveMapPointer(TargetCameraPositionNode.transform.root.gameObject.name);
    }


    private void InitializeWalkAnimation()
    {
        //initialisations for door animations
        doorAnimator = player.GetComponentInChildren<Animator>();
        hud = GameObject.FindGameObjectWithTag("HUD");

        //Todo: Check if prefab "FadeBlackDoor" already exists. If not, instantiate one as child of HUD.

        fadeAnimator = hud.GetComponentInChildren<Animator>();

        //get the length of the door animation
        AnimationClip[] clips = doorAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == doorAnimationName)
                doorAnimationLength = clip.length;
        }
    }
}
