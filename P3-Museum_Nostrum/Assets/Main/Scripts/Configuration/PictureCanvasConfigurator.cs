using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureCanvasConfigurator : MonoBehaviour, ITagEnsurance {

    void Start()
    {
        InitializeTag();
    }

    public void InitializeTag()
    {
        if (gameObject.tag != "PictureCanvas")
        {
            gameObject.tag = "PictureCanvas";
        }
    }
}
