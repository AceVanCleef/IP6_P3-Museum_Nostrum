using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraConfigurator : MonoBehaviour, ITagEnsurance {

	void Start () {
        InitializeTag();
	}

    public void InitializeTag()
    {
        if (gameObject.tag != "MainCamera")
        {
            gameObject.tag = "MainCamera";
        }
    }

}
