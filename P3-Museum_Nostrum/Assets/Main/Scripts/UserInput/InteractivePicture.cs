using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractivePicture : AbstractInteractiveGameObject {

    private float dist;
    private Vector3 offset;
    private Vector3 v3;

    GameObject hitGameObject;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("hit go == null? " + GetHitGameObject(eventData) == null);
        Debug.Log("hitGo name: " + GetHitGameObject(eventData).name);
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

    /*
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;
        Debug.Log("drop!");
        GameObject pictureFrame = FindPictureFrame();
        if (pictureFrame != null)
        {
            AttachUISpriteToFrameBackground(pictureFrame);
        }
        dataLogger.Log("SwipeDragOnDrop", eventData.position.ToString(), "-");
    }*/
}
