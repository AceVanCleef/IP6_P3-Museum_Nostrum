using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// provides default implementation for IInteractiveGameObject. Inherit this class 
/// on GameObjects and override methods to define specific behavior.
/// </summary>
public class AbstractInteractiveGameObject : AbstractUIDetectingGameObject, IInteractiveGameObject
{

    /// <summary>
    /// the current game object hit by raycast.
    /// </summary>
    protected static GameObject hitGameObject;


    protected new virtual void Start()
    {
        base.Start();
        //Debug.Log("AbstractInteractiveGameObject started");
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
    }

    public virtual GameObject GetHitGameObject(PointerEventData eventData)
    {
        return eventData.pointerCurrentRaycast.gameObject;
    }
}
