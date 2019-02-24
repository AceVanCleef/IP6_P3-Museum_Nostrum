using UnityEngine;

/// <summary>
/// draws the view field of the player camera.
/// </summary>
[ExecuteInEditMode]
public class DrawPlayerViewfield : MonoBehaviour {

    //source: https://answers.unity.com/questions/1159913/setting-to-show-camera-cone-always.html

    private static DrawPlayerViewfield instance = null;

    private Camera cameraShowFrustumAlways;

    [Tooltip("Toggle the main camera gizmos on/off.")]
    public bool DrawCameraGizmos = true;

    private void Awake()
    {
        //ensure singleton.
        if (!instance)
        {
            instance = this;
            cameraShowFrustumAlways = Camera.main;
        }
    }

    private void OnDrawGizmos()
    {
        if (DrawCameraGizmos && cameraShowFrustumAlways)
        {
            Gizmos.matrix = cameraShowFrustumAlways.transform.localToWorldMatrix;
            Gizmos.DrawFrustum(cameraShowFrustumAlways.transform.position,
                cameraShowFrustumAlways.fieldOfView,
                cameraShowFrustumAlways.farClipPlane,
                cameraShowFrustumAlways.nearClipPlane,
                cameraShowFrustumAlways.aspect);
        }
    }

}
