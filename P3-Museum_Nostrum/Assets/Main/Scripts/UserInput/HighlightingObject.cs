using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HighlightingObject : MonoBehaviour {


    #region InteractionHighlightningVariables
    private readonly List<IHighlighter> allUISlotHighlighters = new List<IHighlighter>();

    private readonly List<IHighlighter> allDoorHighlighters = new List<IHighlighter>();
    private readonly List<IHighlighter> allPictureFrameHighlighters = new List<IHighlighter>();
    #endregion InteractionHighlightningVariables

    #region SelectionHighlightningVariables
    /// <summary>
    /// the current GUI or game object selected by a single tap.
    /// </summary>
    private static IHighlighter currentlySelected = null;
    private static GameObject selectedGO = null;
    private static bool IsGUIElement = false;
    #endregion SelectionHighlightningVariables

    // Use this for initialization
    protected virtual void Start () {

        GetAllUISlotHighlighters();
        GetAllDoorHighlighters();
        GetAllPictureFrameHighlighters();
    }

    #region InteractionHighlightning
    
    #region GeneralHighlightning
    protected void ActivateHighlightningOf(List<IHighlighter> highlighters)
    {
        for (int i = 0; i < highlighters.Count; ++i)
        {
            highlighters[i].On();
        }
    }

    protected void DeactivateHighlightningOf(List<IHighlighter> highlighters)
    {
        for (int i = 0; i < highlighters.Count; ++i)
        {
            highlighters[i].Off();
        }
    }
    #endregion GeneralHighlightning

    #region UISlotHighlightning
    private void GetAllUISlotHighlighters()
    {
        allUISlotHighlighters.AddRange(UnityEngine.Object.FindObjectsOfType<UISlotHighlighter>());
    }

    protected void ActivateUISlotHighlightning()
    {
        ActivateHighlightningOf(allUISlotHighlighters);
    }

    protected void DeactivateUISlotHighlightning()
    {
        DeactivateHighlightningOf(allUISlotHighlighters);
    }
    #endregion UISlotHighlightning

    #region DoorHighlighting
    private void GetAllDoorHighlighters()
    {
        allDoorHighlighters.AddRange(UnityEngine.Object.FindObjectsOfType<DoorHighlighter>());
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


    #region SelectionHighlightning
    protected bool HasPlayerSelectedAnObject()
    {
        return currentlySelected != null;
    }

    protected bool HasPlayerSelectedGUIElement()
    {
        return IsGUIElement;
    }

    protected void Select(IHighlighter selectedHighlighter, GameObject selectedGameObject, bool isGUIElement = false)
    {
        if (HasPlayerSelectedAnObject())
        {
            Deselect();
        }
        currentlySelected = selectedHighlighter;
        selectedGO = selectedGameObject;
        IsGUIElement = isGUIElement;
        currentlySelected.On();
    }

    protected void Deselect()
    {
        if (HasPlayerSelectedAnObject())
        {
            currentlySelected.Off();
        }
        currentlySelected = null;
        selectedGO = null;
    }

    protected GameObject GetSelectedGameObject()
    {
        return selectedGO;
    }
    #endregion SelectionHighlightning


    #region TextureTransfer
    
    #endregion TextureTransfer
}
