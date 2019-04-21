using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomManager : MonoBehaviour {

    private static CameraZoomManager instance = null;

    private Transform mainCamTransform;
    private Vector3 originalPos;
    private Transform player;
    private Vector3 prevPlayerPos;

    private float transitionDuration = 1.0f;

    private Vector3 lastPosition;

    [SerializeField][Range(0.1f, 2f)]
    private float LerpSpeed = 0.5f;

    public IEnumerator MoveCameraTo(Vector3 targetPos)
    {
        //make sure that the camera stays in front of the image.
        yield return StartCoroutine(LerpToPosition(LerpSpeed, GetPositionInfrontOfSheet(1.25f, targetPos)));
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

    IEnumerator LerpToPosition(float lerpSpeed, Vector3 newPosition, bool localPosition = false)
    {
        Debug.Log("LerpCamTo?");
        float t = 0.0f;
        
        Vector3 startingPos = mainCamTransform.position;
        if (localPosition) //override if necessary.
        {
            startingPos = mainCamTransform.localPosition;
        }

        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / lerpSpeed);
            if (localPosition)
            {
                mainCamTransform.localPosition = Vector3.Lerp(startingPos, newPosition, t);
            }
            else
            {
                mainCamTransform.position = Vector3.Lerp(startingPos, newPosition, t);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void ResetCamera()
    {
        StartCoroutine(LerpToPosition(LerpSpeed, originalPos, true));
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
        //player = UnityEngine.Object.FindObjectOfType<PlayerRotator>().transform;
        originalPos = mainCamTransform.localPosition;
        //prevPlayerPos = player.position;
	}
    /*
    private void Update()
    {
        if (Vector3.Distance(player.position, prevPlayerPos) > float.Epsilon)
        {
            prevPlayerPos = player.position;
            originalPos = mainCamTransform.localPosition;
        }
    }*/

}
