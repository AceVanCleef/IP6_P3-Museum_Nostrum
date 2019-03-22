using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// handles custom user inputs not supported by Unity EventSystem and player rotation callbacks.
/// </summary>
public sealed class InputManager : MonoBehaviour {

    #region SingletonSetup
    private static InputManager inputManager;
    /// <summary>
    /// returns singleton instance of InputManager.
    /// </summary>
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
    /// <summary>
    /// enables raycasting against 3D GameObjects.
    /// </summary>
    /// <remarks>
    /// attaches a PhysicsRaycaster to the main camera. This allows raycasting 
    /// against 3D GameObjects utilizing Unity's EventSystem. Without this, 
    /// only GUI elements can be raycasted using the EventSystem.
    /// </remarks>
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
    /// <summary>
    /// locks player rotation.
    /// </summary>
    public void BlockSwipeAction()
    {
        blockSwipeAction = true;
    }

    /// <summary>
    /// unlocks player rotation.
    /// </summary>
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
    /// <summary>
    /// executes a player rotation depending on Swipe direction.
    /// </summary>
    /// <param name="data">required swipe direction info</param>
    private void HandleSwipe(SwipeData data)
    {
        if (!playerRotator.IsRotating() && !blockSwipeAction)
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

    /// <summary>
    /// executes a left rotation of the player.
    /// </summary>
    public void OnLeftButtonClick()
    {
        //draw a touch on GUI.
        if (DataLogger.Instance)
            DataLogger.Instance.Log("touch", "On LeftButton", Input.mousePosition.ToString());

        if (!playerRotator.IsRotating() && !blockSwipeAction)
        {
            playerRotator.Rotate(PlayerRotator.RotationDirection.Left);
            CameraViewDirection.Instance.GetCurrentState().TransitionLeft();
            CameraViewDirection.Instance.GetCurrentState().PrintState();

            if (DataLogger.Instance)
                DataLogger.Instance.Log("turnButtonLeft", CameraViewDirection.Instance.GetCurrentState().GetDirectionIdentifier().ToString());
        }
    }

    /// <summary>
    /// executes a right rotation of the player.
    /// </summary>
    public void OnRightButtonClick()
    {
        if (DataLogger.Instance)
        {
            //draw a touch on GUI.
            DataLogger.Instance.Log("touch", "On RightButton", Input.mousePosition.ToString());
        }

        if (!playerRotator.IsRotating() && !blockSwipeAction)
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
