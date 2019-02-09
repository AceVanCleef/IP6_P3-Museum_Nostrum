﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractiveUISlot : AbstractInteractiveGUIElement, ITagEnsurance {

    Vector3 startPosition;
    RawImage ri;

    void Start()
    {
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
        //Block swipes
        base.OnBeginDrag(eventData);

        //remember start position of UISlot.
        startPosition = transform.position;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (ri.texture != null)
        {
            transform.position = Input.mousePosition;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        //reset position.
        transform.position = startPosition;

        //Reenable swipes
        base.OnEndDrag(eventData);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        GameObject pictureCanvas = InteractivePicture.FindPictureCanvas(eventData.position);
        if (pictureCanvas != null)
        {
            AttachPictureToPictureCanvas(pictureCanvas);
        }
        //dataLogger.Log("SwipeDragOnDrop", eventData.position.ToString(), "-");
    }



    public override void OnPointerClick(PointerEventData eventData)
    {
        //Todo: Fix / finish image selection by tap. Issue seems to be that the raycast might get stuck in another UI element above.
        // Other potential reasons are unkowns so far.
        // Follow up - todo: When image is inUISlot, player should be able to select it and put it into the picturecanvas via second tap.

        Debug.Log("CLICKING ON UI SLOT.");
        if(AbstractInteractiveGameObject.SelectedGameObject != null)
        {
            Debug.Log("CLICKING ON UI SLOT. 222");
            AttachPictureToUISlot( AbstractInteractiveGameObject.SelectedGameObject );
        }
    }


    #region TransferTexture
    private void AttachPictureToUISlot(GameObject selectedPicture)
    {
        ri.texture = selectedPicture.GetComponent<Renderer>().material.mainTexture;
        selectedPicture.GetComponent<IInteractiveGameObject>().DisableOutline();
        Destroy(selectedPicture);
    }

    private void AttachPictureToPictureCanvas(GameObject pictureCanvas)
    {
        Renderer otherRenderer = pictureCanvas.GetComponent<Renderer>();
        //swap
        Texture cachedTexture = otherRenderer.material.mainTexture;
        otherRenderer.material.mainTexture = ri.texture;
        ri.texture = cachedTexture;
    }
    #endregion TransferTexture

}
