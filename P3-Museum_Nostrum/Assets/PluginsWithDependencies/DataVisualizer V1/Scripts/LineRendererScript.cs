using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererScript : MonoBehaviour {

    private LineRenderer lr;
    [SerializeField]
    private Vector3 startPos = Vector3.zero;
    [SerializeField]
    private Vector3 endPos = Vector3.zero;

    public float deltaWidth = 0.05f;
    public float initialWidth = 0.1f;

    public Color InitColor = Color.grey;

    //statistics
    [SerializeField]
    private int routUsedCount = 0;


    public void Initialize(GameObject fromGO, GameObject toGO)
    {
        if (fromGO == null || toGO == null) return;
        //Todo: identity check.
        //Todo initialized check.
        startPos = fromGO.transform.position;
        endPos = toGO.transform.position;
        lr = GetComponent<LineRenderer>();
        lr.material.color = InitColor;
        lr.positionCount = 2;
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
        SetWidth(initialWidth, initialWidth);
    }

    public void Initialize(Vector3 from, Vector3 to)
    {
        if (from == null || to == null) return;
        //Todo: identity check.
        //Todo initialized check.
        startPos = from;
        endPos = to;
        lr = GetComponent<LineRenderer>();
        lr.material.color = InitColor;
        lr.positionCount = 2;
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
        SetWidth(initialWidth, initialWidth);
        routUsedCount = 1;
    }

    private void SetWidth(float start, float end)
    {
        lr.startWidth = start;
        lr.endWidth = end;
    }

    public bool IsLineBetween(Vector3 start, Vector3 end)
    {
        return Vector3.Distance(start, startPos) < 0.001f && Vector3.Distance(end, endPos) < 0.001f;
    }

    public void IncrementWidth()
    {
        routUsedCount++;
        SetWidth(lr.startWidth + deltaWidth, lr.endWidth + deltaWidth);
    }

}
