﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class InputManager : MonoBehaviour {

    #region SingletonSetup
    private static InputManager inputManager;
    public static InputManager Instance
    {
        get
        {
            return inputManager;
        }
    }

    private InputManager()
    {
        //prevents multiple instances.
    }

    void Awake()
    {
        if (inputManager == null)
        {
            inputManager = this;
            playerRotator = GetComponent<PlayerRotator>();
            AddPhysicsRaycaster();

            //register swipe callback
            SwipeDetector.OnSwipe += HandleSwipe;
        }
    }
    #endregion SingletonSetup


    private bool blockSwipeAction = false;
    [SerializeField]
    [Tooltip("Defines how long other gestures block swipe gestures.")]
    private float UnlockDuration = 0.1f;

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    private readonly float minDistanceForSwipe = 20f;

    private PlayerRotator playerRotator;


    #region EventSystemSetup
    void AddPhysicsRaycaster()
    {
        PhysicsRaycaster physicsRaycaster = GameObject.FindObjectOfType<PhysicsRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
        }
    }
    #endregion EventSystemSetup

    #region BlockSwipeGesture
    public void BlockSwipeAction()
    {
        blockSwipeAction = true;
    }

    public void UnlockSwipeAction()
    {
        StartCoroutine(UnlockSwipeActionAfter(UnlockDuration));
    }

    private IEnumerator UnlockSwipeActionAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        blockSwipeAction = false;
    }
    #endregion BlockSwipeGesture


    #region SwipeCallbacks
    private void HandleSwipe(SwipeData data)
    {
        if (!playerRotator.IsMoving() && !blockSwipeAction)
        {
            if (data.Direction == SwipeDirection.Left)
            {
                playerRotator.Rotate(PlayerRotator.RotationDirection.Right);
                CameraViewDirection.Instance.GetCurrentState().TransitionRight();
                CameraViewDirection.Instance.GetCurrentState().PrintState();
                //dataLogger.Log(data);
                //dataLogger.Log(CameraViewDirection.Instance.GetCurrentState().ToString());
                //if (DataVisualizerManager.Instance != null) DataVisualizerManager.Instance.AfterViewDirectionChange();
            }
            else if (data.Direction == SwipeDirection.Right)
            {
                playerRotator.Rotate(PlayerRotator.RotationDirection.Left);
                CameraViewDirection.Instance.GetCurrentState().TransitionLeft();
                CameraViewDirection.Instance.GetCurrentState().PrintState();
                //dataLogger.Log(data);
                //dataLogger.Log(CameraViewDirection.Instance.GetCurrentState().ToString());
                //if (DataVisualizerManager.Instance != null) DataVisualizerManager.Instance.AfterViewDirectionChange();
            }
        }
        Debug.Log("handling swipes. Direction: " + data.Direction);
    }

    #endregion SwipeCallbacks

}
