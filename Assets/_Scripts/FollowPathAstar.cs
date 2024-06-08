using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathAstar : MonoBehaviour
{
    public float speed = 5f;  // Vitesse de déplacement du joueur
    public float stoppingDistance =10f;  // Distance à partir de laquelle le joueur arrêtera de suivre le chemin
    public Transform boss;  // Référence au transform du boss

    private List<Node> path;  // Chemin à suivre
    private int targetIndex;  // Indice du noeud cible actuel
    private bool isFollowingPath;  // Indique si le joueur suit actuellement un chemin
    public Player player;

    void Start()
    {
        path = new List<Node>();
        // isFollowingPath = false;
    }

    void Update()
    {
        if (!isFollowingPath)
        {
            player.HandleMovement();
        }
        
    }

    public void StartPath(List<Node> newPath)
    {
        if (newPath != null)
        {
            path = newPath;
            targetIndex = 0;
            isFollowingPath = true;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    { 
        if (path.Count == 0)
        {
            
            yield break;
        }

        Vector3 currentWaypoint = path[0].worldPosition;
        while (true)
        {
            // Vérifiez la distance entre le joueur et le boss
            float distanceToBoss = Vector3.Distance(transform.position, boss.position);
            if (distanceToBoss < stoppingDistance)
            {   
                // Arrêtez de suivre le chemin et passez en mode manuel
                yield break;
            }

            if (Vector3.Distance(transform.position, currentWaypoint) < 0.1f)
            {
                targetIndex++;
                if (targetIndex >= path.Count)
                {
                    isFollowingPath = false;
                    yield break;
                }
                currentWaypoint = path[targetIndex].worldPosition;
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
        
    }

   
}