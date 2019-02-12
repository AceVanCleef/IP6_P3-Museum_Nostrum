using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractivePicture : AbstractUIDetectingGameObject {

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

    protected override void Start()
    {
        base.Start();
        //Debug.Log("InteractivePic started");
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
        dist = CalculateDistance(hitGameObject);
        v3 = new Vector3(eventData.position.x, eventData.position.y, dist);
        v3 = Camera.main.ScreenToWorldPoint(v3);
        offset = hitGameObject.transform.position - v3;

        ActivateUISlotHighlightning();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        v3 = new Vector3(eventData.position.x, eventData.position.y, GetHoverDistanceFromCamera(eventData) );
        v3 = Camera.main.ScreenToWorldPoint(v3);
        hitGameObject.transform.position = v3 + offset;
    }


    
    public override void OnEndDrag(PointerEventData eventData)
    {
        //Detect UISlot
        GameObject uiSlot = GetFirstUIElementWith("DraggableUI");
        GameObject pictureCanvas = FindPictureCanvas(eventData.position);

        if (uiSlot != null)
        {
            Debug.Log("ui slot: " + uiSlot.name);
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

    public override GameObject GetHitGameObject(PointerEventData eventData)
    {
        return gameObject;
    }

    #region SingleTap

    public override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicking on Picture.");
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
        if (ri.texture == null)
        {
            ri.texture = pictureRenderer.material.mainTexture;
            ri.color = Color.white;
            Destroy(gameObject);
        }
        else
        {
            //reset position.
            transform.position = startPosition;

            ri.color = Color.white;

            //Swap
            Texture cachedTexture = ri.texture;
            ri.texture = pictureRenderer.material.mainTexture;
            pictureRenderer.material.mainTexture = cachedTexture;
        }
    }

    private void AttachPictureToPictureCanvas(GameObject pictureCanvas)
    {
        Renderer otherRenderer = pictureCanvas.GetComponent<Renderer>();

        //swap
        Texture cachedTexture = otherRenderer.material.mainTexture;
        otherRenderer.material.mainTexture = pictureRenderer.material.mainTexture;
        pictureRenderer.material.mainTexture = cachedTexture;

        if (pictureRenderer.material.mainTexture == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = startPosition;
        }

        //Todo: If pictureCanvas already carries a texture, either...
        //- swap textures and reset InteractivePicture's position
        //- drop texture of pictureCanvas to inventory slot and then 
        //  set texture of this InteractivePicture to pictureCanvas.
    }
    #endregion TransferTexture
}
