using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPostHolderConfig : MonoBehaviour, ITagEnsurance {

    void Awake()
    {
        InitializeTag();
    }

    public void InitializeTag()
    {
        if (gameObject.tag != "WayfindingHolder")
        {
            gameObject.tag = "WayfindingHolder";
        }
    }
}
