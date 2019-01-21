using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// defines contract to implement tag initialization. The concrete implementation is 
/// a matter of the developer. He has to call the method in the Start() of MonoBehavior of 
/// the implementing class.
/// </summary>
public interface ITagEnsurance  {

    //Todo: Find a way to call InitializeTag() in a Start() of a ParentClass or similar.

    void InitializeTag();
}
