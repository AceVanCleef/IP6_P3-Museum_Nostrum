using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private bool isDoubleDoor = false;

    private float doorWidth;

    void Update()
    {
        UpdateDoorDimensions();
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
        pos.y = doorHeight / 2f;
        transform.position = pos;
    }
}
