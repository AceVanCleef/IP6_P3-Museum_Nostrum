using UnityEngine;

/// <summary>
/// draws the gizmos of the referenced camera in the editor.
/// </summary>
[ExecuteInEditMode]
public class FrustrumAlways : MonoBehaviour {

    //source: https://answers.unity.com/questions/1159913/setting-to-show-camera-cone-always.html

    [Tooltip("The camera which's gizmos have to be always drawn.")]
    public Camera cameraShowFrustumAlways;

    [Tooltip("Toggle the camera gizmos on/off.")]
    public bool DrawCameraGizmos = true;

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