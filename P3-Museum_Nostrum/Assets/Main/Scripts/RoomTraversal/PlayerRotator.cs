using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// handles rotation of player GameObject.
/// </summary>
public class PlayerRotator : MonoBehaviour {

    public float turnTime = 0.6f;		// turn time
    public float moveSpeed = 0.5f;		// unit move speed

    private float invMoveTime;			// inverse movement time
    private bool isRotating;			// is unit moving
    private bool canRotate;             // is unit able to move

    /// <summary>
    /// enumerates towards which directions the player can rotate.
    /// </summary>
    public enum RotationDirection
    {
        Left = -1,
        Right = 1
    }

    // function to start turning
    /// <summary>
    /// rotates player according to RotationDirection by 90 degrees.
    /// </summary>
    /// <param name="newRot">the direction the player rotates towards</param>
    public virtual void Rotate(RotationDirection newRot)
    {
        // if unit is not moving
        if (!isRotating)
        {
            StartCoroutine(Turning((int) newRot));		// start ienumerator Turning
        }
    }


    // function to do actual turning from side to side in 90 degrees
    protected IEnumerator Turning(int newRot)
    {
        // unit is rotating, define start and end rotation, define turn rate and time
        isRotating = true;
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

        isRotating = false;		// unit is no longer rotating
    }

    /// <summary>
    /// returns whether the player is currently rotating.
    /// </summary>
    /// <returns></returns>
    public bool IsRotating()
    {
        return isRotating;
    }

    /// <summary>
    /// returns whether the player can rotate at the moment.
    /// </summary>
    /// <returns></returns>
    public bool CanRotate()
    {
        return canRotate;
    }
}
