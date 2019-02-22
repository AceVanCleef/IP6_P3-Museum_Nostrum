using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleInfo : MonoBehaviour {

    public Direction direction;
    //[HideInInspector]
    public Material material;

    [Tooltip("Sets the transform.position.y value of this triangle.")]
    public float elevationAboveFloor = 0.1f;

	// Use this for initialization
	void Awake () {
        material = GetComponent<Renderer>().material;
	}



    #region AdjustTriangleDimensionsDynamically

    /// <summary>
    /// adjusts this heatmap triangle to the dimensions of the RoomConfigurator rc.
    /// </summary>
    /// <param name="rc">The room configuration required to adjust this heatmap triangle.</param>
    public void AdjustTriangleDimensionsTo(RoomConfigurator rc)
    {
        AdjustHypotenuse(rc);
        AdjustTriangleAltitude(rc);
        AdjustPositioning(rc);
    }

    private void AdjustHypotenuse(RoomConfigurator rc)
    {
        //the ratio of "room width or height" to "length of hypotenuse" that keeps the triangle in perfect shape.
        float room2hypotenuseRatio = 3.0769230769230769230769230769231f; //value sampled from a 10x10 room and a default sized triangle.
        //keeps the heatmap in a square format.
        float s = Mathf.Min(rc.RoomLength, rc.RoomWidth);
        //calculate the new hypotenuse.
        Vector3 scale = transform.localScale;
        scale.y = s / room2hypotenuseRatio;
        transform.localScale = scale;
    }

    private void AdjustTriangleAltitude(RoomConfigurator rc)
    {
        //the ratio of "room width or height" to "altitude of triangle" that keeps the triangle in perfect shape.
        float room2altitudeRatio = 1.7391304347826086956521739130435f; //value sampled from a 10x10 room and a default sized triangle.
        //keeps the heatmap in a square format.
        float s = Mathf.Min(rc.RoomLength, rc.RoomWidth);
        //calculate the new altitude.
        Vector3 scale = transform.localScale;
        scale.x = s / room2altitudeRatio;
        transform.localScale = scale;
    }

    private void AdjustPositioning(RoomConfigurator rc)
    {
        //The ratio of "perfect triangle position" to "half room width or height".
        float trianglePos2roomDimensionRatio = 0.66f;
        // E.g. pos.x_in_10x10_room / half_width_of_10x10 room = 3.3 / 5 = 0.66 ratio.
        // x := desired new postion in larger or smaller room.
        // E.g. find x in 25x25 room.
        //      x / 12.5 = 0.66 => 0.66 * 12.5 = 5.

        Vector3 positioningOffset = Vector3.zero;
        //reset position to parent's position.
        transform.localPosition = Vector3.zero;

        //determine positioning offset
        float s = Mathf.Min(rc.RoomLength, rc.RoomWidth);
        switch (direction)
        {
            case Direction.North:
                positioningOffset.z = 0.5f * s * trianglePos2roomDimensionRatio; break;
            case Direction.South:
                positioningOffset.z = 0.5f * s * trianglePos2roomDimensionRatio * -1f; break;
            case Direction.East:
                positioningOffset.x = 0.5f * s * trianglePos2roomDimensionRatio; break;
            case Direction.West:
                positioningOffset.x = 0.5f * s * trianglePos2roomDimensionRatio * -1f; break;
            default: throw new System.Exception("TriangleInfo has no Direction allocated.");
        }
        //to move the triangle above the floor.
        positioningOffset.y = elevationAboveFloor;
        //set the new position.
        transform.localPosition += positioningOffset;
    }

    #endregion AdjustTriangleDimensionsDynamically

}