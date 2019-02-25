using System;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = true;
    //private bool detectSwipeOnlyAfterRelease = false; //original value.

    [SerializeField]
    private float minDistanceForSwipe = 20f;

    public static event Action<SwipeData> OnSwipe = delegate { };

   // private DataLogger dataLogger;

    void Awake()
    {
        //get DataLogger
        GameObject go = GameObject.Find("DataLogger");
       // dataLogger = (DataLogger)go.GetComponent(typeof(DataLogger));
    }

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
            }

            if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            if (IsVerticalSwipe())
            {
                var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;

                //hereComesLogger
                Debug.Log("swipeUpDownRegistered" + direction + "::" + fingerDownPosition.ToString() + "::" + fingerUpPosition.ToString());

                SendSwipe(direction);
            }
            else
            {
                var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                
                //hereComesLogger
                Debug.Log("swipeRightLeftRegistered" + direction + "::" + fingerDownPosition.ToString() + "::" + fingerUpPosition.ToString());

                SendSwipe(direction);
            }
            fingerUpPosition = fingerDownPosition;
        }
        else
        {
            //dataLogger.Log("touch", fingerDownPosition.ToString(), null, null);
            //hereComesLogger
            Debug.Log("touchRegistered" + fingerDownPosition.ToString());
        }
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
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
        OnSwipe(swipeData);
    }

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