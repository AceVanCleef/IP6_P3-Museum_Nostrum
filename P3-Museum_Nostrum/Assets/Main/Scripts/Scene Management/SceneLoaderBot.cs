using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SceneLoader))]
/// <summary>
/// automatically loads the next scene defined by SceneLoader after t seconds.
/// </summary>
public class SceneLoaderBot : MonoBehaviour {

    /// <summary>
    /// delayes the automatical loading of the next scene by t seconds.
    /// </summary>
    [Tooltip("Delayes the automatical loading of the next scene by t seconds.")]
    public float TimeUntilSceneChanges = 0f;

    void Start()
    {
        //Avoid values below 0 seconds.
        if (TimeUntilSceneChanges < 0) TimeUntilSceneChanges = 0f;
        //Load next scene.
        StartCoroutine(LoadSceneAutomatically());
    }

    private IEnumerator LoadSceneAutomatically()
    {
        yield return new WaitForSeconds(TimeUntilSceneChanges);
        GetComponent<SceneLoader>().LoadScene();
    }
    
}
