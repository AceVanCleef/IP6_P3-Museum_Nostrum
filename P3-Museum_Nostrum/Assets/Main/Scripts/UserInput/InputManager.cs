using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

            InitializeButtonCallbacks();
        }
    }
    #endregion SingletonSetup


    private bool blockSwipeAction = false;
    [SerializeField]
    [Tooltip("Defines how long other gestures block swipe gestures.")]
    private float UnlockDuration = 0.1f;

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

                if (DataLogger.Instance)
                    DataLogger.Instance.Log("turnSwipeLeft", CameraViewDirection.Instance.GetCurrentState().GetDirectionIdentifier().ToString());
            }
            else if (data.Direction == SwipeDirection.Right)
            {
                playerRotator.Rotate(PlayerRotator.RotationDirection.Left);
                CameraViewDirection.Instance.GetCurrentState().TransitionLeft();
                CameraViewDirection.Instance.GetCurrentState().PrintState();

                if (DataLogger.Instance)
                    DataLogger.Instance.Log("turnSwipeRight", CameraViewDirection.Instance.GetCurrentState().GetDirectionIdentifier().ToString());
            }
        }
    }

    #endregion SwipeCallbacks

    #region ButtonCallbacks
    private void InitializeButtonCallbacks()
    {
        //setup left and right buttons
        GameObject lbGO = GameObject.Find("LeftButton");
        if (lbGO) lbGO.GetComponent<Button>().onClick.AddListener(OnLeftButtonClick);
        GameObject rbGO = GameObject.Find("RightButton");
        if (rbGO) rbGO.GetComponent<Button>().onClick.AddListener(OnRightButtonClick);
        //Other callbacks...

    }

    private void OnLeftButtonClick()
    {
        //draw a touch on GUI.
        if (DataLogger.Instance)
            DataLogger.Instance.Log("touch", "On LeftButton", Input.mousePosition.ToString());

        if (!playerRotator.IsMoving() && !blockSwipeAction)
        {
            playerRotator.Rotate(PlayerRotator.RotationDirection.Left);
            CameraViewDirection.Instance.GetCurrentState().TransitionLeft();
            CameraViewDirection.Instance.GetCurrentState().PrintState();

            if (DataLogger.Instance)
                DataLogger.Instance.Log("turnButtonLeft", CameraViewDirection.Instance.GetCurrentState().GetDirectionIdentifier().ToString());
        }
    }

    private void OnRightButtonClick()
    {
        if (DataLogger.Instance)
        {
            //draw a touch on GUI.
            DataLogger.Instance.Log("touch", "On RightButton", Input.mousePosition.ToString());
        }

        if (!playerRotator.IsMoving() && !blockSwipeAction)
        {
            playerRotator.Rotate(PlayerRotator.RotationDirection.Right);
            CameraViewDirection.Instance.GetCurrentState().TransitionRight();
            CameraViewDirection.Instance.GetCurrentState().PrintState();

            if (DataLogger.Instance)
                DataLogger.Instance.Log("turnButtonRight", CameraViewDirection.Instance.GetCurrentState().GetDirectionIdentifier().ToString());
        }
    }

    #endregion ButtonCallbacks

}
