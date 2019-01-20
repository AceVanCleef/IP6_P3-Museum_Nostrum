using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayCastTest : MonoBehaviour {

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    // Update is called once per frame
    void Update () {
        if (Input.touchCount != 1)
        {
            return;
        }

        int fingerID = 0;
        Touch touch = Input.touches[fingerID];
        Vector2 pos = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            fingerUpPosition = touch.position;
            fingerDownPosition = touch.position;

            if (EventSystem.current.IsPointerOverGameObject(fingerID))    // is the touch on the GUI
            {
                // GUI Action
                //Debug.Log("Hitting UI Image: " + EventSystem.current.currentSelectedGameObject.name);
                return;
            }

            //shoot raycast.
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject != null)
            {
                
            }
        }
    }
}
