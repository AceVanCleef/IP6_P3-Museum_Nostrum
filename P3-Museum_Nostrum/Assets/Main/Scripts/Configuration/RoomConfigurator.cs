using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// allows adjustment of a room's length, width and height dimensions.
/// </summary>
[ExecuteInEditMode]
public class RoomConfigurator : MonoBehaviour, ITagEnsurance {
   

    private Transform northWall;
    private Transform eastWall;
    private Transform southWall;
    private Transform westWall;
    private Transform floor;
    private Transform ceiling;
    private bool isInitialized = false;

    [Range(3f, 8f)][Tooltip("Define how tall this room is.")]
    public float RoomHeight = 5f;

    [Range(5f, 25f)]
    [Tooltip("Define how wide this room is measured in meters. Direction: West / East.")]
    public float RoomWidth = 10f;

    [Range(5f, 25f)]
    [Tooltip("Define how long this room is measured in meters. Direction: North / South.")]
    public float RoomLength = 10f;

    [SerializeField][Tooltip("Adjust to close gap and prevent light from entering this room. Expands roof by n meters.")]
    private float hideGapInRoof = 0.8f;

    //prevents clipping of walls with floor.
    private readonly float wallPositionOffset = 0.25f;

    /// <summary>
    /// allocates the transform of walls, ceiling and floor to variables.
    /// </summary>
    private void InitializeFields()
    {
        if (isInitialized) return;
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; ++i)
        {
            switch (children[i].name)
            {
                case "N_Wall": northWall = children[i]; break;
                case "S_Wall": southWall = children[i]; break;
                case "E_Wall": eastWall = children[i]; break;
                case "W_Wall": westWall = children[i]; break;
                case "Floor": floor = children[i]; break;
                case "Ceiling": ceiling = children[i]; break;
                default: break;
            }
        }
        isInitialized = true;
    }


    void Update () {
        if (Application.isPlaying)
        {
            // code executed in play mode

        }
        else
        {
            // code executed in edit mode
            InitializeFields();
            ScaleHeight();
            ScaleWidth_WestEast();
            ScaleLength_NorthSouth();
            UpdatePositions();  //has to come after all Scale___() - methods.
        }
    }

    /// <summary>
    /// syncronizes the positions of walls and ceiling with their parent GameObject and 
    /// allows to drag the room GameObject around in the scene view.
    /// 
    /// Warning: This has to be handled in one single method. Otherwise, when moving the parent  
    /// within the scene view, the position of walls or ceiling might not be updated.
    /// </summary>
    private void UpdatePositions()
    {
        //updating walls
        Vector3 pos = eastWall.position;
        pos.x = RoomLength / 2 + wallPositionOffset + transform.position.x;
        pos.y = (RoomHeight / 2) + transform.position.y;
        pos.z = transform.position.z;
        eastWall.position = pos;
        pos = westWall.position;
        pos.x = (RoomLength / 2 + wallPositionOffset) * (-1) + transform.position.x;
        pos.y = (RoomHeight / 2) + transform.position.y;
        pos.z = transform.position.z;
        westWall.position = pos;
        pos = northWall.position;
        pos.x = transform.position.x;
        pos.y = (RoomHeight / 2) + transform.position.y;
        pos.z = RoomWidth / 2 + wallPositionOffset + transform.position.z;
        northWall.position = pos;
        pos = southWall.position;
        pos.x = transform.position.x;
        pos.y = (RoomHeight / 2) + transform.position.y;
        pos.z = (RoomWidth / 2 + wallPositionOffset) * (-1) + transform.position.z;
        southWall.position = pos;

        //update ceiling
        Vector3 ceilingPos = ceiling.position;
        ceilingPos.y = RoomHeight + (ceiling.localScale.y / 2) + transform.position.y; //prevents clipping with walls.
        ceiling.position = ceilingPos;
    }

    /// <summary>
    /// scales the length of north and south wall as well as the length of the floor and ceiling.
    /// </summary>
    private void ScaleLength_NorthSouth()
    {
        //adjust walls
        Transform[] walls = { northWall, southWall };
        float closeGapDelta = 0.5f;     //closes the corners of the walls.
        for (int i = 0; i < walls.Length; ++i)
        {
            Vector3 scale = walls[i].localScale;
            scale.z = RoomLength + closeGapDelta;
            walls[i].localScale = scale;
        }
        //adjust floor...
        Vector3 floorScale = floor.localScale;
        floorScale.x = RoomLength / 10f; //For a plane GO, 'localScale.z = 1' equals 10 units (meters).
        floor.localScale = floorScale;
        //...and ceiling.
        Vector3 ceilingScale = ceiling.localScale;
        ceilingScale.x = RoomLength + hideGapInRoof;
        ceiling.localScale = ceilingScale;

        //reposition other walls: see UpdatePositions().

    }

    /// <summary>
    /// scales the length of west and east wall as well as the length of the floor and ceiling.
    /// </summary>
    private void ScaleWidth_WestEast()
    {
        //adjust walls
        Transform[] walls = { eastWall, westWall };
        for (int i = 0; i < walls.Length; ++i)
        {
            Vector3 scale = walls[i].localScale;
            scale.z = RoomWidth;
            walls[i].localScale = scale;
        }
        //adjust floor...
        Vector3 floorScale = floor.localScale;
        floorScale.z = RoomWidth / 10f; //For a plane GO, 'localScale.z = 1' equals 10 units (meters).
        floor.localScale = floorScale;
        //...and ceiling.
        Vector3 ceilingScale = ceiling.localScale;
        ceilingScale.z = RoomWidth + hideGapInRoof;
        ceiling.localScale = ceilingScale;

        //reposition other walls: see UpdatePositions().
    }

    /// <summary>
    /// scales the height of all four walls.
    /// </summary>
    private void ScaleHeight()
    {
        Transform[] walls = { northWall, southWall, eastWall, westWall };
        for (int i = 0; i < walls.Length; ++i)
        {
            //rescaling the walls.
            Vector3 scale = walls[i].localScale;
            scale.y = RoomHeight;
            walls[i].localScale = scale;

            //Adjusting wall positions by moving them up/down so that they fit with the floor and roof: see UpdatePositions().

        }
        //moving the roof up/down: see UpdatePositions().
    }


    #region dynamicCameraRenderingRange
    //cynamically adjusting how far the player can see so that he only sees highlightning shaders in his near vicinity.

    /// <summary>
    /// cynamically adjusting how far the player can see according to this room's dimensions.
    /// </summary>
    public void AdjustRenderingRangeOfPlayerCamera()
    {
        //calculating the room diagonal using pythagoras.
        float roomDiagonal = Mathf.Sqrt(RoomWidth * RoomWidth + RoomLength * RoomLength);
        Debug.Log("length, width: " + RoomLength + " , " + RoomWidth + " => " + roomDiagonal + " => " + (roomDiagonal / 2f));

        //only half of it is needed.
        float newRenderDistance = roomDiagonal / 2f;
        //finally, set render distance of the player camera.
        Camera.main.GetComponent<Camera>().farClipPlane = newRenderDistance;
    }
    #endregion dynamicCameraRenderingRange


    void Awake()
    {
        InitializeTag();

        //adjust camera rendering range if player starts in this room.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player && Vector3.Distance(transform.position, 
            player.transform.position) < 0.01f)
        {
            AdjustRenderingRangeOfPlayerCamera();
        }
    }

    public void InitializeTag()
    {
        if (gameObject.tag != "Room")
        {
            gameObject.tag = "Room";
        }
    }
}
