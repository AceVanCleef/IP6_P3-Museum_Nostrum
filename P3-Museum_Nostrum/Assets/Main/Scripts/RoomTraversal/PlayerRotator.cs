using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotator : MonoBehaviour {

    public float turnTime = 0.6f;		// turn time
    public float moveSpeed = 0.5f;		// unit move speed

    private float invMoveTime;			// inverse movement time
    private bool isMoving;				// is unit moving
    private bool canMove;               // is unit able to move

    public enum RotationDirection
    {
        Left = -1,
        Right = 1
    }

    // function to start turning
    public virtual void Rotate(RotationDirection newRot)
    {
        // if unit is not moving
        if (!isMoving)
        {
            StartCoroutine(Turning((int) newRot));		// start ienumerator Turning
        }
    }


    // function to do actual turning from side to side in 90 degrees
    protected IEnumerator Turning(int newRot)
    {
        // unit is moving, define start and end rotation, define turn rate and time
        isMoving = true;
        int degrees = newRot * 90;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = transform.rotation * Quaternion.Euler(0, degrees, 0);
        float rate = 1f / turnTime;
        float t = 1f;

        // while time is greater than 0
        while (t > float.Epsilon)
        {
            // reduce turning time and rotate unit by time
            t -= Time.deltaTime * rate;
            transform.rotation = Quaternion.Slerp(endRot, startRot, t);
            yield return new WaitForEndOfFrame();
        }

        isMoving = false;		// unit is no longer moving
    }

    // function to handle no moving situations
    //protected abstract void CannotMove<T>(T component)
    //    where T : Component;

    // function to get isMoving value
    public bool IsMoving()
    {
        return isMoving;
    }

    // function to get canMove value
    public bool CanMove()
    {
        return canMove;
    }
}
