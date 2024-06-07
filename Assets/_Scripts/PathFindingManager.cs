using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    public GameObject pathfindingObject;  // Assign the GameObject containing the Pathfinding script

    void Start()
    {
        pathfindingObject.SetActive(false);  // Ensure it is initially disabled
    }

    public void ActivatePathfinding()
    {
        pathfindingObject.SetActive(true);
    }
}