using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnSwipeScript : MonoBehaviour {

    private LineRenderer lineRenderer;

    public Color InitColor = Color.red;
    public float StrokeWidth = 0.1f;


    private SwipeData screenPosInfo;
    private Vector3[] InitialDrawPos;

    private float zOffset = 2;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public GameObject DrawSwipe(SwipeData data)
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = Camera.main.ScreenToWorldPoint(new Vector3(data.StartPosition.x, data.StartPosition.y, zOffset));
        positions[1] = Camera.main.ScreenToWorldPoint(new Vector3(data.EndPosition.x, data.EndPosition.y, zOffset));
        lineRenderer.positionCount = 2;
        lineRenderer.material.color = InitColor;
        SetWidth(StrokeWidth, StrokeWidth * 0.25f);
        lineRenderer.SetPositions(positions);

        screenPosInfo = data;
        InitialDrawPos = positions;

        return gameObject;
    }

    public GameObject DrawSwipe(SwipeData data, Color c)
    {
        GameObject go = DrawSwipe(data);
        lineRenderer.material.color = c;

        return go;
    }

    private void SetWidth(float start, float end)
    {
        lineRenderer.startWidth = start;
        lineRenderer.endWidth = end;
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
        Vector3[] positions = new Vector3[2];
        positions[0] = Camera.main.ScreenToWorldPoint(new Vector3(screenPosInfo.StartPosition.x, screenPosInfo.StartPosition.y, zOffset));
        positions[1] = Camera.main.ScreenToWorldPoint(new Vector3(screenPosInfo.EndPosition.x, screenPosInfo.EndPosition.y, zOffset));

        lineRenderer.SetPositions(positions);
        
    }

    private void WarpBackToWorldPosition()
    {
        if (HasToWarpBack())
        {
            lineRenderer.SetPositions(InitialDrawPos);
        }
    }

    private bool HasToWarpBack()
    {
        bool startPosIdentical = Vector3.Distance(InitialDrawPos[0], Camera.main.ScreenToWorldPoint(new Vector3(screenPosInfo.StartPosition.x, screenPosInfo.StartPosition.y, zOffset))) < 0.001f;
        bool endPosIdentical = Vector3.Distance(InitialDrawPos[1], Camera.main.ScreenToWorldPoint(new Vector3(screenPosInfo.EndPosition.x, screenPosInfo.EndPosition.y, zOffset))) < 0.001f;
        return !(startPosIdentical && endPosIdentical);
    }

    #endregion InFrontOfCameraMode
}
