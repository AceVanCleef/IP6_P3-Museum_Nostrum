using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// offers a callback to load the next scene defined in the inspector.
/// </summary>
public class SceneLoader : MonoBehaviour {

    [Tooltip("Defines which scene will be loaded by SceneLoader.")]
    /// <summary>
    /// defines which scene will be loaded by SceneLoader.
    /// </summary>
    public string nameOfTargetScene = "StartMenu";

    /// <summary>
    /// loads next scene asynchronously based on the name of the scene set in the inspector.
    /// </summary>
    public void LoadScene()
    {
        StartCoroutine(LoadSceneAsync());
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
