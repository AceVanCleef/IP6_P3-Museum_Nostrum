using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void StartGame()
    {
        SceneManager.LoadScene("Assets/Main/Scenes/Animation/EntranceAnimation.unity");
    }
    public void CloseGame()
    {
        Application.Quit();
    }

}
