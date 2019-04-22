using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayButton : MonoBehaviour {

    private static VideoPlayButton instance = null;


    private VideoClip vc;

    public VideoClip Clip
    {
        get
        {
            return vc;
        }
        set
        {
            vc = value;
        }
    }

    private VideoPlayerManager vpm;
    private Button button;
    private Image img;
    private RectTransform rt;
    private Vector2 originalPos;

    // Use this for initialization
    void Start()
    {
        vpm = UnityEngine.Object.FindObjectOfType<VideoPlayerManager>();
        img = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(InformVideoPlayerManager);
        rt = GetComponent<RectTransform>();
        originalPos = rt.localPosition;

        HideVideoPlayButton();
    }

    private void InformVideoPlayerManager()
    {
        vpm.OpenVideoPlayerWith(vc);
    }

    public void ShowVideoPlayButton(VideoClip vc)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
        img.raycastTarget = true;
        Clip = vc;
        button.interactable = true;
        rt.localPosition = originalPos;
    }

    public void HideVideoPlayButton()
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
        img.raycastTarget = false;
        button.interactable = false;
        rt.localPosition = new Vector3(200f, 0f);
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
}
