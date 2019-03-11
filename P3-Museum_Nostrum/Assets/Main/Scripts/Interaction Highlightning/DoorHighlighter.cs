using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHighlighter : AbstractOutlineHighlighter
{
    protected new virtual void Start()
    {
        base.Start();
        On();

        gameObject.layer = LayerMask.NameToLayer("DoorHighlightningShaders");
    }
}
