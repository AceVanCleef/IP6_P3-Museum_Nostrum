using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


/// <summary>
/// lets you replay a whole gameplay with all interactions and optionaly with visualizations. receives all actions from a JSON file.
/// </summary>
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

    //objects needed for replay
    private GameObject player;
    private InputManager inputManager;
    private PointerEventData ped;

    //ui elements needed for replay
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
        //gets list from JSONParser. list is created from JSON file.
        JSONParser jsonParser = transform.parent.GetComponentInChildren<JSONParser>();

        //needed for movement
        player = GameObject.Find("Player");
        inputManager = (InputManager)player.GetComponent(typeof(InputManager));

        //get EventSystem for eventData
        GameObject go = GameObject.Find("EventSystem");
        EventSystem eventSystem = go.GetComponent<EventSystem>();
        ped = new PointerEventData(eventSystem);
        
        //deactivate welcomeMessage. is not needed for replay
        welcomeMessageUI.SetActive(false);

        //executes every action in the JSON file. 
        foreach (var item in jsonParser.listActions)
        {
            //sets time to wait. depending if you are using realtime(useDeltaTime = true) between to actions or a constant time intervall (useDeltaTime = false)
            timeToWait = breakTime;
            if (useDeltaTime)
            {
                timeToWait = item.timeStamp - tempDeltaTime;
            }

            Debug.Log("Executing: " + item.action + " Stamp: " + item.timeStamp);

            //draws point where a touch was registered
            if (item.action == "touch")
            {
                string v2Data = item.value2;
                v2Data = v2Data.Substring(1, v2Data.Length - 2).Replace(',', ';');
                string[] sArray = v2Data.Split(';');

                Vector2 touchtPosition = new Vector2(float.Parse(sArray[0]), float.Parse(sArray[1]));
                
                
                if (DataVisualizerManager.Instance)
                    DataVisualizerManager.GUI.DrawTouch(touchtPosition);
            }

            //draws a line where a swipe was registered
            if (item.action == "SwipeDetected")
            {
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

                SwipeData swipeData = new SwipeData()
                {
                    Direction = direction,
                    StartPosition = startPosition,
                    EndPosition = endPosition
                };

                if (DataVisualizerManager.Instance)
                    DataVisualizerManager.GUI.DrawSwipe(swipeData);
            }

            //draws a line where a drag n drop was registered
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

            //OnPointerClick method in interactivePictureFrame is called
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

            //OnPointerClick method in interactivePictureFrame is called
            if (item.action == "frameToFrameClick")
            {

                GameObject room = GameObject.Find(item.value1);
                GameObject frame = room.transform.Find("PictureFrames_Holder/" + item.value3).gameObject;

                onPointerClickFrame(room, frame);
            }

            //OnPointerClick method in interactivePictureFrame is called
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


            //OnPointerClick method in interactivePicture is called
            if (item.action == "selectPic")
            {
                Debug.Log("selectPic name:" + item.value1);
                GameObject picture = GameObject.Find(item.value1);
                InteractivePicture interactivePictureScript = (InteractivePicture)picture.GetComponentInChildren(typeof(InteractivePicture));
                interactivePictureScript.OnPointerClick(ped);
            }

            //OnPointerClick method in interactiveSlot is called
            if (item.action == "selectSlot")
            {
                Debug.Log("selectSlot name:" + item.value1);
                GameObject uiSlot = GameObject.Find(item.value1);

                onPointerClickSlot(uiSlot);
            }

            //OnPointerClick method in interactiveSlot is called
            if (item.action == "frameToSlotClick")
            {

                GameObject uiSlot = GameObject.Find(item.value3);

                onPointerClickSlot(uiSlot);
            }
            
            //OnPointerClick method in interactiveSlot is called
            if (item.action == "picToSlotClick")
            {

                GameObject uiSlot = GameObject.Find(item.value2);

                onPointerClickSlot(uiSlot);

                Debug.Log("Time till next Interaction:" + timeToWait);
                yield return new WaitForSeconds(timeToWait);
                tempDeltaTime = item.timeStamp;
            }

            //OnEndDrag method in interactivePicture is called
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

            //OnEndDrag method in interactivePictureFrame is called
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

            //OnEndDrag method in interactiveSlot is called
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

            //OnEndDrag method in interactivePictureFrame is called
            if (item.action == "frameToSlotDrag")
            {
                GameObject startFrame;
                if (item.value2 == "16_9 Frame_(Default)" || item.value2 == "16_9 Frame_(Tutorial)")
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

            //OnEndDrag method in interactiveSlot is called
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

            //OnPointerClick method in Deselector is called
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

            if (item.action == "setAudioBreadcrumbsAudability")
            {
                StartCoroutine(showReplayerMessage("audio turned on: " + item.value1));
            }

            if (item.action == "setLights")
            {
                StartCoroutine(showReplayerMessage("lights turned on: " + item.value1));
            }

            if (item.action == "setInteriorVisibility")
            {
                StartCoroutine(showReplayerMessage("interior turned on: " + item.value1));
            }

            if (item.action == "setWayfindingVisibility")
            {
                StartCoroutine(showReplayerMessage("wayfinding turned on: " + item.value1));
            }

            if (item.action == "setCompassVisibility")
            {
                StartCoroutine(showReplayerMessage("compass turned on: " + item.value1));
            }

            if (item.action == "setMapVisibility")
            {
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
    }

    void onPointerClickFrame(GameObject room, GameObject frame)
    {
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

    /// <summary>
    /// shows short message with the information if a certain factor was toggled on or off
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private IEnumerator showReplayerMessage(string message)
    {
        Debug.Log("Time till next Interaction:" + messageDuration);
        messageUI.SetActive(true);
        messageText.text = message;
        yield return new WaitForSeconds(messageDuration);
        messageUI.SetActive(false);
    }
}
