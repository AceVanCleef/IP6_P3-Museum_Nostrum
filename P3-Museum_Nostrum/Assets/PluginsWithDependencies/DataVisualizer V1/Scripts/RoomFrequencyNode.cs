using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFrequencyNode : MonoBehaviour {

    [SerializeField]
    private float DeltaSizeIncrease = 0.1f;
    [SerializeField]
    private int visitCount = 0; //how often a room got visited.

    public void PlayerEnteredRoom()
    {
        Vector3 temp = transform.localScale;
        temp.x += DeltaSizeIncrease;
        temp.y += DeltaSizeIncrease;
        temp.z += DeltaSizeIncrease;
        transform.localScale = temp;
        visitCount++;
    }
}
