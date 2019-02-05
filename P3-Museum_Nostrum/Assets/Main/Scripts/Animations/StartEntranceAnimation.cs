using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartEntranceAnimation : MonoBehaviour {

    private Animator anim1;

    void Start()
    {
        anim1 = gameObject.GetComponent<Animator>();
    }
    public void startAnimation()
    {
        //Starts Animation
        anim1.Play("EntranceAnimation");
           
      }

    public void afterAnimation()
    {
        //opens next scene. is triggered as event in animation "StartEntranceAnimation"
         SceneManager.LoadScene("Assets/Main/Scenes/Development/Test Signaletik.unity");
    }

}
