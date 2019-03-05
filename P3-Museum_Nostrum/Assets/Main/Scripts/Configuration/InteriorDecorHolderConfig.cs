using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorDecorHolderConfig : MonoBehaviour, ITagEnsurance {

    void Awake()
    {
        InitializeTag();
    }

    public void InitializeTag()
    {
        if (gameObject.tag != "InteriorHolder")
        {
            gameObject.tag = "InteriorHolder";
        }
    }
}
