using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class JSONGenerator : MonoBehaviour
{


    private List<string> listString = new List<string>();

    // Use this for initialization
    void Start()
    {
        SaveItemInfo();
    }

    public void SaveItemInfo()
    {
        string path = null;
        string mainObject;
        //#if UNITY_EDITOR
        string persistentDataPath = Application.persistentDataPath;
        path = persistentDataPath + "/DataLoggerFiles/DataLogger_" + System.DateTime.Now.ToString("dd-MM-yy_hh-mm-ss") + ".json";
        Debug.Log("pfad" + path);
        mainObject = "{\"prototypVersion\" : \"Prototyp3\", \"level\" : \"" + SceneManager.GetActiveScene().name + "\", \"entries\" :[" + System.Environment.NewLine;

        //#endif
        //#if UNITY_STANDALONE
        // You cannot add a subfolder, at least it does not work for me
        // path = "MyGame_Data/Resources/ItemInfo.json"
        //#endif

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
