using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractSilhouetteHighlighter : MonoBehaviour, IHighlighter {

    /// <summary>
    /// grants access to shader properties.
    /// </summary>
    private Outline outlineScript;

    [Tooltip("Define the highlightning color and its transparency.")]
    public Color color;
    /// <summary>
    /// stores the target color in its invisible state (alphe = 0f).
    /// </summary>
    private Color invisible;

    protected virtual void Start()
    {
        //Get shader handling script.
        outlineScript = gameObject.GetComponent<Outline>();
        //Set Outline mode to SilhouetteOnly.
        if (outlineScript.OutlineMode != Outline.Mode.SilhouetteOnly)
        {
            Debug.LogWarning("Outline mode 'SilhouetteOnly' expected. Settings have been automatically adjusted.");
            outlineScript.OutlineMode = Outline.Mode.SilhouetteOnly;
        }

        //Setup values for Off().
        invisible = color;
        invisible.a = 0f;

        Off();
    }

    public void Off()
    {
        outlineScript.OutlineColor = invisible;
    }

    public void On()
    {
        outlineScript.OutlineColor = color;
    }
}
