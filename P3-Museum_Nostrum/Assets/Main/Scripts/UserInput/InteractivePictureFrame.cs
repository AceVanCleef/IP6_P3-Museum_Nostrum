﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractivePictureFrame : AbstractUIDetectingGameObject
{

    private float dist;
    private Vector3 offset;
    private Vector3 v3;

    [SerializeField]
    [Tooltip("How far away from the camera should this GO hover while draggin?")]
    private float distanceFromCamera = 2f;

    Vector3 startPosition;

    //Todo: move this variable to a game settings script.
    [SerializeField]
    private bool CanPlayerDropPictureOnCanvasDirectly = true;

    private Renderer pictureRenderer;

    protected new virtual void Start()
    {
        base.Start();

        Debug.Log("InteractivePictureFrame started");
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
            DataLogger.Instance.Log("dragBeginFrame", gameObject.transform.root.name, gameObject.transform.parent.name, pictureRenderer.material.mainTexture.name, eventData.position.ToString(), transform.parent.position.ToString());

        ActivateUISlotHighlightning();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (HasTexture())
        {
            v3 = new Vector3(eventData.position.x, eventData.position.y, GetHoverDistanceFromCamera(eventData) );
            v3 = Camera.main.ScreenToWorldPoint(v3);
            hitGameObject.transform.position = v3 + offset;
        }  
    }



    public override void OnEndDrag(PointerEventData eventData)
    {
        //Detect UISlot
        GameObject uiSlot = GetFirstUIElementWith("DraggableUI");
        GameObject[] pictureCanvases = FindBothOverlappingPictureCanvases(eventData.position);

        if (uiSlot != null)
        {
            AttachPictureToUISlot(uiSlot);
        }
        else if (CanPlayerDropPictureOnCanvasDirectly && pictureCanvases != null)
        {
            SwapTexturesOf(pictureCanvases[0], pictureCanvases[1]);
        }
        else
        {
            if(DataLogger.Instance)
                DataLogger.Instance.Log("dragEndFrame", gameObject.transform.root.name, gameObject.transform.parent.name, pictureRenderer.material.mainTexture.name, eventData.position.ToString());
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
        //return hitGO.transform.position.z - Camera.main.transform.position.z;
    }

    private float GetHoverDistanceFromCamera(PointerEventData eventData)
    {
        if (FindBothOverlappingPictureCanvases(eventData.position) != null) {
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
    /// <returns></returns>
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

    public override GameObject GetHitGameObject(PointerEventData eventData)
    {
        return gameObject;
    }

    #region SingleTap

    public override void OnPointerClick(PointerEventData eventData)
    {
        if( HasTexture() && !HasPlayerSelectedAnObject() )
        {
            IHighlighter selectionHighlighter = transform.parent.gameObject.GetComponentInChildren<PictureFrameSelectedHighlighter>();
            Select(selectionHighlighter, gameObject);
            DeactivateDoorHighlightning();
            ActivatePictureFrameHighlightning();
            ActivateUISlotHighlightning();
            //prevent overlapping highlightning of this picture frame.
            transform.parent.gameObject.GetComponentInChildren<PictureFrameHighlighter>().Off();
            selectionHighlighter.On();

            //Todo
            //dataLogger.Log("selectFrame", gameObject.transform.root.name, gameObject.transform.parent.name, pictureRenderer.material.mainTexture.name, eventData.position.ToString(), null);
        }
        //receiving a texture
        else if ( HasPlayerSelectedAnObject() )
        {
            if(HasPlayerSelectedGUIElement() )
            {
                ReceiveTextureSelectedFromGUIElement();
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
                uiSlot.transform.parent.transform.parent.name, 
                gameObject.transform.root.name, 
                gameObject.transform.parent.name, 
                pictureRenderer.material.mainTexture.name, 
                ri.mainTexture.name, 
                uiSlot.transform.parent.transform.parent.position.ToString(),
                transform.parent.position.ToString());
        
        //Swap
        Texture cachedTexture = ri.texture;
        ri.texture = pictureRenderer.material.mainTexture;
        pictureRenderer.material.mainTexture = cachedTexture;

        if (ri.texture != null)
        {
            ri.color = Color.white;
        }

        
    }

    private void SwapTexturesOf(GameObject thisCanvas, GameObject otherCanvas)
    {
        Debug.Log("Executing SwapTexturesOf()");
        Renderer thisRenderer = thisCanvas.GetComponent<Renderer>();
        Renderer otherRenderer = otherCanvas.GetComponent<Renderer>();
        Debug.Log("ohterCanves" + otherCanvas.name);

        if(otherCanvas.name == "16:9 Background")
        {
            //todo: does this make sense?
            DataLogger.Instance.Log("frameToFrame", gameObject.transform.root.name, otherCanvas.transform.parent.name, gameObject.transform.parent.name, thisRenderer.material.ToString(), otherRenderer.material.ToString());
        }
        else
        {
            DataLogger.Instance.Log("picToFrameClick", otherCanvas.transform.name, gameObject.transform.root.name, gameObject.transform.parent.name,  thisRenderer.material.ToString(), otherRenderer.material.ToString());
        }
        
       
        //swap
        Texture cachedTexture = otherRenderer.material.mainTexture;
        otherRenderer.material.mainTexture = thisRenderer.material.mainTexture;
        thisRenderer.material.mainTexture = cachedTexture;

        
    }

    private bool HasTexture()
    {
        return pictureRenderer.material.mainTexture != null;
    }

    private void ReceiveTextureSelectedFromGUIElement()
    {
        RawImage ri = GetSelectedGameObject().GetComponent<RawImage>();
        Renderer renderer = GetComponent<Renderer>();

        //Todo:
        /*dataLogger.Log("slotToFrameClick", gameObject.transform.root.name, gameObject.transform.parent.name,
            renderer.material.ToString(), 
            ri.texture.name, null);*/

        //swap
        Texture cachedTexture = renderer.material.mainTexture;
        renderer.material.mainTexture = ri.texture;
        ri.texture = cachedTexture;

        if (ri.texture == null) ri.color = Color.black;

        Deselect();
    }

    private void ReceiveTextureFromSelectedGameObject()
    {
        GameObject selectedGO = GetSelectedGameObject();
        SwapTexturesOf(gameObject, selectedGO);

        //Todo: does this make sense? from pictureframe vs. from interactive picture.
        /*if (DataLogger.Instance)
            DataLogger.Instance.Log("frameToFrameClick", 
                gameObject.transform.root.name, 
                selectedGO.transform.parent.name, 
                gameObject.transform.parent.name, 
                gameObject.GetComponent<Renderer>().material.ToString(), 
                selectedGO.GetComponent<Renderer>().material.ToString());*/


        Renderer otherRenderer = selectedGO.GetComponent<Renderer>();
        if (selectedGO.tag == "Picture" && otherRenderer.material.mainTexture == null)
        {
            Destroy(selectedGO);
        }
        Deselect();
    }
    #endregion TransferTexture

}
