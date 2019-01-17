using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [Image]
/// Is draggable and selectable via tap. How to differentiate between selection and dragging?
/// OnTap -> Finger didn't move. Technically speaking, the difference of starting point and ending point 
/// of the touch will be within a smal ldelta value.
/// OnDrag -> The distance between start and end point of the touch will be larger than a small delta value.
/// A drag differentiates itself by hitting an interactable object using a raycast.
/// In case, no object has been hit by raycasting, a swipe will be interpreted.
/// 
/// [Door]
/// Some objects are only interactable by touch. One solution to enforace a distinction between 
/// objects who can be interacted with either tap or drag, is to define seperate Tags.
/// </summary>
public sealed class InputManager : AbstractInputManager {

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

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    private readonly float minDistanceForDrag = 1f;
    private readonly float minDistanceForSwipe = 20f;

    private GameObject hitGameObject = null;
    private List<InputAction> currentInteractionTags = new List<InputAction>();

    //Dragging
    private float dist;
    private Vector3 offset;
    private Vector3 v3;

    void Start () {
		if (inputManager == null)
        {
            inputManager = this;
        }
	}


	// Update is called once per frame
	void Update () {
		//Todo: Zwischen Tap, Swipe und Drag unterscheiden.

        if (Input.touchCount != 1)
        {
            return;
        }

        Touch touch = Input.touches[0];
        Vector2 pos = touch.position;

        if(touch.phase == TouchPhase.Began)
        {
            fingerUpPosition = touch.position;
            fingerDownPosition = touch.position;

            //shoot raycast.
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject != null)
            {
                currentInteractionTags.Clear();
                hitGameObject = hit.transform.gameObject;
                if (hitGameObject.GetComponent<InteractionTags>() != null)
                {
                    currentInteractionTags.AddRange(hitGameObject.GetComponent<InteractionTags>().interactionTags);

                    InitDragHandling(pos);
                }
                Debug.Log("Hit: " + hitGameObject.name + " interactiontags: " + currentInteractionTags.Count);
            }
        }

        if (touch.phase == TouchPhase.Moved)
        {
            fingerDownPosition = touch.position;
            if (IsDragGesture())
            {
                HandleDrag();

                Debug.Log("Moving " + hitGameObject.name + " with current pos: " + hitGameObject.transform.position + " - contains Drag tag? " + currentInteractionTags.Contains(InputAction.Drag));
            }
        }

        //Todo: check whether this phase is relevant:
        if (touch.phase == TouchPhase.Stationary)
        {
            fingerDownPosition = touch.position;

        }

        if (touch.phase == TouchPhase.Ended)
        {
            fingerDownPosition = touch.position;
            
            if (IsSingleTap())
            {
                Debug.Log("tapped screen");
                    
                Debug.Log("Item selected.");
                DoorScript ds = hitGameObject.GetComponent<DoorScript>();
                if (ds)
                {
                    ds.WarpToNextRoom();
                }
                //Todo: Image selection handling.
                    
            }
            else if (IsSwipe())
            {
                Debug.Log("Finger moved");

                if (IsVerticalSwipe())
                {
                    var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                    SendSwipe(direction);
                }
                else
                {
                    var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                    SendSwipe(direction);
                }
                fingerUpPosition = fingerDownPosition;
            }

        }
    }


    #region GestureChecks

    private bool IsDragGesture()
    {
        return (VerticalMovementDistance() > minDistanceForDrag || HorizontalMovementDistance() > minDistanceForDrag) && 
            currentInteractionTags.Contains(InputAction.Drag);
    }

    private bool IsSingleTap()
    {
        return (VerticalMovementDistance() < minDistanceForDrag || HorizontalMovementDistance() < minDistanceForDrag) &&
            currentInteractionTags.Contains(InputAction.Tap);
    }

    private bool IsSwipe()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    #endregion GestureChecks




    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
    }




    #region SwipeGestureAPI

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingerDownPosition,
            EndPosition = fingerUpPosition
        };
        //dataLogger.Log("SwipeDetected", fingerDownPosition.ToString(), fingerUpPosition.ToString());
        //OnSwipe(swipeData);
        HandleSwipe(swipeData);

    }

    private void HandleSwipe(SwipeData data)
    {
        /*if (!dragManager.IsDragging() && !playerMove.IsMoving())
        {
            if (data.Direction == SwipeDirection.Left)
            {
                playerMove.MovePlayer(0, 0, 1);
                Debug.Log("Swipe in Direction: " + data.Direction);
                CameraViewDirection.Instance.GetCurrentState().TransitionRight();
                CameraViewDirection.Instance.GetCurrentState().PrintState();
                dataLogger.Log(data);
                dataLogger.Log(CameraViewDirection.Instance.GetCurrentState().ToString());
                if (DataVisualizerManager.Instance != null) DataVisualizerManager.Instance.AfterViewDirectionChange();
            }
            else if (data.Direction == SwipeDirection.Right)
            {
                playerMove.MovePlayer(0, 0, -1);
                Debug.Log("Swipe in Direction: " + data.Direction);
                CameraViewDirection.Instance.GetCurrentState().TransitionLeft();
                CameraViewDirection.Instance.GetCurrentState().PrintState();
                dataLogger.Log(data);
                dataLogger.Log(CameraViewDirection.Instance.GetCurrentState().ToString());
                if (DataVisualizerManager.Instance != null) DataVisualizerManager.Instance.AfterViewDirectionChange();
            }
        }*/
        Debug.Log("handling swipes. Direction: " + data.Direction);
    }

    public struct SwipeData
    {
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        public SwipeDirection Direction;
    }

    public enum SwipeDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    #endregion SwipeGestureAPI




    #region DragGestureAPI

    private void InitDragHandling(Vector2 tochPos)
    {
        dist = CalculateDistance(hitGameObject);
        v3 = new Vector3(tochPos.x, tochPos.y, dist);
        v3 = Camera.main.ScreenToWorldPoint(v3);
        offset = hitGameObject.transform.position - v3;
    }

    private void HandleDrag()
    {
        v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
        v3 = Camera.main.ScreenToWorldPoint(v3);
        hitGameObject.transform.position = v3 + offset;
        Debug.DrawRay(transform.position, hitGameObject.transform.position, Color.blue);
    }

    private float CalculateDistance(GameObject hitGO)
    {
        //return CameraViewDirection.Instance.GetCurrentState().HandleDragDistanceCalculation(hit.transform.position, Camera.main.transform.position);
        return hitGO.transform.position.z - Camera.main.transform.position.z;
    }

    #endregion DragGestureAPI
}
