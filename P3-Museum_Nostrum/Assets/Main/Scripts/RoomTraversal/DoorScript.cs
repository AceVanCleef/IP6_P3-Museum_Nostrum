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
    private float doorAnimationLength = 0;

    protected override void Start()
    {
        targetPositionInfo = TargetCameraPositionNode.GetComponent<CameraPositionInfo>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("GoAnimation");
        //WarpToNextRoom();
        StartCoroutine(StartDoorAnimation());
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

    public IEnumerator StartDoorAnimation()
    {

       
        Animator doorAnimator = player.GetComponentInChildren<Animator>();
        doorAnimator.Play("DoorAnimation");
        Debug.Log("LosAnimation");
        hud = GameObject.FindGameObjectWithTag("HUD");

        if (hud != null)
            Debug.Log("null1");

        Animator fadeAnimator = hud.GetComponentInChildren<Animator>();
        
        if (fadeAnimator != null)
            Debug.Log("null2");


        fadeAnimator.Play("DoorAnimationFade");


        

        AnimationClip[] clips = doorAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == doorAnimationName)
                doorAnimationLength = clip.length;            
        }

        Debug.Log(doorAnimationLength+":X:"+ doorAnimationName);


        yield return new WaitForSeconds(doorAnimationLength - 0.75f);

        WarpToNextRoom();

    }

}
