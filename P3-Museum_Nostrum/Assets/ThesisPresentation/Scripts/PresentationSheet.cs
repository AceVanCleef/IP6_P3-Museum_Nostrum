using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PresentationSheet : MonoBehaviour, IPointerClickHandler
{
    private FullScreenPresentationSurface fsps;
    private Texture t;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!fsps.IsShowing())
        {
            fsps.Show(t);
        }

    }

    // Use this for initialization
    void Start () {
        fsps = UnityEngine.Object.FindObjectOfType<FullScreenPresentationSurface>();
        t = GetComponent<Renderer>().material.mainTexture;
    }
	
	
}
