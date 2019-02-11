using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// highlights the surface of a picture frame when activated.
/// </summary>
public class PictureFrameHighlighter : MonoBehaviour, IHighlighter {

    /// <summary>
    /// grants access to shader properties.
    /// </summary>
    private Outline outlineScript;

    [Tooltip("Define the highlightning color and its transparency.")]
    public Color targetColor;
    /// <summary>
    /// stores the target color in its invisible state (alphe = 0f).
    /// </summary>
    private Color invisible;

    void Start()
    {
        outlineScript = gameObject.GetComponent<Outline>();
        if(outlineScript.OutlineMode != Outline.Mode.SilhouetteOnly)
        {
            Debug.LogWarning("Outline mode 'SilhouetteOnly' expected. Settings have been automatically adjusted.");
            outlineScript.OutlineMode = Outline.Mode.SilhouetteOnly;
        }
        invisible = targetColor;
        invisible.a = 0f;
        Off();
    }

    public void Off()
    {
        outlineScript.OutlineColor = invisible;
    }

    public void On()
    {
        outlineScript.OutlineColor = targetColor;
    }
}
