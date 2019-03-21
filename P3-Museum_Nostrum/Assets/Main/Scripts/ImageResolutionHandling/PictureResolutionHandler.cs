using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Todo: check whether this class is still necessary. In case the game has to support different photo dimensions, it will.
public class PictureResolutionHandler : MonoBehaviour {

    //todo: Implement interface (and abstract class) in case image frame and UISlots get resizing too.

    //common image ratios for photographs: https://www.shutterstock.com/blog/common-aspect-ratios-photo-image-sizes
    //- 16:9
    //- 4:3
    //- 3:2 Note: Might not work due to unity's import settings. Todo: How to keep the aspect ratio of source image?
    //- 4:3
    //- 1:1
    //

    
    [SerializeField][Tooltip("Scales this picture plane's width and height measured in unity units.")]
    private float baseScale = 0.01f;

    private float baseHeightForLandscape = 9f;
    private float baseheightForPortrait = 16f;

    [SerializeField][Tooltip("If checked, allows you to constantly update the aspect ratio of this gameobject.")]
    private bool debugging = false;

    // Use this for initialization
    void Start () {
        if (!debugging) Rescale();
	}
	
	// Update is called once per frame
	void Update () {
        if (debugging) Rescale();
	}

    private void Rescale()
    {
        Texture t = GetTexture();
        //How much taller is the width compared to height?
        float aspectRatio = t.width / (t.height * 1.0f);
        Vector3 scale = transform.localScale;
        scale.x = CalculateWidth(aspectRatio);
        scale.z = CalculateHeight(aspectRatio);
        transform.localScale = scale;
    }

    private float CalculateHeight(float aspectRatio)
    {
        return DetermineHeight(aspectRatio) * baseScale;
    }

    private float CalculateWidth(float aspectRatio)
    {
        return aspectRatio * DetermineHeight(aspectRatio) * baseScale;
    }

    private float DetermineHeight(float aspectRatio)
    {
        // widthxheight
        // 2048x1024 -> 2/1  = 2        => Querformat
        // 1024x2048 -> 1/2  = 0.5      => Hochformat
        // 1000x1500 -> 2/3  = 0.666    => Hochformat
        // 1500x1000 -> 3/2 = 1.5       => Querformat
        // 1024x1024 -> 1/1 = 1         => Quadratisch
        if (aspectRatio > 1f)
        {
            return baseHeightForLandscape;
        }
        else
        {
            return baseheightForPortrait;
        }
    }

    private Texture GetTexture()
    {
        return GetComponent<Renderer>().material.mainTexture;
    }

    public void SetTexture(Texture t)
    {
        GetComponent<Renderer>().material.mainTexture = t;
        Rescale();
    }
}
