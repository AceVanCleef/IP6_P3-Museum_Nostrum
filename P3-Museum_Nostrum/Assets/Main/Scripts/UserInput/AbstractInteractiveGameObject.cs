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

    /// <summary>
    /// the current game object selected by a single tap.
    /// </summary>
    protected static GameObject selectedGameObject = null;
    public static GameObject SelectedGameObject
    {
        get
        {
            return selectedGameObject;
        }
    }

    protected readonly float outlineWidthOnInactive = 0f;
    [SerializeField][Tooltip("Defines how wide the highlightning outline should be.")]
    private float outlineWidthOnActive = 0.15f;
    protected float OutlineWidthOnActive
    {
        //readonly access while providing adjustability in inspector.
        get
        {
            return outlineWidthOnActive;
        }
    }


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

    public virtual void ToggleOutline()
    {
        //requires UltimateOutline shader attached as material on GameObject.
    }

    public virtual void DisableOutline()
    {
        //requires UltimateOutline shader attached as material on GameObject.
    }

    public virtual void EnableOutline()
    {
        //requires UltimateOutline shader attached as material on GameObject.
    }
}
