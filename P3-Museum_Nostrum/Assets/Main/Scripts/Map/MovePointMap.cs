﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// manages the mini map.
/// </summary>
/// <remarks>Note that in the current version the room coordinates have to be defined by hand for each level.</remarks>
public class MovePointMap : MonoBehaviour
{
    Dictionary<string, float[]> dictionary = new Dictionary<string, float[]>();
    private RectTransform playerPointerRect;
    private PlayerPointerScript pps;

    public GameObject PlayerPointer;

    private bool isMapOpen = false;
    
    //coordindates of each room on the ground view
    //the name of each room is used as key
    void OnEnable()
    {
        //sets coordinates depending on which level has been loaded
        if (SceneManager.GetActiveScene().name == "Museum Nostrum LVL 01")
        {
            dictionary.Add("Entrance Hall", new float[] { 200f, 223f });
            dictionary.Add("Corridor EntranceHall_Museumsshop", new float[] { 275f, 237f });
            dictionary.Add("Museumshop", new float[] { 332f, 237f });
            dictionary.Add("Corridor Museumsshop_RightGallery", new float[] { 331f, 281f });
            dictionary.Add("Right Gallery Room", new float[] { 325f, 331f });
            dictionary.Add("Cross Wing R1", new float[] { 282f, 312f });
            dictionary.Add("Cross Wing R2", new float[] { 260f, 312f });
            dictionary.Add("Cross Wing R5", new float[] { 260f, 290f });
            dictionary.Add("Cross Wing R3", new float[] { 237f, 312f });
            dictionary.Add("Cross Wing R4", new float[] { 260f, 335f });
            dictionary.Add("Triangle Wing R1", new float[] { 369f, 312f });
            dictionary.Add("Triangle Wing R3", new float[] { 392f, 324f });
            dictionary.Add("Triangle Wing R2", new float[] { 392f, 302f });
            dictionary.Add("Square Wing R1", new float[] { 369f, 355f });
            dictionary.Add("Square Wing R2", new float[] { 392f, 355f });
            dictionary.Add("Square Wing R4", new float[] { 369f, 377f });
            dictionary.Add("Square Wing R3", new float[] { 393f, 377f });
            dictionary.Add("Corridor RightGallery_SpecialExhibition", new float[] { 326f, 382f });
            dictionary.Add("Special_Exhibition", new float[] { 326f, 419f });
            dictionary.Add("Corridor SpecialExhibition_Cafeteria", new float[] { 271f, 421f });
            dictionary.Add("Cafeteria", new float[] { 200f, 422f });
            dictionary.Add("Room - Intermediate Cafeteria 1", new float[] { 199f, 480f });
            dictionary.Add("Room - Intermediate Cafeteria 2", new float[] { 199f, 514f });
            dictionary.Add("WC near Cafeteria", new float[] { 235f, 515f });
            dictionary.Add("Broom Cupboard", new float[] { 158f, 520f });
            dictionary.Add("Corridor Garden_Cafeteria", new float[] { 124f, 421f });
            dictionary.Add("Garden", new float[] { 72f, 421f });
            dictionary.Add("Corridor LeftGalleryRoom_Garden	", new float[] { 72f, 384f });
            dictionary.Add("Left Gallery Room", new float[] { 72f, 334f });
            dictionary.Add("Alongside Wing R1", new float[] { 27f, 313f });
            dictionary.Add("Alongside Wing R2", new float[] { 27f, 335f });
            dictionary.Add("Alongside Wing R3", new float[] { 27f, 358f });
            dictionary.Add("Room 1", new float[] { 127f, 312f });
            dictionary.Add("Room 2", new float[] { 139f, 335f });
            dictionary.Add("Room 3", new float[] { 115f, 335f });
            dictionary.Add("Room 4", new float[] { 115f, 357f });
            dictionary.Add("Room 5", new float[] { 139f, 357f });
            dictionary.Add("Corridor RoomX_LeftGallery", new float[] { 69f, 283f });
            dictionary.Add("Room_X", new float[] { 69f, 241f });
            dictionary.Add("WC - Left", new float[] { 28f, 246f });
            dictionary.Add("Corridor EntranceHall_RoomX", new float[] { 124f, 238f });
            dictionary.Add("Corridor EntranceHall_Cafeteria", new float[] { 200f, 323f });
            dictionary.Add("Cellar Room A", new float[] { 570f, 359f });
            dictionary.Add("Cellar Room B", new float[] { 525f, 378f });
            dictionary.Add("Cellar Room DE1", new float[] { 525f, 336f });
            dictionary.Add("Cellar Room DE2", new float[] { 482f, 395f });
            dictionary.Add("Cellar Room C", new float[] { 482f, 354f });
            dictionary.Add("Cellar Room D", new float[] { 482f, 311f });
            dictionary.Add("Cellar Room DE4", new float[] { 482f, 267f });
            dictionary.Add("Cellar Room DE3", new float[] { 442f, 354f });
            dictionary.Add("Cellar Room E", new float[] { 442f, 311f });
        }

        if (SceneManager.GetActiveScene().name == "Museum Nostrum LVL 01 V2")
        {
            dictionary.Add("Entrance Hall", new float[] { 240f, 320f });
            dictionary.Add("Corridor EntranceHall_Museumsshop", new float[] { 321f, 287f });
            dictionary.Add("Museumshop", new float[] { 376f, 283f });
            dictionary.Add("Right Gallery Room", new float[] { 377f, 375f });
            dictionary.Add("Special_Exhibition", new float[] { 377f, 468f });
            dictionary.Add("Corridor SpecialExhibition_Cafeteria", new float[] { 321f, 457f });
            dictionary.Add("Corridor Museumsshop_RightGallery", new float[] { 377f, 324f });
            dictionary.Add("Corridor RightGallery_SpecialExhibition", new float[] { 377f, 426f });
            dictionary.Add("Cafeteria", new float[] { 240f, 423f });
            dictionary.Add("WC_Cafeteria", new float[] { 244f, 501f });
            dictionary.Add("Broom Cupboard", new float[] { 91f, 290f });
            dictionary.Add("Garden", new float[] { 154f, 473f });
            dictionary.Add("Left Gallery Room", new float[] { 142f, 396f });
            dictionary.Add("Room_X", new float[] { 154f, 315f });
            dictionary.Add("WC - Left", new float[] { 91f, 330f });
            dictionary.Add("Cellar Room 1", new float[] { 378f, 240f });
            dictionary.Add("Cellar Room 2", new float[] { 359f, 201f });
            dictionary.Add("Cellar Room 3", new float[] { 397f, 201f });
        }

        if (SceneManager.GetActiveScene().name == "Museum Nostrum LVL 01" || SceneManager.GetActiveScene().name == "Museum Nostrum LVL 01 V2")
        {
            //initialises the RectTransfrom of the point, which indicates the position on the map
            playerPointerRect = PlayerPointer.GetComponent<RectTransform>();
            //cache a reference to player pointer script, responsible for enabling7disabling 
            //the player representation on the map.
            pps = PlayerPointer.GetComponent<PlayerPointerScript>();
        }
    }


    //inverts the alpha channel of the map point.map point can not be disabled like the map itself, because it needs to moved, eventhough its not visible.
    /// <summary>
    /// toggles the visibility of the mini map.
    /// </summary>
    public void toggleMap()
    {
        if (!isMapOpen)
        {
            if (DataLogger.Instance)
            {
                //draw a touch on GUI.
                DataLogger.Instance.Log("touch", "On MapToggleIcon", Input.mousePosition.ToString());
                DataLogger.Instance.Log("openMap", Input.mousePosition.ToString());
            }
            pps.ChangeVisibilityTo(100f);
        }
        else
        {
            if (DataLogger.Instance)
            {
                //draw a touch on GUI.
                DataLogger.Instance.Log("touch", "On OpenedMap", Input.mousePosition.ToString());
                DataLogger.Instance.Log("closeMap", Input.mousePosition.ToString());

            }
            pps.ChangeVisibilityTo(0f);
        }
        isMapOpen = !isMapOpen;
    }

    /// <summary>
    /// moves the player pointer towards the room defined by roomName.
    /// </summary>
    /// <param name="roomName">name of the room the player is currently moving towards.</param>
    public void moveMapPointer(string roomName)
    {
        if (dictionary.ContainsKey(roomName))
        {
            //gets the coordinates from the dictionary
            Vector3 pos;
            pos.x = dictionary[roomName][0];
            pos.y = dictionary[roomName][1];
            pos.z = 0f;

            //sets the new position
            playerPointerRect.anchoredPosition = pos;
        }
    }
}
