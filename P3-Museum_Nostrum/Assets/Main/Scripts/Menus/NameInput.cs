using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameInput : MonoBehaviour {

    private string playerName;
    private string greeting = "Guten Tag ";
    private string askForMuseumname = "\nKlicken Sie auf die Tür, um das Museum zu betreten";
    private string museumName;
    public InputField playerField;
    public InputField museumField;
    public Text museumText;
    public Button playerNextButton;
    public Button museumNextButton;

    // Use this for initialization
    void Start () {
        playerNextButton.onClick.AddListener(onClickPlayer);
        playerNextButton.onClick.AddListener(onClickMuseum);
    }

    // saves the inputField input when button playerNextButton is clicked
    public void onClickPlayer()
    {
        playerName = playerField.text;
        changeMuseumText();
    }

    // saves the inputField input when button museumNextButton is clicked
    public void onClickMuseum()
    {
        museumName = museumField.text;
    }

    //shows the previously given name of the player in a textbox
    private void changeMuseumText()
    {
        museumText.text = greeting + playerName + askForMuseumname;
    }
}
