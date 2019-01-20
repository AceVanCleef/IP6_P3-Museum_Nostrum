using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// provides default implementation for IInteractiveGUIElements. Inherit this class 
/// on GUI elements and override methods to define specific behavior.
/// </summary>
public class AbstractInteractiveGUIElement : MonoBehaviour, IInteractiveGUIElement
{
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        InputManager.Instance.BlockSwipeAction();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        InputManager.Instance.UnlockSwipeAction();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
    }

}
