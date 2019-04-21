using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIZoomImage : MonoBehaviour, IScrollHandler
{
    //source: https://www.youtube.com/watch?v=BFX3FpUnoio&t=14s

    private Vector3 initialScale;

    [SerializeField]
    private float zoomSpeed = 0.1f;
    [SerializeField]
    private float maxZoom = 10f;

    private ScrollRect sr;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    private void Start()
    {
        sr = GetComponentInParent<ScrollRect>();
    }

    public void OnScroll(PointerEventData eventData)
    {
        var delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
        var desiredScale = transform.localScale + delta;

        desiredScale = ClampDesiredScale(desiredScale);

        transform.localScale = desiredScale;

        ScrollToMousePosOnZoomIn(eventData.scrollDelta.y);
    }

    private Vector3 ClampDesiredScale(Vector3 desiredScale)
    {
        desiredScale = Vector3.Max(initialScale, desiredScale);
        desiredScale = Vector3.Min(initialScale * maxZoom, desiredScale);
        return desiredScale;
    }

    public void Reset()
    {
        transform.localScale = initialScale;
    }

    private void ScrollToMousePosOnZoomIn(float scrollDeltaY)
    {
        if (scrollDeltaY > 0)
            sr.ScrollToMousePosition();
    }
}
