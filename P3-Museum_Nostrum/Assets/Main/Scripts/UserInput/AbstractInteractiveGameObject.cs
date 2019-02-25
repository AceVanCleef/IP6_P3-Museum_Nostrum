using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// provides default implementation for IInteractiveGameObject. Inherit this class 
/// on GameObjects and override methods to define specific behavior.
/// </summary>
public class AbstractInteractiveGameObject : HighlightingObject, IInteractiveGameObject
{

    /// <summary>
    /// the current game object hit by raycast.
    /// </summary>
    protected static GameObject hitGameObject;

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
        ActivatePictureFrameHighlightning();

        //required for DataLogger.
        DragStartPosOnScreen = eventData.position;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (DataLogger.Instance)
            DataLogger.Instance.Log("drag n drop", DragStartPosOnScreen.ToString(), eventData.position.ToString());

        InputManager.Instance.UnlockSwipeAction();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
    }

    public virtual GameObject GetHitGameObject(PointerEventData eventData)
    {
        return eventData.pointerCurrentRaycast.gameObject;
    }
    #endregion UserInput
}
