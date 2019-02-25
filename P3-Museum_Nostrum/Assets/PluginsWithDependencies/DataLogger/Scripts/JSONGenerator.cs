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

    private List<string> listString = new List<string>();

    public static JSONGenerator instance = null;

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
        #if UNITY_EDITOR
        
        path = "Assets/PluginsWithDependencies/DataLogger/DataLoggerFiles/DataLogger_" + System.DateTime.Now.ToString("dd-MM-yy_hh-mm-ss") + ".json";
        Debug.Log("pfad" + path);
        mainObject = "{\"prototypVersion\" : \"Prototyp3\", \"level\" : \"" + SceneManager.GetActiveScene().name + "\", \"entries\" :[" + System.Environment.NewLine;

#endif
#if UNITY_STANDALONE
        string persistentDataPath = Application.persistentDataPath;
      //  path = persistentDataPath + "/DataLoggerFiles/DataLogger_" + System.DateTime.Now.ToString("dd-MM-yy_hh-mm-ss") + ".json";
#endif

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
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
