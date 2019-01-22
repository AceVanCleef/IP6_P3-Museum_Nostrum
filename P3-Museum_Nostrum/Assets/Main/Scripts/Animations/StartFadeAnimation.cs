using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFadeAnimation : MonoBehaviour {

    private Animator anim1;

    void Start()
    {
        anim1 = gameObject.GetComponent<Animator>();
    }
    
    public void startAnimation()
    {
        //Starts Animation
        anim1.Play("FadeAnimation");
    }
}
