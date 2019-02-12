using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractOutlineHighlighter : MonoBehaviour, IHighlighter {

    /// <summary>
    /// grants access to shader properties.
    /// </summary>
    private Outline outlineScript;

    [Tooltip("Define the highlightning color.")]
    public Color outlineColor;

    protected virtual void Start()
    {
        //Get shader handling script.
        outlineScript = gameObject.GetComponent<Outline>();
        //Set Outline mode to OutlineAll.
        if (outlineScript.OutlineMode != Outline.Mode.OutlineAll ||
            outlineScript.OutlineMode != Outline.Mode.OutlineVisible)
        {
            Debug.LogWarning("Outline mode 'OutlineAll or OutlineVisible' recommended.");
            outlineScript.OutlineMode = Outline.Mode.OutlineAll;

        }

        outlineScript.OutlineColor = outlineColor;
        Off();
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
