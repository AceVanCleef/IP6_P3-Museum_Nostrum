using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deselector : AbstractInteractiveGameObject
{


    protected new virtual void Start()
    {
        base.Start();
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
        Debug.Log("Hit DeselectPlane");
    }
    #endregion UserInput

}
