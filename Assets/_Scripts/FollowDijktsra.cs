using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDjikstra : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;

    private List<Vector3> path;
    private int currentPathIndex;

    void Update()
    {
        if (path != null && currentPathIndex < path.Count)
        {
            MovePlayerAlongPath();
        }
    }

    public void SetPath(List<Vector3> newPath)
    {
        Debug.Log("Chemin reçu: " + newPath.Count + " waypoints");
        path = newPath;
        currentPathIndex = 0;
    }

    void MovePlayerAlongPath()
    {
        if (path == null || currentPathIndex >= path.Count)
            return;

        Vector3 targetPosition = path[currentPathIndex];
        Debug.Log("Se déplaçant vers: " + targetPosition);
        player.position = Vector3.MoveTowards(player.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(player.position, targetPosition) < 0.1f)
        {
            currentPathIndex++;
            Debug.Log("Prochain waypoint: " + currentPathIndex);
        }
    }
}