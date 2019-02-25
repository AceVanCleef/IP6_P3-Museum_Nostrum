using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCameraConfigurator : MonoBehaviour, ITagEnsurance
{
    GameObject masterSlider;
    Slider slider;

    private DataLogger dataLogger;

    void Start()
    {
        InitializeTag();

        //get DataLogger
        GameObject go = GameObject.Find("DataLogger");
        dataLogger = (DataLogger)go.GetComponent(typeof(DataLogger));
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
            dataLogger.Log("setMasterVolume", slider.value.ToString(), null, null, null, null);
            AudioListener.volume = slider.value;
        }
    }

}
