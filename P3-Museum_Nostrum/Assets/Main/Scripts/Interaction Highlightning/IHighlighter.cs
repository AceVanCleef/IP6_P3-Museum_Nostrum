using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHighlighter {

    /// <summary>
    /// activates highlightning of this gameObject.
    /// </summary>
    void On();

    /// <summary>
    /// deactivates highlightning of this gameObject.
    /// </summary>
    void Off();
}
