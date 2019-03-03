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

    public void jsonToList(string json)
    {
        listString.Add(json);
    }

    public void SaveItemInfo()
    {
        string path = null;
        string mainObject;

        //path = "Assets/PluginsWithDependencies/DataLogger/DataLoggerFiles/DataLogger_" + System.DateTime.Now.ToString("dd-MM-yy_hh-mm-ss") + ".json";

        mainObject = "{\"prototypVersion\" : \"Prototyp3\", \"level\" : \"" + SceneManager.GetActiveScene().name + "\", \"entries\" :[" + System.Environment.NewLine;

        string persistentDataPath = Application.persistentDataPath;
        path = persistentDataPath + "/DataLogger_" + System.DateTime.Now.ToString("dd-MM-yy_hh-mm-ss") + ".json";

        Debug.Log("Pfad:" + path);
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {

            using (StreamWriter writer = new StreamWriter(fs))
            {

                writer.Write(mainObject);
                foreach (var item in listString)
                {
                    Debug.Log("JSONGene" + item);
                    writer.Write(item + "," + System.Environment.NewLine);
                }
                writer.Write("]" + "}");
            }

        }
        Debug.Log("End:" + path);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
