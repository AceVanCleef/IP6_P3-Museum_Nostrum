using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractiveUISlot : AbstractInteractiveGUIElement, ITagEnsurance {

    Vector3 startPosition;

    void Start()
    {
        InitializeTag();
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
        startPosition = transform.position;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPosition;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        //RectTransform invPanel = transform as RectTransform;
        Debug.Log("drop!");
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
        GetComponent<RawImage>().texture = selectedPicture.GetComponent<Renderer>().material.mainTexture;
        selectedPicture.GetComponent<IInteractiveGameObject>().DisableOutline();
        Destroy(selectedPicture);
    }

    private void AttachPictureToPictureCanvas(GameObject pictureCanvas)
    {
        RawImage ri = GetComponent<RawImage>();
        pictureCanvas.GetComponent<Renderer>().material.mainTexture = ri.texture;
        ri.texture = null;
    }
    #endregion TransferTexture

}
