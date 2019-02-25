using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deselector : AbstractInteractiveGameObject
{

    private DataLogger dataLogger;
    protected new virtual void Start()
    {
        base.Start();
        if (!GetComponent<Collider>())
        {
            string parentName = "";
            if (transform.parent)
            {
                parentName = " in " + transform.parent.name;
            }
            Debug.LogWarning("Collider missing at " + gameObject.name + parentName + 
                ". User will be unable to deselect interactive GameObjects if touching this part of this room." +
                " Ensure each wall and ceiling contain a box collider and each floor a mesh collider.");
        }

        //get DataLogger
        GameObject go = GameObject.Find("DataLogger");
        dataLogger = (DataLogger)go.GetComponent(typeof(DataLogger));
    }

    #region UserInput
    public override void OnBeginDrag(PointerEventData eventData)
    {
    }


    public override void OnEndDrag(PointerEventData eventData)
    {

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Hit Deselector.");
        dataLogger.Log("deselect", eventData.position.ToString(), null, null, null, null);
        Deselect();
    }
    #endregion UserInput

}
