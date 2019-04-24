using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitDoor : DoorScript {

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        Debug.Log("Exit Door? " +base.name);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        Debug.Log("Clicking on exit door" + " duratoin: " + GetDoorAnimationLength());
        StartCoroutine(CloseApp());
    }

    public IEnumerator CloseApp()
    {
        float doorAnimationDuration = GetDoorAnimationLength();
        float t = 0f;

        while (t < doorAnimationDuration)
        {
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Application.Quit();
    }
}
