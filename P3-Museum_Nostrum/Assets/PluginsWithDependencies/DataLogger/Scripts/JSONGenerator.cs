using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class JSONGenerator : MonoBehaviour
{
    public float timeStamp;
    public string action;
    public string value1;
    public string value2;
    public string value3;
    public string value4;
    public string value5;
    public string value6;
    public string value7;

    private List<string> listString = new List<string>();

    private static JSONGenerator instance = null;
    public static JSONGenerator Instance
    {
        get
        {
            return instance;
        }
    }

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

     
    }

    /// <summary>
    /// adds one entry to listString
    /// </summary>
    /// <param name="json">one entry</param>
    public void jsonToList(string json)
    {
        listString.Add(json);
    }

    /// <summary>
    /// creates JSON file at specific dataPath
    /// </summary>
    public void SaveItemInfo()
    {
        string path = null;
        string mainObject;

        //first line in JSON file. general information about the content
        mainObject = "{\"prototypVersion\" : \"Prototyp3\", \"level\" : \"" + SceneManager.GetActiveScene().name + "\", \"entries\" :[" + System.Environment.NewLine;

        string persistentDataPath = Application.persistentDataPath;

        //path for creation of JSON files in editor mode uf unity 
        //path = "Assets/PluginsWithDependencies/DataLogger/DataLoggerFiles/DataLogger_" + System.DateTime.Now.ToString("dd-MM-yy_hh-mm-ss") + ".json";

        //path for creation of JSON files on android or iOS tablets
        path = persistentDataPath + "/DataLogger_" + System.DateTime.Now.ToString("dd-MM-yy_hh-mm-ss") + ".json";

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {

            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(mainObject);
                foreach (var item in listString)
                {
                    writer.Write(item + "," + System.Environment.NewLine);
                }
                writer.Write("]" + "}");
            }

        }
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif
    }
}
