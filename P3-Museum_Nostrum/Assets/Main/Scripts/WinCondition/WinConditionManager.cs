using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class WinConditionManager : MonoBehaviour {

    private static WinConditionManager instance = null;
    public static WinConditionManager Instance
    {
        get
        {
            return instance;
        }
    }

    /// <summary>
    /// stores a reference to all pictures in the current scene.
    /// </summary>
    private readonly List<InteractivePicture> allPictures = new List<InteractivePicture>();
    /// <summary>
    /// represents how many pictures were in the scene at the beginning of the game.
    /// </summary>
    private int initialPictureCount;
    /// <summary>
    /// keeps track of how many pictures are collected.
    /// </summary>
    private int pictureCount = 0;

    /// <summary>
    /// stores access to all UISlots.
    /// </summary>
    private readonly List<InteractiveUISlot> allUISlots = new List<InteractiveUISlot>();

    private WinconditionUI winUI;

    void Awake()
    {
        //ensure singleton.
        if (!instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private WinConditionManager() { }

    // Use this for initialization
    void Start () {
        //Get pictures.
        allPictures.AddRange(UnityEngine.Object.FindObjectsOfType<InteractivePicture>());
        initialPictureCount = allPictures.Count;

        //Get UISlots
        allUISlots.AddRange(UnityEngine.Object.FindObjectsOfType<InteractiveUISlot>());

        //Setup UI values.
        winUI = UnityEngine.Object.FindObjectOfType<WinconditionUI>();
        winUI.SetTotalCountTo(initialPictureCount);
        winUI.SetCurrentCountTo(pictureCount);


        //in case no pictures are placed in level (e.g. during level creation process)
        if (initialPictureCount < 1)
        {
            winUI.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
        if (IsWon())
        {
            //update UI.
            winUI.HideWinGuidanceMsg();
            winUI.ShowWonMsg();

            //execute cleanup
            CleanUp();
        }
    }

    /// <summary>
    /// runs cleanup jobs.
    /// </summary>
    private void CleanUp()
    {
        StartCoroutine(CleanUpAfter(1.5f));
    }

    private IEnumerator CleanUpAfter(float seconds)
    {
        //Write collected data to JSON
        if (DataLogger.Instance)
        {
            DataLogger.Instance.Log("endGame", "Win condition is met.", "Thank you for playing ;-)");
            DataLogger.Instance.PrintDataList();
        }

        yield return new WaitForSeconds(seconds);

        //load start scene.
        GetComponent<SceneLoader>().LoadScene();
    }

    /// <summary>
    /// updates the WinConditionManager.
    /// </summary>
    /// <param name="picture"></param>
    /// <returns></returns>
    public bool RegisterPickupOf(InteractivePicture picture)
    {
        ++pictureCount;
        winUI.SetCurrentCountTo(pictureCount);
        if (AreAllPicturesFound())
        {
            //inform player how to finish the game.
            Debug.Log("All pictures found: " + pictureCount + " / " + initialPictureCount);
            winUI.ShowWinGuidanceMsg();
        }
        return allPictures.Remove(picture);
    }


    private bool IsWon()
    {
        return AreAllPicturesFound() && AreAllUISlotsEmpty();
    }

    private bool AreAllPicturesFound()
    {
        return pictureCount == initialPictureCount;
    }

    private bool AreAllUISlotsEmpty()
    {
        int i = 0;
        while (i < allUISlots.Count && !allUISlots[i].HasTexture())
        {
            ++i;
        }
        Debug.Log("i: " + i + " slots count: " + allUISlots.Count + " all empty? " + (i == allUISlots.Count));
        return i == allUISlots.Count;
    }
}
