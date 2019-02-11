using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlotHighlighter : MonoBehaviour {

    private Image img;

	// Use this for initialization
	void Start () {
        img = GetComponent<Image>();

        //Guarantee correct start transparency.
        DeactivateHighlightning();
    }
	
	public void HighlightSlot()
    {
        //Todo: evtl. check whether slot is free
        Color current = img.color;
        current.a = 0.75f;
        img.color = current;
    }

    public void DeactivateHighlightning()
    {
        Color current = img.color;
        current.a = 0f;
        img.color = current;
    }
}
