using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour {

    private VideoPlayer vp;
    
    public GameObject Window;
    public GameObject ProjectionSurface;

	// Use this for initialization
	void Start () {
        vp = GetComponentInChildren<VideoPlayer>();

        CloseVideoPlayer();
	}
	
	public void CloseVideoPlayer()
    {
        Window.SetActive(false);
        ProjectionSurface.SetActive(false);
    }

    public void OpenVideoPlayer()
    {
        ProjectionSurface.SetActive(true);
        Window.SetActive(true);
    }

    public void OpenVideoPlayerWith(VideoClip vc)
    {
        vp.clip = vc;
        OpenVideoPlayer();
    }
}
