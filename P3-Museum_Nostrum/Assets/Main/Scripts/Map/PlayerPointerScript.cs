using UnityEngine;
using UnityEngine.UI;

public class PlayerPointerScript : MonoBehaviour {

    /// <summary>
    /// stores the visual representation of the player pointer.
    /// </summary>
    private Image[] imgOfChildren;

    /// <summary>
    /// stores the source of the rotation.
    /// </summary>
    private Transform playerTransform;


    private RectTransform playerPointerRect;

    [Tooltip("Adjust the rotation to clockwise or counter-clockwise. " +
        "Note: must be set up before launching the game.")]
    public bool invertRotationDirection = true;
    private float invertValue;  //either 1f or -1f.

    void Awake()
    {
        InitializeTag();
    }

    public void InitializeTag()
    {
        if (gameObject.tag != "PlayerPointer")
        {
            gameObject.tag = "PlayerPointer";
        }
    }

    // Use this for initialization
    void Start () {
        imgOfChildren = GetComponentsInChildren<Image>();
        playerPointerRect = GetComponent<RectTransform>();

        //ensures that the pointer starts invisible.
        ChangeVisibilityTo(0f);


        //Get the source of the rotation, namely the player GameObject.
        playerTransform = Camera.main.GetComponent<Transform>();

        if (invertRotationDirection)
        {
            invertValue = -1f;
        }
        else
        {
            invertValue = 1f;
        }
    }

    void Update()
    {
        RotatePlayerPointer();
    }


    public void ChangeVisibilityTo(float alpha)
    {
        for (int i = 0; i < imgOfChildren.Length; ++i)
        {
            Color c = imgOfChildren[i].color;
            c.a = alpha;
            imgOfChildren[i].color = c;
        }
    }


    private void RotatePlayerPointer()
    {
        Vector3 compassRotation = playerPointerRect.rotation.eulerAngles;
        compassRotation.z = CalculateAngle() * invertValue;
        playerPointerRect.rotation = Quaternion.Euler(compassRotation);
    }

    private float CalculateAngle()
    {
        return playerTransform.rotation.eulerAngles.y;
    }
}
