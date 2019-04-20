using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class FullScreenPresentationSurface : MonoBehaviour, IPointerClickHandler
{
    private static FullScreenPresentationSurface instance = null;

    private RawImage rw;

    private VideoPlayButton vpb;
    private VideoPlayerManager vpm;

    private CameraZoomManager czm;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsShowing())
        {
            Hide();
            vpb.HideVideoPlayButton();
            vpm.CloseVideoPlayer();
            czm.ResetCamera();
        }
    }


    private void Init()
    {
        rw.color = new Color(rw.color.r, rw.color.g, rw.color.b, 0f);
        rw.texture = null;
        rw.raycastTarget = false;
    }

    public void Show(Texture t)
    {
        rw.color = new Color(rw.color.r, rw.color.g, rw.color.b, 1f);
        rw.texture = t;
        rw.raycastTarget = true;
    }

    public void Hide()
    {
        rw.color = new Color(rw.color.r, rw.color.g, rw.color.b, 0f);
        rw.texture = null;
        rw.raycastTarget = false;
    }

    public bool IsShowing()
    {
        return rw.texture;
    }

    void Awake()
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

    void Start()
    {
        rw = GetComponent<RawImage>();
        vpb = UnityEngine.Object.FindObjectOfType<VideoPlayButton>();
        vpm = UnityEngine.Object.FindObjectOfType<VideoPlayerManager>();
        czm = UnityEngine.Object.FindObjectOfType<CameraZoomManager>();
        Init();
    }
}
