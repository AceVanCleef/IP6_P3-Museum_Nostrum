using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomManager : MonoBehaviour {

    private static CameraZoomManager instance = null;

    private Transform mainCamTransform;
    private Vector3 originalPos;

    private float transitionDuration = 1.0f;

    private Vector3 lastPosition;

    [SerializeField][Range(0.1f, 2f)]
    private float LerpSpeed = 0.5f;

    public IEnumerator MoveCameraTo(Vector3 targetPos)
    {

        Debug.Log("MoveCamTo? " + mainCamTransform.name);
        //make sure that the camera stays in front of the image.

        yield return StartCoroutine(LerpToPosition(LerpSpeed, GetPositionInfrontOfSheet(1.25f, targetPos)));

        Debug.Log("done zooming");
    }

    private Vector3 GetPositionInfrontOfSheet(float dist, Vector3 originalTargetPos)
    {
        switch(CameraViewDirection.Instance.GetCurrentState().GetDirectionIdentifier())
        {
            case Direction.North:
                return new Vector3(originalTargetPos.x, originalTargetPos.y, originalTargetPos.z - dist);
            case Direction.East:
                return new Vector3(originalTargetPos.x - dist, originalTargetPos.y, originalTargetPos.z);
            case Direction.South:
                return new Vector3(originalTargetPos.x, originalTargetPos.y, originalTargetPos.z + dist);
            case Direction.West:
                return new Vector3(originalTargetPos.x + dist, originalTargetPos.y, originalTargetPos.z);
        }
        return originalTargetPos;
    }

    IEnumerator LerpToPosition(float lerpSpeed, Vector3 newPosition)
    {
        Debug.Log("LerpCamTo?");
        float t = 0.0f;
        Vector3 startingPos = mainCamTransform.position;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / lerpSpeed);
            mainCamTransform.position = Vector3.Lerp(startingPos, newPosition, t);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ResetCamera()
    {
        StartCoroutine(LerpToPosition(LerpSpeed, originalPos));
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        mainCamTransform = Camera.main.transform;
        originalPos = mainCamTransform.position;
	}

}
