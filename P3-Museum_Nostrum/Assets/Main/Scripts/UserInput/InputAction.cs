using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputAction {
    Swipe,  //e.g. rotate camera.
    Drag,   //e.g. drag 3D model of image into inventory
    Tap,    //e.g. select 3D model of image, before tapping inventory slot.
    UIDrag, //e.g. dragging image in inventory to image frame.
    UITap   //e.g. select image in inventory slot, before tapping on image frame to set it there.
}
