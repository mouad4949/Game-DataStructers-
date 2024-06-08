using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    public GameObject pathfindingObject;
    public GameObject GridA;
    public GridA gA;  // Assign the GameObject containing the Pathfinding script
    private bool isSkipClicked = false;  // Track if Skip button was clicked
    void Start()
    {
       pathfindingObject.SetActive(false);  // Ensure it is initially disabled
    }

    public void ActivatePathfinding()
    {
        pathfindingObject.SetActive(true);
        Debug.Log("canvas A* , D");
    }
    
    public void DeactivatePathfinding()
    {
        Debug.Log("Deactivating Pathfinding");
        pathfindingObject.SetActive(false);
    }

    public void ActivateGridA()
    {
        Debug.Log("A* active");
        
        GridA.SetActive(true);
        gA.traceGrid();
         // Hide pathfindingObject when GridA is activated
    }
    
    public bool IsGridAActive()
    {
        return GridA.activeSelf;
    }

    public void DesaactivateGridA()
    {   
        Debug.Log("A* desactive");

        GridA.SetActive(false); 
    }

    
    public void Skip()
    {
        isSkipClicked = true;  // Set flag to true when Skip button is clicked
        pathfindingObject.SetActive(false);
    }
    
    public bool IsSkipClicked()
    {
        return isSkipClicked;  // Return the value of the flag
    }

    public void ResetSkip()
    {
        isSkipClicked = false;  // Reset the flag
    }
}
