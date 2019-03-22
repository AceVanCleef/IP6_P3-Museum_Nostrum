using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// handles user input for and texture swapping of this InteractiveUISlot.
/// </summary>
public class InteractiveUISlot : AbstractInteractiveGUIElement, ITagEnsurance
{

    Vector3 startPosition;
    RawImage ri;

    //the dimensions of this UISlot.
    private static readonly Vector2 InitialSize = new Vector2(160f, 160f);
    private static readonly Vector2 HoverSize = new Vector2(200f, 200f);


    protected new virtual void Start()
    {
        base.Start();

        InitializeTag();
        ri = GetComponent<RawImage>();
    }

    public void InitializeTag()
    {
        if (gameObject.tag != "DraggableUI")
        {
            gameObject.tag = "DraggableUI";
        }
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        //required for DataLogger.
        DragStartPosOnScreen = eventData.position;

        if (HasTexture())
        {
            //Block swipes
            base.OnBeginDrag(eventData);

            //remember start position of UISlot.
            startPosition = transform.position;

            GetComponent<RectTransform>().sizeDelta = HoverSize;

            ActivatePictureFrameHighlightning();

            if (DataLogger.Instance)
                DataLogger.Instance.Log("dragBeginSlot",
                    transform.parent.transform.parent.name,         //name of UISlot
                    ri.texture.name,                                //texture name of UISlot
                    eventData.position.ToString(),                  //start position of drag gesture
                    transform.parent.transform.parent.position.ToString()); //position of UISlot
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (HasTexture())
        {
            transform.position = Input.mousePosition;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (HasTexture())
        {
            GameObject pictureCanvas = InteractivePicture.FindPictureCanvas(eventData.position);
            OnEndDragNext(pictureCanvas, eventData, startPosition);
        }

        if (DataLogger.Instance)
            DataLogger.Instance.Log("drag n drop", DragStartPosOnScreen.ToString(), eventData.position.ToString());
    }

    /// <summary>
    /// OnEndDrag callback.
    /// </summary>
    /// <remarks>requiered by and interfacing with ReplayManager.</remarks>
    /// <param name="pictureCanvas"></param>
    /// <param name="eventData"></param>
    /// <param name="startPos"></param>
    public void OnEndDragNext(GameObject pictureCanvas, PointerEventData eventData, Vector3 startPos)
    {

        if (pictureCanvas != null)
        {
            AttachPictureToPictureCanvas(pictureCanvas);

        }
        else
        {
            if (DataLogger.Instance)
                DataLogger.Instance.Log("dragEndSlot",
                    transform.parent.transform.parent.name,         //name of UISlot
                    ri.texture.name,                                //texture name of UISLot
                    eventData.position.ToString(),                  //end position of drag motion.
                    transform.parent.transform.parent.ToString());  //position of UISlot
        }

        //reset position.
        transform.position = startPos;

        GetComponent<RectTransform>().sizeDelta = InitialSize;
        //toggle highlightnings.
        Deselect();

        //Reenable swipes
        base.OnEndDrag(eventData);


        if (DataLogger.Instance)
            DataLogger.Instance.Log("drag n drop", DragStartPosOnScreen.ToString(), eventData.position.ToString());
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        if (DataLogger.Instance)
        {
            //draw a touch on GUI.
            DataLogger.Instance.Log("touch", "On InteractiveUISlot", eventData.position.ToString());
        }

        if (HasTexture() && !HasPlayerSelectedAnObject())
        {
            GameObject highlighterCarryingGUIElement = transform.parent.transform.parent.gameObject;
            Select(highlighterCarryingGUIElement.GetComponent<UISlotSelectedHighlighter>(), gameObject, true);
            DeactivateDoorHighlightning();
            ActivatePictureFrameHighlightning();

            if (DataLogger.Instance)
                DataLogger.Instance.Log("selectSlot",
                    transform.parent.transform.parent.name,                 //(unique) name of UISlot
                    ri.texture.name,                                        //which texture?
                    eventData.position.ToString(),                          //UI touch position
                    transform.parent.transform.parent.position.ToString()); //positoin of UISlot in GUI, relative to its parent.
        }
        else if (HasPlayerSelectedAnObject() && !HasPlayerSelectedGUIElement())
        {
            ReceiveTextureFromSelectedGameObject();
            ActivateDoorHighlightning();
            DeactivatePictureFrameHighlightning();
            DeactivateUISlotHighlightning();
        }

    }


    #region TransferTexture
    private void AttachPictureToPictureCanvas(GameObject pictureCanvas)
    {
        Renderer otherRenderer = pictureCanvas.GetComponent<Renderer>();

        if (DataLogger.Instance)
            DataLogger.Instance.Log("slotToFrameDrag",
                pictureCanvas.transform.root.name,              //name of current room
                pictureCanvas.transform.parent.name,            //name of picture frame
                transform.parent.transform.parent.name,         //name of UISlot
                ri.texture.name,                                //texture name of UISlot
                otherRenderer.material.name,                    //texture name of picture frame
                pictureCanvas.transform.parent.position.ToString(),     //position of picture frame
                transform.parent.transform.parent.position.ToString()); //position of UISlot

        //swap
        Texture cachedTexture = otherRenderer.material.mainTexture;
        otherRenderer.material.mainTexture = ri.texture;
        ri.texture = cachedTexture;

        if (ri.texture == null) ri.color = Color.black;
    }

    public bool HasTexture()
    {
        return ri.texture != null;
    }

    private void ReceiveTextureFromSelectedGameObject()
    {
        GameObject selectedGO = GetSelectedGameObject();
        Renderer otherRenderer = selectedGO.GetComponent<Renderer>();
        ri.color = Color.white;

        //logging select/deselect
        if (DataLogger.Instance)
        {
            if (selectedGO.tag == "PictureCanvas")
            {
                DataLogger.Instance.Log("frameToSlotClick",
                    selectedGO.transform.root.name,             //name of current room
                    selectedGO.transform.parent.name,           //name of picture frame
                    transform.parent.transform.parent.name,     //name of UISlot
                    ri.mainTexture.name,                        //texture name of UISlot
                    otherRenderer.material.name,                //texture name of picture frame
                    selectedGO.transform.parent.position.ToString(),        //position of picture frame
                    transform.parent.transform.parent.position.ToString()); //position of UISlot.

            }
            else if (selectedGO.tag == "Picture")
            {
                DataLogger.Instance.Log("picToSlotClick",
                    selectedGO.transform.name,                  //name of picture
                    transform.parent.transform.parent.name,     //name of UISlot
                    ri.mainTexture.name,                        //texture name of UISlot
                    otherRenderer.material.name,                //texture name of picture
                    selectedGO.transform.position.ToString(),               //position of picture frame
                    transform.parent.transform.parent.position.ToString()); //position of UISlot.
            }
        }

        //swap textures
        Texture cachedTexture = otherRenderer.material.mainTexture;
        otherRenderer.material.mainTexture = ri.texture;
        ri.texture = cachedTexture;


        if (selectedGO.tag == "Picture" && otherRenderer.material.mainTexture == null)
        {
            if (WinConditionManager.Instance)
                WinConditionManager.Instance.RegisterPickupOf(selectedGO.GetComponent<InteractivePicture>());

            Destroy(selectedGO);
        }
        Deselect();
    }
    #endregion TransferTexture

}
