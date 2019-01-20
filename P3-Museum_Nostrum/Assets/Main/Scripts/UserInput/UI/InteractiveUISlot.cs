using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        //Detect UISlot

        //if not hit, reset position => User gets feedback about what a picture can interact with.
        transform.position = startPosition;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;
        Debug.Log("drop!");
        /*GameObject pictureFrame = FindPictureFrame();
        if (pictureFrame != null)
        {
            AttachUISpriteToFrameBackground(pictureFrame);
        }*/
        //dataLogger.Log("SwipeDragOnDrop", eventData.position.ToString(), "-");
    }
}
