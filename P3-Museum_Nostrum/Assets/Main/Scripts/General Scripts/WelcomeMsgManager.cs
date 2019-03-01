using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeMsgManager : MonoBehaviour {

    [Header("Dialogue Sequence")]
    [Tooltip("Add any TextMeshPro based dialogue.")]
    public List<TextMeshProUGUI> allMessages = new List<TextMeshProUGUI>();

    private int currentMsgIndex = 0;

    [Space(10)]

    [Header("Buttons")]
    [Tooltip("Add the button responsible for continueing the dialogue.")]
    public Button NextButton;

	// Use this for initialization
	void Start () {
        //ensure right setup.
        if (allMessages.Count > 0)
        {
            allMessages[0].gameObject.SetActive(true);
            for (int i = 1; i < allMessages.Count; ++i)
            {
                allMessages[i].gameObject.SetActive(false);
            }

            SetUpButtonClickhandler();
        }

    }

    private void SetUpButtonClickhandler()
    {
        NextButton.onClick.AddListener(delegate { ShowNextDialogue();  });
    }

    public void ShowNextDialogue()
    {
        if (currentMsgIndex < allMessages.Count - 1)
        {
            allMessages[currentMsgIndex].gameObject.SetActive(false);
            ++currentMsgIndex;
            //show next dialogue message.
            allMessages[currentMsgIndex].gameObject.SetActive(true);

            //adjust button message in case last dialogue is being shown.
            if (currentMsgIndex == allMessages.Count - 1)
            {
                NextButton.GetComponentInChildren<Text>().text = "Los Geht's!";
            }

        }
        else
        {
            //deactivate dialogue window.
            gameObject.SetActive(false);
        }
        Debug.Log("current msg idx: " + currentMsgIndex);

    }
}
