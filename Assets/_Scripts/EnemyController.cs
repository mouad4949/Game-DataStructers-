using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Public variables
    public float speed = 15f;
    public float detectionDistance = 80f; // La distance à laquelle l'ennemi détectera le joueur
    public LayerMask unwalkableMask;

    // Private variables
    private Rigidbody2D rigidbody2d;
    private GridA grid;

    // Reference to the player
    private GameObject player;
    private Vector2 targetPosition;

    public Transform target;
    public Player PlayerMovement;
    
    public float damage = 10f;
    public CoinManager coinManager;

    // Start is called before the first frame update
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Assure-toi que PlayerMovement est bien assigné dans l'inspecteur de Unity
            if (PlayerMovement != null)
            {
                PlayerMovement.health -= damage;
            }
            else
            {
                Debug.LogWarning("PlayerMovement n'est pas assigné dans l'inspecteur.");
            }

            // Tu peux ajouter ici des effets de collision supplémentaires si nécessaire
            target = null;
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {   // Augmenter enemiesKilled dans le CoinManager lorsque cet ennemi est détruit
            if (coinManager != null)
            {
                coinManager.enemiesKilled++;
            }
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        
    }
}
