using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;



public class ReplayManager : MonoBehaviour
{
    private static ReplayManager instance = null;

    public static ReplayManager Instance
    {
        get
        {
            return instance;
        }
    }

    private JSONParser jsonParser;

    //change speed of execution
    [Range(0.0f, 5.0f)]
    public float breakTime = 0.75f;
    private float timeToWait;
    public float tempDeltaTime;

    [Range(0.0f, 5.0f)]
    public float messageDuration = 1f;



    //enable to realtime mode
    public bool useDeltaTime = false;


    private GameObject player;

    private InputManager inputManager;

    private PointerEventData ped;

    public GameObject welcomeMessageUI;
    public GameObject messageUI;
    public TextMeshProUGUI messageText;

    public Button openMapButton;
    public Button closeMapButton;



    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading sceneas
        DontDestroyOnLoad(gameObject);



    }

    IEnumerator Start()
    {
        JSONParser jsonParser = transform.parent.GetComponentInChildren<JSONParser>();

        //get Player for movement
        player = GameObject.Find("Player");

        //get EventSystem for eventData
        GameObject go = GameObject.Find("EventSystem");
        EventSystem eventSystem = go.GetComponent<EventSystem>();
        ped = new PointerEventData(eventSystem);

        inputManager = (InputManager)player.GetComponent(typeof(InputManager));

        welcomeMessageUI.SetActive(false);

        foreach (var item in jsonParser.listActions)
        {
            timeToWait = breakTime;
            if (useDeltaTime)
            {
                timeToWait = item.timeStamp - tempDeltaTime;
            }

            if (item.action == "touch")
            {
                string v2Data = item.value2;
                v2Data = v2Data.Substring(1, v2Data.Length - 2).Replace(',', ';');
                string[] sArray = v2Data.Split(';');

                Vector2 touchtPosition = new Vector2(float.Parse(sArray[0]), float.Parse(sArray[1]));

                Debug.Log("touch_Position: " + touchtPosition.ToString());

                if (DataVisualizerManager.Instance)
                    DataVisualizerManager.GUI.DrawTouch(touchtPosition);
            }

            if (item.action == "SwipeDetected")
            {

                Debug.Log("swipeDraw");

                var direction = SwipeDirection.Down;
                if (item.value3 == "Down") { direction = SwipeDirection.Down; }
                else if (item.value3 == "Up") { direction = SwipeDirection.Up; }
                else if (item.value3 == "Left") { direction = SwipeDirection.Left; }
                else { direction = SwipeDirection.Right; }

                string v2Data = item.value1;
                v2Data = v2Data.Substring(1, v2Data.Length - 2).Replace(',', ';');
                string[] sArray = v2Data.Split(';');

                Vector2 startPosition = new Vector2(float.Parse(sArray[0]), float.Parse(sArray[1]));

                v2Data = item.value2;
                v2Data = v2Data.Substring(1, v2Data.Length - 2).Replace(',', ';');
                sArray = v2Data.Split(';');

                Vector2 endPosition = new Vector2(float.Parse(sArray[0]), float.Parse(sArray[1]));

                Debug.Log("swipe_startPosition: " + startPosition.ToString() + " endPosition: " + endPosition.ToString() + " direction: " + direction.ToString());

                SwipeData swipeData = new SwipeData()
                {
                    Direction = direction,
                    StartPosition = startPosition,
                    EndPosition = endPosition
                };

                if (DataVisualizerManager.Instance)
                    DataVisualizerManager.GUI.DrawSwipe(swipeData);
            }

            if (item.action == "drag n drop")
            {
                string v2Data = item.value1;
                v2Data = v2Data.Substring(1, v2Data.Length - 2).Replace(',', ';');
                string[] sArray = v2Data.Split(';');

                Vector2 startPosition = new Vector2(float.Parse(sArray[0]), float.Parse(sArray[1]));

                v2Data = item.value2;
                v2Data = v2Data.Substring(1, v2Data.Length - 2).Replace(',', ';');
                sArray = v2Data.Split(';');

                Vector2 endPosition = new Vector2(float.Parse(sArray[0]), float.Parse(sArray[1]));

                Debug.Log("drag n drop_startPosition: " + startPosition.ToString() + " endPosition: " + endPosition.ToString());

                if (DataVisualizerManager.Instance)
                    DataVisualizerManager.GUI.DrawDrag(startPosition, endPosition);
            }


            if (item.action == "goToRoom")
            {
                //get targetPosition from value
                string v3Data = item.value2;
                v3Data = v3Data.Substring(1, v3Data.Length - 2).Replace(',', ';');
                string[] sArray = v3Data.Split(';');
                Vector3 targetPosition = new Vector3(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]));

                
                //get startPosition from value
                string v2Data = item.value3;
                v2Data = v2Data.Substring(1, v2Data.Length - 2).Replace(',', ';');
                sArray = v2Data.Split(';');
                Vector3 startPosition = new Vector3(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]));

                //set player to targetPosition
                player.transform.position = targetPosition;


                if (DataVisualizerManager.Instance)
                {
                    DataVisualizerManager.Instance.TraceLineBetween(startPosition, targetPosition);
                    DataVisualizerManager.Instance.PlayerEnteredNewRoom();
                    DataVisualizerManager.Instance.AfterViewDirectionChange();
                }
            }

            if (item.action == "turnSwipeRight" || item.action == "turnButtonLeft")
            {
                inputManager.OnLeftButtonClick();
                if (DataVisualizerManager.Instance)
                {
                    DataVisualizerManager.Instance.AfterViewDirectionChange();
                }
            }

            if (item.action == "turnSwipeLeft" || item.action == "turnButtonRight")
            {
                inputManager.OnRightButtonClick();
                if (DataVisualizerManager.Instance)
                {
                    DataVisualizerManager.Instance.AfterViewDirectionChange();
                }
            }


            if (item.action == "selectFrame" || item.action == "slotToFrameClick")
            {
                GameObject frame;
                GameObject room;
                if (item.value1 == "16_9 Frame_(Default)" || item.value1 == "16_9 Frame_(Tutorial)")
                {
                    room = GameObject.Find("Entrance Hall");
                    frame = GameObject.Find(item.value1);
                }
                else
                {
                    room = GameObject.Find(item.value1);
                    frame = room.transform.Find("PictureFrames_Holder/" + item.value2).gameObject;
                }

                onPointerClickFrame(room, frame);
            }

            if (item.action == "frameToFrameClick")
            {

                GameObject room = GameObject.Find(item.value1);
                GameObject frame = room.transform.Find("PictureFrames_Holder/" + item.value3).gameObject;

                onPointerClickFrame(room, frame);
            }

            if (item.action == "picToFrameClick")
            {
                GameObject frame;
                GameObject room;
                if (item.value2 == "16_9 Frame_(Default)" || item.value1 == "16_9 Frame_(Tutorial)")
                {
                    room = GameObject.Find("Entrance Hall");
                    frame = GameObject.Find(item.value2);
                }
                else
                {
                    room = GameObject.Find(item.value1);
                    frame = room.transform.Find("PictureFrames_Holder/" + item.value3).gameObject;
                }

                onPointerClickFrame(room, frame);
            }



            if (item.action == "selectPic")
            {
                Debug.Log("selectPic name:" + item.value1);
                GameObject picture = GameObject.Find(item.value1);
                InteractivePicture interactivePictureScript = (InteractivePicture)picture.GetComponentInChildren(typeof(InteractivePicture));
                interactivePictureScript.OnPointerClick(ped);
            }

            if (item.action == "selectSlot")
            {
                Debug.Log("selectSlot name:" + item.value1);
                GameObject uiSlot = GameObject.Find(item.value1);

                onPointerClickSlot(uiSlot);
            }

            if (item.action == "frameToSlotClick")
            {

                GameObject uiSlot = GameObject.Find(item.value3);

                onPointerClickSlot(uiSlot);
            }

            if (item.action == "picToSlotClick")
            {

                GameObject uiSlot = GameObject.Find(item.value2);

                onPointerClickSlot(uiSlot);

                Debug.Log("Time till next Interaction:" + timeToWait);
                yield return new WaitForSeconds(timeToWait);
                tempDeltaTime = item.timeStamp;
            }

            if (item.action == "picToFrameDrag")
            {

                GameObject picture = GameObject.Find(item.value4);
                InteractivePicture interactivePictureScript = (InteractivePicture)picture.GetComponentInChildren(typeof(InteractivePicture));

                GameObject targetFrame;
                if (item.value1 == "16_9 Frame_(Default)" || item.value1 == "16_9 Frame_(Tutorial)")
                {
                    targetFrame = GameObject.Find(item.value1);
                }
                else
                {
                    GameObject room = GameObject.Find(item.value1);
                    targetFrame = room.transform.Find("PictureFrames_Holder/" + item.value2).gameObject;
                }



                GameObject background = targetFrame.transform.Find("16:9 Background").gameObject;

                interactivePictureScript.AttachPictureToPictureCanvas(background);
            }

            if (item.action == "frameToFrameDrag")
            {


                GameObject room = GameObject.Find(item.value1);
                GameObject startFrame = room.transform.Find("PictureFrames_Holder/" + item.value3).gameObject;



                GameObject startBackground = startFrame.transform.Find("16:9 Background").gameObject;

                InteractivePictureFrame interactivePictureFrameScript = (InteractivePictureFrame)startBackground.GetComponentInChildren(typeof(InteractivePictureFrame));


                GameObject[] pictureCanvases = new GameObject[2];


                GameObject targetFrame = room.transform.Find("PictureFrames_Holder/" + item.value2).gameObject;
                GameObject targetBackground = targetFrame.transform.Find("16:9 Background").gameObject;

                pictureCanvases[0] = startBackground;
                pictureCanvases[1] = targetBackground;

                interactivePictureFrameScript.OnEndDragNext(ped, null, pictureCanvases, startFrame.transform.position);
            }

            if (item.action == "slotToFrameDrag")
            {
                GameObject uiSlot = GameObject.Find(item.value3);
                GameObject border = uiSlot.transform.Find("Border").gameObject;
                GameObject itemImage = border.transform.Find("ItemImage (Raw)").gameObject;

                InteractiveUISlot interactiveUISlotScript = (InteractiveUISlot)itemImage.GetComponentInChildren(typeof(InteractiveUISlot));

                GameObject targetFrame = null;

                if (item.value1 == "16_9 Frame_(Default)" || item.value1 == "16_9 Frame_(Tutorial)")
                {
                    targetFrame = GameObject.Find(item.value1);
                }
                else
                {
                    GameObject room = GameObject.Find(item.value1);
                    targetFrame = room.transform.Find("PictureFrames_Holder/" + item.value2).gameObject;
                }

                GameObject targetBackground = targetFrame.transform.Find("16:9 Background").gameObject;


                Debug.Log("slotToFrameDrag::_" + uiSlot.transform.position);
                // yield return new WaitForSeconds(timeTillNextAction*5);
                interactiveUISlotScript.OnEndDragNext(targetBackground, ped, uiSlot.transform.position);

            }



            if (item.action == "frameToSlotDrag")
            {
                GameObject startFrame;
                if (item.value2 == "16_9 Frame_(Default)" || item.value1 == "16_9 Frame_(Tutorial)")
                {
                    startFrame = GameObject.Find(item.value2);
                }
                else
                {
                    GameObject room = GameObject.Find(item.value2);
                    startFrame = room.transform.Find("PictureFrames_Holder/" + item.value3).gameObject;
                }



                GameObject startBackground = startFrame.transform.Find("16:9 Background").gameObject;

                InteractivePictureFrame interactivePictureFrameScript = (InteractivePictureFrame)startBackground.GetComponentInChildren(typeof(InteractivePictureFrame));

                GameObject uiSlot = GameObject.Find(item.value1);
                GameObject border = uiSlot.transform.Find("Border").gameObject;
                GameObject itemImage = border.transform.Find("ItemImage (Raw)").gameObject;

                Debug.Log("frameToSlotDrag");
                interactivePictureFrameScript.OnEndDragNext(ped, itemImage, null, startFrame.transform.position);
            }

            if (item.action == "picToSlotDrag" || item.action == "picToSlotDragSwap")
            {
                Debug.Log("start Pic to SLot");
                GameObject picture = GameObject.Find(item.value2);
                InteractivePicture interactivePictureScript = (InteractivePicture)picture.GetComponentInChildren(typeof(InteractivePicture));

                GameObject uiSlot = GameObject.Find(item.value1);

                GameObject border = uiSlot.transform.Find("Border").gameObject;
                GameObject itemImage = null;
                itemImage = border.transform.Find("ItemImage (Raw)").gameObject;



                interactivePictureScript.OnEndDragNext(ped, itemImage);
            }



            if (item.action == "deselect")
            {

                //get room P1
                GameObject room = GameObject.Find("Entrance Hall");
                Deselector deselector = (Deselector)room.GetComponentInChildren(typeof(Deselector));

                deselector.OnPointerClick(ped);
            }


            if (item.action == "openMap")
            {
                openMapButton.onClick.Invoke();
            }


            if (item.action == "closeMap")
            {
                closeMapButton.onClick.Invoke();
            }

            if (item.action == "audio")
            {

                Debug.Log("Time till next Interaction:" + messageDuration);
                StartCoroutine(showReplayerMessage("audio turned on: " + item.value1));
            }

            if (item.action == "setLights")
            {
                Debug.Log("Time till next Interaction:" + messageDuration);
                StartCoroutine(showReplayerMessage("lights turned on: " + item.value1));
            }

            if (item.action == "setInteriorVisibility")
            {
                Debug.Log("Time till next Interaction:" + messageDuration);
                StartCoroutine(showReplayerMessage("interior turned on: " + item.value1));
            }

            if (item.action == "setWayfindingVisibility")
            {
                Debug.Log("Time till next Interaction:" + messageDuration);
                StartCoroutine(showReplayerMessage("wayfinding turned on: " + item.value1));
            }

            if (item.action == "setCompassVisibility")
            {
                Debug.Log("Time till next Interaction:" + messageDuration);
                StartCoroutine(showReplayerMessage("compass turned on: " + item.value1));
            }

            if (item.action == "setMapVisibility")
            {
                Debug.Log("Time till next Interaction:" + messageDuration);
                StartCoroutine(showReplayerMessage("map turned on: " + item.value1));
            }


            if (item.action == "endGame")
            {
                StartCoroutine(showReplayerMessage("end of replayer"));
                Debug.Log("end of replayer");
            }

            Debug.Log("Time till next Interaction:" + timeToWait);
            yield return new WaitForSeconds(timeToWait);
            tempDeltaTime = item.timeStamp;
        }
        yield return new WaitForSeconds(1);
    }

    void onPointerClickFrame(GameObject room, GameObject frame)
    {
        //benötigt keine ped.position für frame to frame when clicked

        GameObject background = frame.transform.Find("16:9 Background").gameObject;
        InteractivePictureFrame interactivePictureFrameScript = (InteractivePictureFrame)background.GetComponentInChildren(typeof(InteractivePictureFrame));


        interactivePictureFrameScript.OnPointerClick(ped);
    }



    void onPointerClickSlot(GameObject uiSlot)
    {
        GameObject border = uiSlot.transform.Find("Border").gameObject;
        GameObject itemImage = border.transform.Find("ItemImage (Raw)").gameObject;
        InteractiveUISlot interactivePictureFrameScript = (InteractiveUISlot)itemImage.GetComponentInChildren(typeof(InteractiveUISlot));


        interactivePictureFrameScript.OnPointerClick(ped);
    }

    private IEnumerator showReplayerMessage(string message)
    {
        messageUI.SetActive(true);
        messageText.text = message;
        yield return new WaitForSeconds(messageDuration);
        messageUI.SetActive(false);
    }
}
