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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!fsps.IsShowing())
        {
            fsps.Show(t);
            if (videoclip)
            {
                vpb.ShowVideoPlayButton(videoclip);
            }
        }

    }

    // Use this for initialization
    void Start () {
        fsps = UnityEngine.Object.FindObjectOfType<FullScreenPresentationSurface>();
        t = GetComponent<Renderer>().material.mainTexture;
        vpb = UnityEngine.Object.FindObjectOfType<VideoPlayButton>();
    }
	
	
}
