using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    //Plays step Sound
    public void playStepSFX()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }
}
