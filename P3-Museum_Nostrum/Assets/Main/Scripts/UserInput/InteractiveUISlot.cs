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

    private DataLogger dataLogger;

    protected new virtual void Start()
    {
        base.Start();

        InitializeTag();
        ri = GetComponent<RawImage>();

        //get DataLogger
        GameObject go = GameObject.Find("DataLogger");
        dataLogger = (DataLogger)go.GetComponent(typeof(DataLogger));
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

            dataLogger.Log("dragBeginSlot", gameObject.transform.parent.transform.parent.name, ri.texture.name, eventData.position.ToString(), null, null);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (HasTexture())
        {
            transform.position = Input.mousePosition;
            // dataLogger.Log("dragOnSlot", gameObject.transform.parent.transform.parent.name, ri.texture.name, eventData.position.ToString(), null);
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
            else
            {
                dataLogger.Log("dragEndSlot", gameObject.transform.parent.transform.parent.name, ri.texture.name, eventData.position.ToString(), null, null);
            }
            
            //reset position.
            transform.position = startPosition;

            GetComponent<RectTransform>().sizeDelta = InitialSize;
            //toggle highlightnings.
            Deselect();

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
            DeactivateDoorHighlightning();
            ActivatePictureFrameHighlightning();

            dataLogger.Log("selectSlot", gameObject.transform.parent.transform.parent.name, ri.texture.name, eventData.position.ToString(), null, null);
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
        dataLogger.Log("slotToFrameDrag", pictureCanvas.transform.root.name, pictureCanvas.transform.parent.name, gameObject.transform.parent.transform.parent.name, ri.texture.name, otherRenderer.material.name);
        
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
        dataLogger.Log("toSlotClick", gameObject.transform.parent.transform.parent.name, ri.mainTexture.name, otherRenderer.material.name, null, null);
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
