using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntVal : MonoBehaviour {

    private Text t;

    private int intVal;

    public int GetIntVal()
    {
        return intVal;
    }

    public void SetIntVal(int value)
    {
        intVal = value;
    }

    // Use this for initialization
    void Start () {
        t = GetComponent<Text>();

        Debug.Log("starting IntVal.cs");

    }

    // Update is called once per frame
    void Update () {
        SetIntVal(DataCarrier.Instance.IntValue);
        t.text = intVal.ToString();
        Debug.Log("updating IntVal.cs");

    }
}
