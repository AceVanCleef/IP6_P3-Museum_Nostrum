using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundSliderClicked : MonoBehaviour, IPointerDownHandler
{
    private GameObject caw;
    // Use this for initialization
    void Start()
    {
        caw = GameObject.Find("CameraAnimationWrapper");
    }
    
    //plays sound if sound-slider is clicked
    public void OnPointerDown(PointerEventData eventData)
    {
        caw.GetComponent<AudioSource>().Play();
    }
}
