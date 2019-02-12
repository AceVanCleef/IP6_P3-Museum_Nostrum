using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// highlights the UISlot in its outlineColor when activated.
/// </summary>
public class UISlotHighlighter : MonoBehaviour, IHighlighter
{

    [Tooltip("Define the highlightning color.")]
    public Color outlineColor;
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
        Color current = outlineColor;
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
