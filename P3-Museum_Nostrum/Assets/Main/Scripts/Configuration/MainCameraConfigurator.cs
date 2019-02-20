using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCameraConfigurator : MonoBehaviour, ITagEnsurance
{
    GameObject masterSlider;
    Slider slider;

    void Start()
    {
        InitializeTag();
    }

    public void InitializeTag()
    {
        if (gameObject.tag != "MainCamera")
        {
            gameObject.tag = "MainCamera";
        }
    }
    public void setMasterVolume()
    {
        if (!masterSlider)
            masterSlider = GameObject.Find("MasterSlider");

        if (masterSlider)
        {
            slider = masterSlider.GetComponent<Slider>();
            AudioListener.volume = slider.value;
        }
    }

}
