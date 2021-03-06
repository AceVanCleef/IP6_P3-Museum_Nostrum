﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// allows configuration of the door's dimensions.
/// </summary>
[ExecuteInEditMode]
public class DoorConfigurator : MonoBehaviour {

    // Inspiration for dimensions:
    // http://www.build.com.au/standard-door-sizes
    // https://www.solidwoodendoors.com/tall-doors-correct-proportions/
    // https://homeguides.sfgate.com/standard-inside-door-sizes-84805.html

    [SerializeField]
    [Range(2.04f, 4.08f)]
    private float doorHeight = 2.032f;

    [SerializeField]
    [Range(2.0f, 3.0f)] //previous min: 2.217f
    [Tooltip("Define how wide the door is in relation to its height. Most common ratio: 2,4878.")]
    private float height2WidthRatio = 2.5f;

    [SerializeField][Tooltip("Double the width of the door? Used when a double door texture is applied to this GO.")]
    private bool isDoubleDoor = false;

    private float doorWidth;

    void Update()
    {
        if (Application.isPlaying)
        {
            // code executed in play mode
        }
        else
        {
            UpdateDoorDimensions();
        }
    }

    private void UpdateDoorDimensions()
    {
        doorWidth = doorHeight / height2WidthRatio;
        if (isDoubleDoor) doorWidth *= 2;
        Vector3 scale = transform.localScale;
        scale.x = doorWidth;
        scale.y = doorHeight;
        transform.localScale = scale;
        //adjust position
        Vector3 pos = transform.position;
        if (transform.parent != null)
        {
            pos.y = transform.parent.position.y + (doorHeight / 2f);
        }
        else
        {
            pos.y = doorHeight / 2f;
            Debug.LogWarning(transform.name + " can't be moved vertically. " + transform.name + 
                " must be child of another GameObject. Attach it to a room, a room's Doors_Holder or any other GameObject.");
        }
        transform.position = pos;
    }
}
