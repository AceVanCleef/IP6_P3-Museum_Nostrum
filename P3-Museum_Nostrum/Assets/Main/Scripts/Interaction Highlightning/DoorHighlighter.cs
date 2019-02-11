using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHighlighter : MonoBehaviour, IHighlighter
{
    private Outline outlineScript;

    void Start()
    {
        outlineScript = gameObject.GetComponent<Outline>();
        if (outlineScript.OutlineMode != Outline.Mode.OutlineAll ||
            outlineScript.OutlineMode != Outline.Mode.OutlineVisible)
        {
            Debug.LogWarning("Outline mode 'OutlineAll or OutlineVisible' recommended.");
        }

        On();
    }

    public void Off()
    {
        outlineScript.OutlineWidth = 0f;
    }

    public void On()
    {
        outlineScript.OutlineWidth = 30f;
    }
}
