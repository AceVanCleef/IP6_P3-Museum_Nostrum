using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FullScreenPresentationSurface : MonoBehaviour, IPointerClickHandler
{
    private static FullScreenPresentationSurface instance = null;

    private RawImage rw;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("hitting rw");
        if (IsShowing())
        {
            Hide();
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

    // Use this for initialization
    void Start()
    {
        rw = GetComponent<RawImage>();
        Init();

    }
}
