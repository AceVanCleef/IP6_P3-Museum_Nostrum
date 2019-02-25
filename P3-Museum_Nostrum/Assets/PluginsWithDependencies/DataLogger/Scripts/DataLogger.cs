﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataLogger : MonoBehaviour {

    public static DataLogger instance = null;

    private List<KeyValuePair<float, Action>> dataList = new List<KeyValuePair<float, Action>>();

    private JSONGenerator jSonGenerator;

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

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //get DataLogger
        GameObject go = GameObject.Find("JSONGenerator");
        jSonGenerator = (JSONGenerator)go.GetComponent(typeof(JSONGenerator));
        
    }

    //Initializes the game for each level.
    public void Log(string actionName, string value1, string value2, string value3, string value4, string value5)
    {
        Debug.Log(Time.time + "_Action" + actionName + "_value1" + value1 + "_value2" + value2 + "_value3" + value3 + "_value4" + value4 + "_value5" + value5);
        dataList.Add(new KeyValuePair<float, Action>(Time.time, new Action()
        {
            ActionName = actionName,
            Data1 = value1,
            Data2 = value2,
            Data3 = value3,
            Data4 = value4,
            Data5 = value5,
        }));
    }

    public void PrintDataList()
    {
        string json = null;
        foreach (var item in dataList)
        {
             Debug.Log("DataList//" + item.Value.ActionName + "//" + item.Key + "//" + item.Value.Data1 + "//" + item.Value.Data2 + "//" + item.Value.Data3 + "//" + item.Value.Data4);
            jSonGenerator.timeStamp = item.Key;
            jSonGenerator.action = item.Value.ActionName;
            jSonGenerator.value1 = item.Value.Data1;
            jSonGenerator.value2 = item.Value.Data2;
            jSonGenerator.value3 = item.Value.Data3;
            jSonGenerator.value4 = item.Value.Data4;
            jSonGenerator.value5 = item.Value.Data5;

            json = JsonUtility.ToJson(jSonGenerator);
            jSonGenerator.jsonToList(json);
        }

        jSonGenerator.SaveItemInfo();
    }

    public struct Action
    {
        public string ActionName;
        public string Data1;
        public string Data2;
        public string Data3;
        public string Data4;
        public string Data5;
    }
}
