using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFadeAnimation : MonoBehaviour {

    private Animator animationFade;

    void Start()
    {
        animationFade = gameObject.GetComponent<Animator>();
    }
    
    public void startAnimation()
    {
        //Starts Animation
        animationFade.Play("FadeAnimation");
    }
}
