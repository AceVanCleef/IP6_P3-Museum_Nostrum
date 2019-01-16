using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputMessage {
    public bool handled;
    private InputAction action;

    public InputMessage(InputAction action)
    {
        handled = false;
        this.action = action;
    }
}
