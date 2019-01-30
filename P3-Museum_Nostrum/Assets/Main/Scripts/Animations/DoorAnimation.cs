using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    /*private Animator anim1;

    void Start()
    {
        anim1 = gameObject.GetComponent<Animator>();
    }
    public void startAnimation()
    {
        //Starts Animation
        anim1.Play("DoorAnimation");

    }*/

    //Plays step Sound
    public void playStepSFX()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }



}
