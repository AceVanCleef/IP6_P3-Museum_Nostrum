using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadSceneOnTapTest : AbstractInteractiveGameObject {

    public Color color;

    public string nameOfTargetScene;

	// Use this for initialization
	protected virtual new void Start () {
        base.Start();
        GetComponent<Renderer>().material.color = color;
	}

    public override void OnPointerClick(PointerEventData eventData)
    {
        //loading next scene.
        Debug.Log("loading next scene.");
        LoadScene();
    }


    public void LoadSceneFromUIButton()
    {
        Debug.Log("Loading scene from UI Button");
        LoadScene();
    }

    private void LoadScene()
    {
        DataCarrier.Instance.IntValue -= 1;
        Debug.Log("Loadscene -> DataCarrier.Instance.IntValue = " + DataCarrier.Instance.IntValue);
        StartCoroutine( LoadSceneAsync() );
        Debug.Log("loaded scene"); //doesn't show up! GO already destroyed
    }

    IEnumerator LoadSceneAsync()
    {
        // "The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens."
        // Source: https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadSceneAsync.html

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nameOfTargetScene );

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //testing whether I can do some action after scene has been loaded:
        Debug.Log("Done loading " + nameOfTargetScene + " scene!"); //Note: doesn't reach it. GO already destroyed.
    }
}
