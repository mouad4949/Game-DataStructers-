using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
      public float speed = 25f;
    public float detectionDistance = 80f; // La distance à laquelle l'ennemi détectera le joueur
    

    // Private variables
    private Rigidbody2D rigidbody2d;
    private GridA grid;

    // Reference to the player
    private GameObject player;
    private Vector2 targetPosition;

    public Transform target;
    public Player PlayerMovement;
    public int healthBoss=200;
    
    public float damage = 25f;
     public GameManagerScript gameManager;
   

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
            
           

            // Tu peux ajouter ici des effets de collision supplémentaires si nécessaire
            target = null;
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {  
            healthBoss-=10;
            Destroy(collision.gameObject);
            if(healthBoss<=0){
             gameObject.SetActive(false);
             gameManager.Won();
        }
        
    }
    }
}
