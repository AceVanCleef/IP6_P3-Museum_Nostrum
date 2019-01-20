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
        anim1.Play("EntranceAnimation");
           // SceneManager.LoadScene("Assets/Main/Scenes/Animation/EntranceAnimation.unity");
      }
}
