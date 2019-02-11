using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlotHighlighter : MonoBehaviour, IHighlighter
{

    private Image img;

	// Use this for initialization
	void Start () {
        img = GetComponent<Image>();

        //Guarantee correct start transparency.
        Off();
    }
	
	public void On()
    {
        //Todo: evtl. check whether slot is free
        Color current = img.color;
        current.a = 0.75f;
        img.color = current;
    }

    public void Off()
    {
        Color current = img.color;
        current.a = 0f;
        img.color = current;
    }
}
