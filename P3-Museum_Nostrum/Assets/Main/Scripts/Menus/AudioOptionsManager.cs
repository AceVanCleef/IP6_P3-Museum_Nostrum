using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptionsManager : MonoBehaviour
{
   
    public Slider slider; 
   

    // Use this for initialization
    void Awake()
    {
        //slider = GetComponent<Slider>();
    }
    
    void Update()
    {
        /*if (slider)
        {
            slider = GetComponent<Slider>();
        }*/
        AudioListener.volume = slider.value;
    }
}
