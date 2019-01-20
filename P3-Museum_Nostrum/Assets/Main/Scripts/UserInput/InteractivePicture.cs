using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractivePicture : AbstractInteractiveGameObject {

    private float dist;
    private Vector3 offset;
    private Vector3 v3;

    Vector3 startPosition;

    //Todo: move this variable to a game settings script.
    [SerializeField]
    private bool CanPlayerDropPictureOnCanvasDirectly = true;

    #region UserInput

    protected override void Start()
    {
        base.Start();
        //Debug.Log("InteractivePic started");
        startPosition = transform.position;
        //Your other initialization code...

    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        hitGameObject = GetHitGameObject(eventData);
        dist = CalculateDistance(hitGameObject);
        v3 = new Vector3(eventData.position.x, eventData.position.y, dist);
        v3 = Camera.main.ScreenToWorldPoint(v3);
        offset = hitGameObject.transform.position - v3;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        v3 = new Vector3(eventData.position.x, eventData.position.y, dist);
        v3 = Camera.main.ScreenToWorldPoint(v3);
        hitGameObject.transform.position = v3 + offset;
    }


    
    public override void OnEndDrag(PointerEventData eventData)
    {
        //Detect UISlot
        GameObject uiSlot = GetFirstUIElementWith("DraggableUI");
        GameObject pictureCanvas = FindPictureCanvas(eventData.position);
        Debug.Log("pictureCanvas != null?" + pictureCanvas.name);

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
    }

    private GameObject FindPictureCanvas(Vector2 pos)
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
        //different behavior when dragging with finger:
        //return base.GetHitGameObject(eventData);
        return gameObject;
    }

    private float CalculateDistance(GameObject hitGO)
    {
        return CameraViewDirection.Instance.GetCurrentState().HandleDragDistanceCalculation(hitGO.transform.position, Camera.main.transform.position);
        //return hitGO.transform.position.z - Camera.main.transform.position.z;
    }

    #endregion UserInput

    private void AttachPictureToUISlot(GameObject uiSlot)
    {
        uiSlot.GetComponent<RawImage>().texture = gameObject.GetComponent<Renderer>().material.mainTexture;
        Destroy(gameObject);
    }

    private void AttachPictureToPictureCanvas(GameObject pictureCanvas)
    {
        pictureCanvas.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<Renderer>().material.mainTexture;
        Destroy(gameObject);
    }

}
