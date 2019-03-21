using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// handles user input on this interactive picture.
/// </summary>
public class InteractivePicture : AbstractUIDetectingGameObject, ITagEnsurance
{
    //required for dragging the interactive picture.
    private float dist;
    private Vector3 offset;
    private Vector3 v3;

    [SerializeField]
    [Tooltip("How far away from the camera should this GO hover while draggin?")]
    private float distanceFromCamera = 2f;

    /// <summary>
    /// stores the start position of the picture in 3D space.
    /// </summary>
    Vector3 startPosition;

    [SerializeField]
    private bool CanPlayerDropPictureOnCanvasDirectly = true;

    private Renderer pictureRenderer;

    protected override void Start()
    {
        base.Start();
        InitializeTag();
        //Debug.Log("InteractivePic started");
        startPosition = transform.position;
        //Your other initialization code...

        pictureRenderer = GetComponent<Renderer>();

    }

    public void InitializeTag()
    {
        if (gameObject.tag != "Picture")
        {
            gameObject.tag = "Picture";
        }
    }

    #region UserInput

    #region DragAndDrop

    public override void OnBeginDrag(PointerEventData eventData)
    {
        //Block swipes
        base.OnBeginDrag(eventData);

        hitGameObject = GetHitGameObject(eventData);
        dist = CalculateDistance(hitGameObject);
        v3 = new Vector3(eventData.position.x, eventData.position.y, dist);
        v3 = Camera.main.ScreenToWorldPoint(v3);
        offset = hitGameObject.transform.position - v3;

        ActivateUISlotHighlightning();

        if (DataLogger.Instance)
            DataLogger.Instance.Log("dragBeginPic",
                gameObject.name,                            //which picture?
                pictureRenderer.material.ToString(),        //which texture?
                eventData.position.ToString(),              //UI touch position
                transform.position.ToString());             //picture's position in 3D world space (since Picture_holder is at (0,0,0)).
    }

    public override void OnDrag(PointerEventData eventData)
    {
        v3 = new Vector3(eventData.position.x, eventData.position.y, GetHoverDistanceFromCamera(eventData));
        v3 = Camera.main.ScreenToWorldPoint(v3);
        hitGameObject.transform.position = v3 + offset;
    }



    public override void OnEndDrag(PointerEventData eventData)
    {
        //Detect UISlot
        GameObject uiSlot = GetFirstUIElementWith("DraggableUI");

        OnEndDragNext(eventData, uiSlot);
    }

    /// <summary>
    /// OnEndDrag callback.
    /// </summary>
    /// <remarks>requiered by and interfacing with ReplayManager.</remarks>
    /// <param name="eventData">required OnEndDrag event data</param>
    /// <param name="uiSlot">the hit UISlot GameObject</param>
    public void OnEndDragNext(PointerEventData eventData, GameObject uiSlot)
    {

        GameObject pictureCanvas = FindPictureCanvas(eventData.position);
        if (uiSlot != null)
        {
            AttachPictureToUISlot(uiSlot);
        }
        else if (CanPlayerDropPictureOnCanvasDirectly && pictureCanvas != null)
        {
            AttachPictureToPictureCanvas(pictureCanvas);
        }
        else
        {
            //if not hit, reset position => User gets feedback about what a picture can interact with.
            transform.position = startPosition;
        }

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
        GameObject pc = FindPictureCanvas(eventData.position);
        if (pc != null)
        {
            float deltaDistToPictureFrame = 0.05f;
            switch (CameraViewDirection.Instance.GetCurrentState().GetDirectionIdentifier())
            {
                case Direction.North: return pc.transform.parent.transform.position.z - deltaDistToPictureFrame;
                case Direction.East: return pc.transform.parent.transform.position.x - deltaDistToPictureFrame;
                case Direction.South: return (pc.transform.parent.transform.position.z + deltaDistToPictureFrame) * -1f;
                case Direction.West: return (pc.transform.parent.transform.position.x + deltaDistToPictureFrame) * -1f;
            }
        }

        return dist - (dist - distanceFromCamera);  //e.g. 4.5 - (4.5 - 1) = 4.5 - 3.5 = 1 (unit away from camera).
    }

    #endregion DragAndDrop


    /// <summary>
    /// returns the first found PictureCanvas at screen pos.
    /// </summary>
    /// <param name="pos">Screen position</param>
    /// <returns></returns>
    public static GameObject FindPictureCanvas(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; ++i)
        {
            if (hits[i].collider.tag == "PictureCanvas")
            {
                return hits[i].collider.gameObject;
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
        Select(GetComponentInChildren<PictureSelectedHighlighter>(), gameObject);
        DeactivateDoorHighlightning();
        ActivatePictureFrameHighlightning();
        ActivateUISlotHighlightning();

        if (DataLogger.Instance)
        {
            //draw a touch on GUI.
            DataLogger.Instance.Log("touch", "On InteractivePicture", eventData.position.ToString());
            //Inform game logic to selectPic.
            DataLogger.Instance.Log("selectPic",
                gameObject.name,
                pictureRenderer.material.mainTexture.ToString(),
                eventData.position.ToString(),
                transform.position.ToString());
        }
    }

    #endregion SingleTap

    #endregion UserInput


    #region TransferTexture
    private void AttachPictureToUISlot(GameObject uiSlot)
    {
        RawImage ri = uiSlot.GetComponent<RawImage>();

        //checks whether a uni- or bi-directional exchange of textures has happened.
        if (ri.texture == null)
        {
            if (!pictureRenderer)
                pictureRenderer = GetComponent<Renderer>();

            //unidirectional exchange
            ri.texture = pictureRenderer.material.mainTexture;
            ri.color = Color.white;

            if (DataLogger.Instance)
                DataLogger.Instance.Log("picToSlotDrag",            //includes destroying gameObject
                    uiSlot.transform.parent.transform.parent.name,  //name of UISlot
                    transform.name,                                 //picture's name
                    pictureRenderer.material.mainTexture.ToString(), //texture of this picture gameObject
                    transform.position.ToString(),                  //picture's position in 3D world space.
                    uiSlot.transform.parent.transform.parent.position.ToString());  //UISlot's position in UI relative to its parent.

            if (WinConditionManager.Instance)
                WinConditionManager.Instance.RegisterPickupOf(GetComponent<InteractivePicture>());

            Destroy(gameObject);
        }
        else
        {
            //bidirectional exchange

            //reset position.
            transform.position = startPosition;

            //turn UISlot from black to white.
            ri.color = Color.white;

            //Swap textures
            Texture cachedTexture = ri.texture;
            ri.texture = pictureRenderer.material.mainTexture;
            pictureRenderer.material.mainTexture = cachedTexture;

            if (DataLogger.Instance)
                DataLogger.Instance.Log("picToSlotDragSwap",            //places picture gameObject back to startPosition
                    uiSlot.transform.parent.transform.parent.name,      //name of UISlot
                    transform.name,                                     //picture's name
                    ri.texture.name,                                    //texture's name of UISlot
                    pictureRenderer.material.mainTexture.ToString(),    //texture's name of picture gameObject
                    transform.position.ToString(),                      //picture's position
                    uiSlot.transform.parent.transform.parent.position.ToString());  //UISlot's position relative to its parent.
        }
    }

    /// <summary>
    /// transfers the texture of this interactive picture to the target pictureCanvas.
    /// </summary>
    /// <param name="pictureCanvas">target picture canvas</param>
    public void AttachPictureToPictureCanvas(GameObject pictureCanvas)
    {
        
        Renderer otherRenderer = pictureCanvas.GetComponent<Renderer>();
        
        //swap
        Texture cachedTexture = otherRenderer.material.mainTexture;
        otherRenderer.material.mainTexture = pictureRenderer.material.mainTexture;
        pictureRenderer.material.mainTexture = cachedTexture;

        if (DataLogger.Instance)
            DataLogger.Instance.Log("picToFrameDrag",
                pictureCanvas.transform.root.name,              //room name
                pictureCanvas.transform.parent.name,            //target frame's name
                otherRenderer.material.mainTexture.ToString(),  //texture of frame
                transform.name,                                 //name of picture
                pictureRenderer.material.ToString(),            //texture of picture
                pictureCanvas.transform.parent.position.ToString(), //position of target frame relative to its room.
                transform.position.ToString());                 //picture's position in 3D world space.

        //checks whether a uni- or bi-directional exchange of textures has happened.
        if (pictureRenderer.material.mainTexture == null)
        {
            //unidirectional exchange
            if (WinConditionManager.Instance)
                WinConditionManager.Instance.RegisterPickupOf(GetComponent<InteractivePicture>());

            Destroy(gameObject);
        }
        else
        {
            //bidirectoinal exchange
            transform.position = startPosition;
        }
    }

    #endregion TransferTexture
}
