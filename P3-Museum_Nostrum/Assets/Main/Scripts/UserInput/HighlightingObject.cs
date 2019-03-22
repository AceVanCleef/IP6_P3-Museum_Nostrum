using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// handles highlightning of interactive gameObjects as well as GameObject selection.
/// </summary>
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

    /// <summary>
    /// highlights the inventory slots.
    /// </summary>

    protected void ActivateUISlotHighlightning()
    {
        ActivateHighlightningOf(allUISlotHighlighters);
    }

    /// <summary>
    /// disables highlighting of inventory slots.
    /// </summary>
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

    /// <summary>
    /// highlights all doors.
    /// </summary>
    protected void ActivateDoorHighlightning()
    {
        ActivateHighlightningOf(allDoorHighlighters);
    }

    /// <summary>
    /// disables highlightning of all doors.
    /// </summary>
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

    /// <summary>
    /// highlights all picture frames.
    /// </summary>
    protected void ActivatePictureFrameHighlightning()
    {
        ActivateHighlightningOf(allPictureFrameHighlighters);
    }

    /// <summary>
    /// disables highlightning of all picture frames.
    /// </summary>
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

    /// <summary>
    /// allows destinction between normal GameObjects and GUI elements.
    /// </summary>
    /// <remarks>required by subclasses for approriate texture swapping.</remarks>
    /// <returns>bool</returns>
    protected bool HasPlayerSelectedGUIElement()
    {
        return IsGUIElement;
    }

    /// <summary>
    /// registers the selectedGameObject as selected and highlights it.
    /// </summary>
    /// <param name="selectedHighlighter">the highlighter of the selected GameObject.</param>
    /// <param name="selectedGameObject">the selected GameObject.</param>
    /// <param name="isGUIElement">distinguishes whether the selected object is a GUI element.</param>
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

    /// <summary>
    /// deselects (previously) selected GameObject and resets all highlightnings.
    /// </summary>
    protected void Deselect()
    {
        if (HasPlayerSelectedAnObject())
        {
            currentlySelected.Off();
        }
        currentlySelected = null;
        selectedGO = null;
        DeactivatePictureFrameHighlightning();
        DeactivateUISlotHighlightning();
        ActivateDoorHighlightning();
    }

    /// <summary>
    /// returns the selected gameObject.
    /// </summary>
    /// <returns></returns>
    protected GameObject GetSelectedGameObject()
    {
        return selectedGO;
    }
    #endregion SelectionHighlightning
}
