using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// handles user input for and texture swapping of this interactive picture frame.
/// </summary>
public class InteractivePictureFrame : AbstractUIDetectingGameObject
{
    //required by drag gesture handling.
    private float dist;
    private Vector3 offset;
    private Vector3 v3;

    [SerializeField]
    [Tooltip("How far away from the camera should this GO hover while draggin?")]
    private float distanceFromCamera = 2f;

    Vector3 startPosition;

    [SerializeField]
    private bool CanPlayerDropPictureOnCanvasDirectly = true;

    private Renderer pictureRenderer;

    protected new virtual void Start()
    {
        base.Start();

        //Debug.Log("InteractivePictureFrame started");
        startPosition = transform.position;
        //Your other initialization code...

        pictureRenderer = GetComponent<Renderer>();

    }


    #region UserInput

    #region DragAndDrop

    public override void OnBeginDrag(PointerEventData eventData)
    {
        //Block swipes
        base.OnBeginDrag(eventData);

        hitGameObject = GetHitGameObject(eventData);
        dist = CalculateDistance(hitGameObject);    //todo: bring dragging GO closer to camera.
        Debug.Log("distance: " + dist);
        v3 = new Vector3(eventData.position.x, eventData.position.y, dist);
        v3 = Camera.main.ScreenToWorldPoint(v3);
        offset = hitGameObject.transform.position - v3;

        if (DataLogger.Instance)
            DataLogger.Instance.Log("dragBeginFrame",
                transform.root.name,                        //room name
                transform.parent.name,                      //name of the picture frame
                ((pictureRenderer.material.mainTexture) ? pictureRenderer.material.mainTexture.name : ""),  //which texture?
                eventData.position.ToString(),              //UI touch position
                transform.parent.position.ToString());      //picture frame's position relative to the room's position.

        ActivateUISlotHighlightning();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (HasTexture())
        {
            v3 = new Vector3(eventData.position.x, eventData.position.y, GetHoverDistanceFromCamera(eventData));
            v3 = Camera.main.ScreenToWorldPoint(v3);
            hitGameObject.transform.position = v3 + offset;
        }
    }



    public override void OnEndDrag(PointerEventData eventData)
    {
        //Detect UISlot
        GameObject uiSlot = GetFirstUIElementWith("DraggableUI");
        GameObject[] pictureCanvases = FindBothOverlappingPictureCanvases(eventData.position);

        OnEndDragNext(eventData, uiSlot, pictureCanvases, startPosition);
    }

    /// <summary>
    /// OnEndDrag callback.
    /// </summary>
    /// <remarks>requiered by and interfacing with ReplayManager.</remarks>
    /// <param name="eventData"></param>
    /// <param name="uiSlot"></param>
    /// <param name="pictureCanvases"></param>
    /// <param name="startPos"></param>
    public void OnEndDragNext(PointerEventData eventData, GameObject uiSlot, GameObject[] pictureCanvases, Vector3 startPos)
    {

        if (!pictureRenderer)
            pictureRenderer = GetComponent<Renderer>();

        if (uiSlot != null)
        {
            AttachPictureToUISlot(uiSlot);
        }
        else if (CanPlayerDropPictureOnCanvasDirectly && pictureCanvases != null)
        {
            //log a swap between two picture frames.
            if (DataLogger.Instance)
            {
                Renderer thisRenderer = pictureCanvases[0].GetComponent<Renderer>();
                Renderer otherRenderer = pictureCanvases[1].GetComponent<Renderer>();

                DataLogger.Instance.Log("frameToFrameDrag",
                    transform.root.name,                            //name of current room.
                    pictureCanvases[0].transform.parent.name,       //name of 1st picture frame
                    pictureCanvases[1].transform.parent.name,       //name of 2nd picture frame
                    thisRenderer.material.ToString(),               //name of 1st texture involved
                    otherRenderer.material.ToString(),              //name of 2nd texture involved
                    pictureCanvases[0].transform.parent.position.ToString(),    //position of 1st picture frame relative to its room.
                    pictureCanvases[1].transform.parent.position.ToString());   //position of 2nd picture frame relative to its room.
            }

            SwapTexturesOf(pictureCanvases[0], pictureCanvases[1]);
        }
        else
        {
            if (DataLogger.Instance)
                DataLogger.Instance.Log("dragEndFrame",
                    transform.root.name,                        //name of current room
                    transform.parent.name,                      //name of picture frame
                    ((pictureRenderer.material.mainTexture) ? pictureRenderer.material.mainTexture.name : ""),  //texture name of picture frame
                    eventData.position.ToString(),              //end position of drag motion.
                    transform.parent.position.ToString());      //position of picture frame relative to its room.
        }

        //reset position.
        transform.position = startPosition;

        //toggle highlightnings.
        Deselect();

        //Reenable swipes
        base.OnEndDrag(eventData);

    }

    private float CalculateDistance(GameObject hitGO)
    {
        return CameraViewDirection.Instance.GetCurrentState().HandleDragDistanceCalculation(hitGO.transform.position, Camera.main.transform.position);
        //Debugging:
        //return hitGO.transform.position.z - Camera.main.transform.position.z;
    }

    private float GetHoverDistanceFromCamera(PointerEventData eventData)
    {
        if (FindBothOverlappingPictureCanvases(eventData.position) != null)
        {
            return dist - 0.05f;
            // Note: A CameraViewDirection state based solution could be implemented where this PictureFrame Background will
            // hover slightly in front of the other PictureFrame. But since Picture Frames are supposed to be positioned 
            // slightly in front of the wall, this simple solution should suffice.
        }
        //Todo: return dist - 0.05f when picture is above the frame the user picked it up from.

        return dist - (dist - distanceFromCamera);  //e.g. 4.5 - (4.5 - 1) = 4.5 - 3.5 = 1 (unit away from camera).
    }

    #endregion DragAndDrop


    /// <summary>
    /// returns the first two detected GameObject tagged "PictureCanvas".
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>the first two detected GameObjects</returns>
    public static GameObject[] FindBothOverlappingPictureCanvases(Vector2 pos)
    {
        GameObject[] pictureCanvases = new GameObject[2];
        int j = 0;

        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; ++i)
        {
            // Note: The raycast hits overlapping GameObjects.
            if (hits[i].collider.tag == "PictureCanvas")
            {
                pictureCanvases[j] = hits[i].collider.gameObject;
                ++j;
                if (j == pictureCanvases.Length)
                    return pictureCanvases; //Therefore, the hovering and the target pictureCanvas are returned.
            }
        }
        return null;
    }


    /// <summary>
    /// returns the hit GameObject.
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public override GameObject GetHitGameObject(PointerEventData eventData)
    {
        return gameObject;
    }

    #region SingleTap

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (DataLogger.Instance)
        {
            //draw a touch on GUI.
            DataLogger.Instance.Log("touch", "On InteractivePictureFrame", eventData.position.ToString());
        }

        if (HasTexture() && !HasPlayerSelectedAnObject())
        {
            IHighlighter selectionHighlighter = transform.parent.gameObject.GetComponentInChildren<PictureFrameSelectedHighlighter>();
            Select(selectionHighlighter, gameObject);
            DeactivateDoorHighlightning();
            ActivatePictureFrameHighlightning();
            ActivateUISlotHighlightning();
            //prevent overlapping highlightning of this picture frame.
            transform.parent.gameObject.GetComponentInChildren<PictureFrameHighlighter>().Off();
            selectionHighlighter.On();

            if (DataLogger.Instance)
                DataLogger.Instance.Log("selectFrame",
                    transform.root.name,                        //room name
                    transform.parent.name,                      //frame name
                    pictureRenderer.material.mainTexture.name,  //which texture
                    eventData.position.ToString(),              //UI touch position
                    transform.parent.position.ToString());      //frame position relative to its room.
        }
        //receiving a texture
        else if (HasPlayerSelectedAnObject())
        {
            if (HasPlayerSelectedGUIElement())
            {
                ReceiveTextureFromSelectedGUIElement();
                ActivateDoorHighlightning();
                DeactivatePictureFrameHighlightning();

            }
            else
            {
                ReceiveTextureFromSelectedGameObject();
                ActivateDoorHighlightning();
                DeactivatePictureFrameHighlightning();
                DeactivateUISlotHighlightning();
            }

        }
    }
    #endregion SingleTap

    #endregion UserInput


    #region TransferTexture
    private void AttachPictureToUISlot(GameObject uiSlot)
    {
        RawImage ri = uiSlot.GetComponent<RawImage>();

        if (DataLogger.Instance)
            DataLogger.Instance.Log("frameToSlotDrag",
                uiSlot.transform.parent.transform.parent.name,  //name of target UISlot
                transform.root.name,                            //name of the room which is parent of this frame
                transform.parent.name,                          //name of this frame
                pictureRenderer.material.mainTexture.name,      //name of textures used by this frame.
                ri.mainTexture.name,                            //name of texture used by UISlot
                uiSlot.transform.parent.transform.parent.position.ToString(),   //position of UIslot relative to Inventory GO (parent).
                transform.parent.position.ToString());          //position of this frame.

        //Swap
        Texture cachedTexture = ri.texture;
        ri.texture = pictureRenderer.material.mainTexture;
        pictureRenderer.material.mainTexture = cachedTexture;

        if (ri.texture != null)
        {
            ri.color = Color.white;
        }

    }

    /// <summary>
    /// swaps textures between two gameObjects.
    /// </summary>
    /// <param name="thisCanvas"></param>
    /// <param name="otherCanvas"></param>
    private void SwapTexturesOf(GameObject thisCanvas, GameObject otherCanvas)
    {
        Renderer thisRenderer = thisCanvas.GetComponent<Renderer>();
        Renderer otherRenderer = otherCanvas.GetComponent<Renderer>();
        Debug.Log("ohterCanves" + otherCanvas.name);

        //swap
        Texture cachedTexture = otherRenderer.material.mainTexture;
        otherRenderer.material.mainTexture = thisRenderer.material.mainTexture;
        thisRenderer.material.mainTexture = cachedTexture;
    }

    private bool HasTexture()
    {
        return pictureRenderer.material.mainTexture != null;
    }

    private void ReceiveTextureFromSelectedGUIElement()
    {
        RawImage ri = GetSelectedGameObject().GetComponent<RawImage>();
        Renderer renderer = GetComponent<Renderer>();

        if (DataLogger.Instance)
            DataLogger.Instance.Log("slotToFrameClick",
                gameObject.transform.root.name,             //name of current room.
                transform.parent.name,                      //name of receiving picture frame
                ri.transform.parent.transform.parent.name,  //name of UISlot delivering a texture
                renderer.material.ToString(),               //texture name of this picture frame
                ri.texture.name,                            //texture name of UISlot's texture.
                transform.parent.position.ToString(),       //position of picture frame
                ri.transform.parent.transform.parent.position.ToString());  //position of UISlot

        //swap
        Texture cachedTexture = renderer.material.mainTexture;
        renderer.material.mainTexture = ri.texture;
        ri.texture = cachedTexture;

        if (ri.texture == null) ri.color = Color.black;

        Deselect();
    }

    private void ReceiveTextureFromSelectedGameObject()
    {
        //selectedGO delivers the texture needed for a texture swap.
        GameObject selectedGO = GetSelectedGameObject();
        SwapTexturesOf(gameObject, selectedGO);

        //log select/deselect
        if (DataLogger.Instance)
        {
            //log receiving from other picture frame.
            if (selectedGO.tag == "PictureCanvas")
            {
                DataLogger.Instance.Log("frameToFrameClick",
                    gameObject.transform.root.name,                 //name of current room.
                    selectedGO.transform.parent.name,               //name of texture delivering GO
                    gameObject.transform.parent.name,               //name of texture receiving GO
                    gameObject.GetComponent<Renderer>().material.ToString(),    //texture of receiving GO
                    selectedGO.GetComponent<Renderer>().material.ToString(),    //texture of delivering GO
                    gameObject.transform.parent.position.ToString(),    //position of receiving GO
                    selectedGO.transform.parent.position.ToString());   //position of delivering GO
            }
            ////log receiving from interactive picture.
            else if (selectedGO.tag == "Picture")
            {
                DataLogger.Instance.Log("picToFrameClick",
                    selectedGO.transform.name,              //name of interactive picture
                    gameObject.transform.root.name,         //name of current room.
                    gameObject.transform.parent.name,       //name of picture frame, recieving the texture
                    gameObject.GetComponent<Renderer>().material.ToString(),    //texture of receiving GO
                    selectedGO.GetComponent<Renderer>().material.ToString(),    //texture of delivering GO
                    selectedGO.transform.position.ToString(),   //position of delivering picture.
                    transform.parent.position.ToString());      //position of receiving picture frame.
            }

        }


        Renderer otherRenderer = selectedGO.GetComponent<Renderer>();
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
