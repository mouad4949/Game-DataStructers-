using UnityEngine;

public class Boss : MonoBehaviour
{
    // Instance statique du boss
    private static Boss instance;
    public float speed = 15;
    public float detectionDistance = 80f;


    Rigidbody2D rigidbody2d;

    GridA grid;

    GameObject player;
    Vector2 targetPosition;


    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        grid = FindObjectOfType<GridA>();

        // Find the player object by tag
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        // If the player exists and is within detectionDistance, calculate the direction towards the player
        if (player != null && Vector2.Distance(transform.position, player.transform.position) <= detectionDistance)
        {
            // Calculate direction towards the player
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // Move towards the player
            rigidbody2d.MovePosition(rigidbody2d.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    
    
}
