using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneA : MonoBehaviour {


    private string nameOfTargetScene = "A";

    private void Start()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        StartCoroutine(LoadSceneAsync());
        Debug.Log("loaded scene"); //doesn't show up!
    }

    IEnumerator LoadSceneAsync()
    {
        // "The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens."
        // Source: https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadSceneAsync.html

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nameOfTargetScene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
