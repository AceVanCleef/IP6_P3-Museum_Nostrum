using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// provides default implementation for IInteractiveGameObject. Inherit this class 
/// on GameObjects and override methods to define specific behavior.
/// </summary>
public class AbstractInteractiveGameObject : MonoBehaviour, IInteractiveGameObject
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

    #region InteractionHighlightningVariables
    private readonly List<IHighlighter> allDoorHighlighters = new List<IHighlighter>();
    private readonly List<IHighlighter> allPictureFrameHighlighters = new List<IHighlighter>();
    #endregion InteractionHighlightningVariables

    protected virtual void Start()
    {
        //Debug.Log("AbstractInteractiveGameObject started");

        GetAllDoorHighlighters();
        GetAllPictureFrameHighlighters();
    }

    #region UserInput
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        InputManager.Instance.BlockSwipeAction();
        DeactivateDoorHighlightning();
        ActivatePictureFrameHighlightning();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        ActivateDoorHighlightning();
        DeactivatePictureFrameHighlightning();
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


    #region InteractionHighlightning
    #region GeneralHighlightning
    protected void ActivateHighlightningOf( List<IHighlighter> highlighters )
    {
        for (int i = 0; i < highlighters.Count; ++i)
        {
            highlighters[i].On();
        }
    }

    protected void DeactivateHighlightningOf( List<IHighlighter> highlighters )
    {
        for (int i = 0; i < highlighters.Count; ++i)
        {
            highlighters[i].Off();
        }
    }
    #endregion GeneralHighlightning


    #region DoorHighlighting
    private void GetAllDoorHighlighters()
    {
        allDoorHighlighters.AddRange( UnityEngine.Object.FindObjectsOfType<DoorHighlighter>() );
    }

    protected void ActivateDoorHighlightning()
    {
        ActivateHighlightningOf(allDoorHighlighters);
    }

    protected void DeactivateDoorHighlightning()
    {
        DeactivateHighlightningOf(allDoorHighlighters);
    }
    #endregion DoorHighlightning

    #region PictureFrameHighlighting
    private void GetAllPictureFrameHighlighters()
    {
        allPictureFrameHighlighters.AddRange(UnityEngine.Object.FindObjectsOfType<PictureFrameHighlighter>());
    }

    protected void ActivatePictureFrameHighlightning()
    {
        ActivateHighlightningOf(allPictureFrameHighlighters);
    }

    protected void DeactivatePictureFrameHighlightning()
    {
        DeactivateHighlightningOf(allPictureFrameHighlighters);
    }
    #endregion PictureFrameHighlightning

    #endregion InteractionHighlightning
}
