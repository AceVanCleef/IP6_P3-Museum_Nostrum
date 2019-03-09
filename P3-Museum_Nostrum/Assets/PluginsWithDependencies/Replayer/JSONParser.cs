using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class JSONParser : MonoBehaviour
{

    //list of all actions found in json file
    public List<JsonAction> listActions = new List<JsonAction>();


    public string path;

    // Use this for initialization
    void Awake()
    {
        readString();
    }

    void readString()
    {
        StreamReader reader = new StreamReader(path);

        string jsonLine;
        bool isLastLine = false;

        JsonAction myObject = new JsonAction();

        //skip first line
        reader.ReadLine();

        while (reader.Peek() >= 0)
        {

            jsonLine = reader.ReadLine();

            //couldnt be checked in the next if => workaround with bool
            if (jsonLine == "]}")
            {
                isLastLine = true;
            }

            if (jsonLine != "" && !isLastLine)
            {
                jsonLine = jsonLine.Remove(jsonLine.Length - 1);
                myObject = JsonUtility.FromJson<JsonAction>(jsonLine);
                listActions.Add(myObject);
            }

        }
        Debug.Log("JSONFinish");
        reader.Close();
    }
}

