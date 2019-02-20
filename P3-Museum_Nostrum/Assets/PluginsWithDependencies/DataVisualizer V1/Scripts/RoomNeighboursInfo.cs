using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNeighboursInfo : MonoBehaviour {

    [Tooltip("Required for DataVisualizerManager.InitLinesBetweenAllRooms()")]
    public List<GameObject> allNeighbours = new List<GameObject>();

	public List<GameObject> GetNeighbours()
    {
        return allNeighbours;
    }
}
