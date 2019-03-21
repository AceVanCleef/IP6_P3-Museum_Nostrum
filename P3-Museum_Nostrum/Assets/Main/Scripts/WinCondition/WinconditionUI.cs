using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// visualizes the current count of found pictures on the GUI.
/// </summary>
public class WinconditionUI : MonoBehaviour {

    [SerializeField] private GameObject CurrentCountGO;
    [SerializeField] private GameObject TotalCountGO;
    [SerializeField] private GameObject WinGuidanceMessageGO;
    [SerializeField] private GameObject WonMessageGO;

    private TextMeshProUGUI currentCountTxt;
    private TextMeshProUGUI totalCountTxt;

    void Awake()
    {
        InitWinConditionGUI();
    }

    // Use this for initialization
    void Start () {
		if (!WinConditionManager.Instance)
        {
            gameObject.SetActive(false);
            Debug.Log("Missing WinConditionManager instance.");
        }
	}

    private void InitWinConditionGUI()
    {
        //ensure text messages are hidden
        HideWinGuidanceMsg();
        HideWonMsg();

        //get components
        currentCountTxt = CurrentCountGO.GetComponent<TextMeshProUGUI>();
        totalCountTxt = TotalCountGO.GetComponent<TextMeshProUGUI>();
    }
	
    /// <summary>
    /// sets the UI text to current picture count.
    /// </summary>
    /// <param name="c"></param>
	public void SetCurrentCountTo(int c)
    {
        currentCountTxt.text = c.ToString();
    }

    /// <summary>
    /// sets the UI text to total (or initial) picture count.
    /// </summary>
    /// <param name="c"></param>
    public void SetTotalCountTo(int c)
    {
        totalCountTxt.text = c.ToString();
    }

    public void ShowWinGuidanceMsg()
    {
        WinGuidanceMessageGO.SetActive(true);
    }

    public void ShowWonMsg()
    {
        WonMessageGO.SetActive(true);
    }

    public void HideWinGuidanceMsg()
    {
        WinGuidanceMessageGO.SetActive(false);
    }

    public void HideWonMsg()
    {
        WonMessageGO.SetActive(false);
    }
}
