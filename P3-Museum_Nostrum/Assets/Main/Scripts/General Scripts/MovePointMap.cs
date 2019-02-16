using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePointMap : MonoBehaviour
{
    Dictionary<string, float[]> dictionary = new Dictionary<string, float[]>();
    private RectTransform rect;

    public Image mapPoint;

    void Start()
    {
        //coordindates uf each room on the ground view
        //the name of each room is used as key
        dictionary.Add("Entrance Hall", new float[] { 299.3f, -554.3f });
        dictionary.Add("Corridor EntranceHall_Museumsshop", new float[] { 453f, -523f });
        dictionary.Add("Museumshop", new float[] { 565f, -522f });
        dictionary.Add("Corridor Museumsshop_RightGallery", new float[] { 565f, -436f });
        dictionary.Add("Right Gallery Room", new float[] { 555f, -327f });
        dictionary.Add("Cross Wing R1", new float[] { 463f, -374f });
        dictionary.Add("Cross Wing R2", new float[] { 419f, -374f });
        dictionary.Add("Cross Wing R5", new float[] { 419f, -418f });
        dictionary.Add("Cross Wing R3", new float[] { 375f, -374f });
        dictionary.Add("Cross Wing R4", new float[] { 419f, -330f });
        dictionary.Add("Triangle Wing R1", new float[] { 642f, -376f });
        dictionary.Add("Triangle Wing R3", new float[] { 688f, -349f });
        dictionary.Add("Triangle Wing R2", new float[] { 688f, -395f });
        dictionary.Add("Square Wing R1", new float[] { 644f, -286f });
        dictionary.Add("Square Wing R2", new float[] { 688f, -286f });
        dictionary.Add("Square Wing R4", new float[] { 644f, -242f });
        dictionary.Add("Square Wing R3", new float[] { 688f, -242f });
        dictionary.Add("Corridor RightGallery_SpecialExhibition", new float[] { 552f, -230f });
        dictionary.Add("Special_Exhibition", new float[] { 552f, -157f });
        dictionary.Add("Corridor SpecialExhibition_Cafeteria", new float[] { 443f, -157f });
        dictionary.Add("Cafeteria", new float[] { 299f, -157f });
        dictionary.Add("Room - Intermediate Cafeteria 1", new float[] { 297f, -36f });
        dictionary.Add("Room - Intermediate Cafeteria 2", new float[] { 297f, 37f });
        dictionary.Add("WC near Cafeteria", new float[] { 363f, 37f });
        dictionary.Add("Broom Cupboard", new float[] { 215f, 47f });
        dictionary.Add("Corridor Garden_Cafeteria", new float[] { 142f, -157f });
        dictionary.Add("Garden", new float[] { 38f, -157f });
        dictionary.Add("Corridor LeftGalleryRoom_Garden", new float[] { 38f, -232f });
        dictionary.Add("Left Gallery Room", new float[] { 38f, -331f });
        dictionary.Add("Alongside Wing R1", new float[] { -52f, -370f });
        dictionary.Add("Alongside Wing R2", new float[] { -52f, -329f });
        dictionary.Add("Alongside Wing R3", new float[] { -52f, -285f });
        dictionary.Add("Room 1", new float[] { 127f, -375f });
        dictionary.Add("Room 2", new float[] { 171f, -375f });
        dictionary.Add("Room 3", new float[] { 171f, -329f });
        dictionary.Add("Room 4", new float[] { 219f, -329f });
        dictionary.Add("Room 5", new float[] { 219f, -375f });
        dictionary.Add("Corridor RoomX_LeftGallery", new float[] { 32f, -433f });
        dictionary.Add("Room_X", new float[] { 30f, -515f });
        dictionary.Add("WC - Left", new float[] { -50f, -508f });
        dictionary.Add("Corridor EntranceHall_RoomX", new float[] { 144f, -523f });
        dictionary.Add("Corridor EntranceHall_Cafeteria", new float[] { 299f, -361f });
        dictionary.Add("Cellar Room A", new float[] { 1048f, -279f });
        dictionary.Add("Cellar Room B", new float[] { 963f, -240f });
        dictionary.Add("Cellar Room DE1", new float[] { 963f, -325f });
        dictionary.Add("Cellar Room DE2", new float[] { 874f, -205f });
        dictionary.Add("Cellar Room C", new float[] { 874f, -290f });
        dictionary.Add("Cellar Room D", new float[] { 874f, -375f });
        dictionary.Add("Cellar Room DE4", new float[] { 874f, -465f });
        dictionary.Add("Cellar Room DE3", new float[] { 785f, -290f });
        dictionary.Add("Cellar Room E", new float[] { 785f, -375f });


        //initialises the RectTransfrom of the point, which indicates the position on the map
        rect = mapPoint.GetComponent<RectTransform>();
    }

    //inverts the alpha channel of the map point. map point can not be disabled like the map itself, because it needs to moved, eventhough its not visible.
    public void invertVisibilityOfMapPoint()
    {
        var tempColor = mapPoint.color;
        tempColor.a = ((mapPoint.color.a == 100) ? 0f : 100f);
        mapPoint.color = tempColor;
    }


    public void movePointMap(string parentName)
    {
        if (dictionary.ContainsKey(parentName))
        {
            //gets the coordinates from the dictionary
            Vector3 pos;
            pos.x = dictionary[parentName][0];
            pos.y = dictionary[parentName][1];
            pos.z = 0f;

            //sets the new position
            rect.anchoredPosition = pos;
        }
    }
}
