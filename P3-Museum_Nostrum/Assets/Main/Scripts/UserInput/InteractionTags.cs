using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows to define multiple tags which determine how a user can interact with this GameObject.
/// </summary>
public class InteractionTags : MonoBehaviour {

    //only allocate this in inspector!
    public InputAction[] interactionTags;
}
