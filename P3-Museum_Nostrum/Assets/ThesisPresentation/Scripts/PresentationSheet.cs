using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class PresentationSheet : MonoBehaviour, IPointerClickHandler
{
    private FullScreenPresentationSurface fsps;
    private Texture t;

    private VideoPlayButton vpb;
    public VideoClip videoclip;

    private CameraZoomManager czm;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!fsps.IsShowing())
        {
            StartCoroutine(AsyncShow());
        }

    }

    private IEnumerator AsyncShow()
    {
        yield return czm.MoveCameraTo(transform.position);
        // How to Wait for the execution of a previous function:
        // https://answers.unity.com/questions/1034021/start-method-after-coroutine-has-finished.html
        fsps.Show(t);
        if (videoclip)
        {
            vpb.ShowVideoPlayButton(videoclip);
        }
    }

    // Use this for initialization
    void Start () {
        fsps = UnityEngine.Object.FindObjectOfType<FullScreenPresentationSurface>();
        t = GetComponent<Renderer>().material.mainTexture;
        vpb = UnityEngine.Object.FindObjectOfType<VideoPlayButton>();
        czm = UnityEngine.Object.FindObjectOfType<CameraZoomManager>();
    }
	
	
}
