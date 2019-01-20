using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// defines what user inputs (and thus gestures) are supported for GameObjects.
/// </summary>
public interface IInteractiveGameObject : IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{

    GameObject GetHitGameObject(PointerEventData eventData);

    void ToggleOutline();
    void DisableOutline();
    void EnableOutline();
}
