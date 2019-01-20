using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEntranceAnimation : MonoBehaviour {

    public Animator anim1;

    void Start()
    {
        anim1 = gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            anim1.Play("EntranceAnimation");
        }
    }
}
