using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// defines what user inputs (and thus gestures) are supported for GUI elements.
/// </summary>
public interface IInteractiveGUIElement : IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
}
