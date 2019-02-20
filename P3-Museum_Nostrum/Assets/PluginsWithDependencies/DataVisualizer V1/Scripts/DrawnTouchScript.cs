using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnTouchScript : MonoBehaviour {

    public Color InitColor = Color.blue;
    public float radius = 0.5f;
    private float zOffset = 1.75f;
    private SpriteRenderer sr;

    private Vector2 screenPos;
    private Vector3 InitialDrawPos;
    private Quaternion OriginalOrientation;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public GameObject DrawTouch(Vector2 pos)
    {
        sr.color = InitColor;
        screenPos = pos;

        gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, zOffset));
        transform.localScale = new Vector3(radius, radius, transform.localScale.z);
        
        //rotates sprite towards camera:
        transform.rotation = new Quaternion(0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w);

        //source for rotation:
        // https://stackoverflow.com/questions/22696782/placing-an-object-in-front-of-the-camera 

        InitialDrawPos = transform.position;
        OriginalOrientation = transform.rotation;
        return gameObject;
    }

    public void DrawTouch(Vector2 pos, Color c)
    {
        DrawTouch(pos);
        sr.color = c;
    }

    #region InFrontOfCameraMode

    public bool HasToReposition = false;
    void Update()
    {
        if (HasToReposition)
        {
            RepositionInFrontOfCamera();
        }
        else
        {
            WarpBackToWorldPosition();
        }
    }

    private void RepositionInFrontOfCamera()
    {
        transform.rotation = new Quaternion(0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w);
        gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, zOffset));
    }

    private void WarpBackToWorldPosition()
    {
        if (Vector3.Distance(transform.position, InitialDrawPos) < 0.001f) return;
        transform.position = InitialDrawPos;
        transform.rotation = OriginalOrientation;
    }

    #endregion InFrontOfCameraMode
}
