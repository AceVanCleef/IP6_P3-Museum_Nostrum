using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// provides default implementation for IInteractiveGUIElements. Inherit this class 
/// on GUI elements and override methods to define specific behavior.
/// </summary>
public class AbstractInteractiveGUIElement : HighlightingObject, IInteractiveGUIElement
{
    /// <summary>
    /// the position on screen where the drag has been started.
    /// </summary>
    protected Vector2 DragStartPosOnScreen;

    protected new virtual void Start()
    {
        base.Start();
        //Debug.Log("AbstractInteractiveGameObject started");
    }


    #region UserInput
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        Deselect(); //guarantees expected user behavior of "either select/deselect" or "drag n drop".
        InputManager.Instance.BlockSwipeAction();
        DeactivateDoorHighlightning();

    }

    public virtual void OnDrag(PointerEventData eventData)
    {
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        ActivateDoorHighlightning();
        InputManager.Instance.UnlockSwipeAction();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
    }
    #endregion UserInput

}
