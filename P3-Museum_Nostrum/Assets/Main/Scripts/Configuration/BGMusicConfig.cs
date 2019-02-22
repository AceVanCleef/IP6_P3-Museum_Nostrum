using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicConfig : MonoBehaviour, ITagEnsurance {

    void Start()
    {
        InitializeTag();
    }

    public void InitializeTag()
    {
        if (gameObject.tag != "BackgroundMusic")
        {
            gameObject.tag = "BackgroundMusic";
        }
    }
}
