using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundSliderClicked : MonoBehaviour, IPointerDownHandler
{
    //plays sound if sound-slider is clicked
    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<AudioSource>().Play();
    }
}
