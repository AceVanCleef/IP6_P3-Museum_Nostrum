using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartEntranceAnimation : MonoBehaviour {

    private Animator animation;

    void Start()
    {
        animation = gameObject.GetComponent<Animator>();
    }
    public void startAnimation()
    {
        //Starts Animation
        animation.Play("EntranceAnimation");
      }

    public void afterAnimation()
    {
        //opens next scene. is triggered as event in animation "StartEntranceAnimation"
        UnityEngine.Object.FindObjectOfType<SceneLoader>().LoadScene();
    }

}
