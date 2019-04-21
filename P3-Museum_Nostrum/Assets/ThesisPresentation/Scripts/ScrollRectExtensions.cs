using UnityEngine;
using UnityEngine.UI;

public static class ScrollRectExtensions {

	public static void ScrollToMousePosition(this ScrollRect sc)
    {
        float xPosInPercent = Input.mousePosition.x / Screen.width;
        float yPosInPercent = Input.mousePosition.y / Screen.height;
        sc.normalizedPosition = new Vector2(xPosInPercent, yPosInPercent);
    }
}
