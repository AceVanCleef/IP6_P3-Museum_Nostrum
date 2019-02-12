using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractiveUISlot : AbstractInteractiveGUIElement, ITagEnsurance
{

    Vector3 startPosition;
    RawImage ri;

    private static readonly Vector2 InitialSize = new Vector2(160f, 160f);
    private static readonly Vector2 HoverSize = new Vector2(200f, 200f);
    //private static readonly Vector2 squareHoverSize = new Vector2(300f, 150f);

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
        if (HasTexture())
        {
            //Block swipes
            base.OnBeginDrag(eventData);

            //remember start position of UISlot.
            startPosition = transform.position;

            GetComponent<RectTransform>().sizeDelta = HoverSize;

            ActivatePictureFrameHighlightning();
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
            if (pictureCanvas != null)
            {
                AttachPictureToPictureCanvas(pictureCanvas);
            }
            //dataLogger.Log("SwipeDragOnDrop", eventData.position.ToString(), "-");

            //reset position.
            transform.position = startPosition;

            GetComponent<RectTransform>().sizeDelta = InitialSize;

            DeactivatePictureFrameHighlightning();

            //Reenable swipes
            base.OnEndDrag(eventData);
        }
    }



    public override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLICKING ON UI SLOT.");
        if (HasTexture() && !HasPlayerSelectedAnObject())
        {
            GameObject highlighterCarryingGUIElement = transform.parent.transform.parent.gameObject;
            Select(highlighterCarryingGUIElement.GetComponent<UISlotSelectedHighlighter>(), gameObject, true);
        }
        else if (HasPlayerSelectedAnObject() && !HasPlayerSelectedGUIElement())
        {
            ReceiveTextureFromSelectedGameObject();
        }

    }


    #region TransferTexture
    private void AttachPictureToPictureCanvas(GameObject pictureCanvas)
    {
        Renderer otherRenderer = pictureCanvas.GetComponent<Renderer>();
        //swap
        Texture cachedTexture = otherRenderer.material.mainTexture;
        otherRenderer.material.mainTexture = ri.texture;
        ri.texture = cachedTexture;

        if (ri.texture == null) ri.color = Color.black;
    }

    private bool HasTexture()
    {
        return ri.texture != null;
    }

    private void ReceiveTextureFromSelectedGameObject()
    {
        GameObject selectedGO = GetSelectedGameObject();
        Renderer otherRenderer = selectedGO.GetComponent<Renderer>();
        ri.color = Color.white;

        Texture cachedTexture = otherRenderer.material.mainTexture;
        otherRenderer.material.mainTexture = ri.texture;
        ri.texture = cachedTexture;

        if (selectedGO.tag == "Picture" && otherRenderer.material.mainTexture == null)
        {
            
            Destroy(selectedGO);
        }
        Deselect();
    }
    #endregion TransferTexture

}
