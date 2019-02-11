using System.Collections;
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

        ActivateUISlotHighlightning();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (pictureRenderer.material.mainTexture != null)
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

        Debug.Log("Caught uiSlot? " + (uiSlot != null));
        Debug.Log("Caught both pictureCanvas? " + (pictureCanvases != null));
        Debug.Log("Amt. of UIslots found: " + GetAllUIElementsWith("DraggableUI").Count);

        if (uiSlot != null)
        {
            Debug.Log("ui slot: " + uiSlot.name);
            AttachPictureToUISlot(uiSlot);
        }
        else if (CanPlayerDropPictureOnCanvasDirectly && pictureCanvases != null)
        {
            SwapTexturesOf(pictureCanvases[0], pictureCanvases[1]);
        }

        //reset position.
        transform.position = startPosition;

        DeactivateUISlotHighlightning();

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
        if (selectedGameObject == null)
        {
            selectedGameObject = gameObject;
        }
        else
        {
            //Swap focus
            selectedGameObject = gameObject;    //updating selection to this GO.
        }
        //Todo: deselect when hitting no interactive object.
    }
    #endregion SingleTap

    #endregion UserInput


    #region TransferTexture
    private void AttachPictureToUISlot(GameObject uiSlot)
    {
        RawImage ri = uiSlot.GetComponent<RawImage>();

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

        //swap
        Texture cachedTexture = otherRenderer.material.mainTexture;
        otherRenderer.material.mainTexture = thisRenderer.material.mainTexture;
        thisRenderer.material.mainTexture = cachedTexture;
    }
    #endregion TransferTexture

}
