using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptionsManager : MonoBehaviour
{
   
    private Slider slider;
    public int a;
    public int b;
    public int c;


    // Use this for initialization
    void Awake()
    {
        GameObject mapWrapper = GameObject.Find("MasterSlider");
        if(mapWrapper)
            slider = mapWrapper.GetComponent<Slider>();

        a = 1;
        b = 2;
        c = 3;
    }
    
    void Update()
    {
        if (slider)
        {
            Debug.Log("aud" + slider.value);
            AudioListener.volume = slider.value;
        }
        
    }
}
