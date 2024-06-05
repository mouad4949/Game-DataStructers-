using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public Animator animator;
    public CoinManager cm;
    public float health;
    public float maxHealth;
    public Image healthBar;
    public float speed = 5f;
    public float coinDetectionRadius = 5f; // Distance de détection des pièces

    Vector2 movementInput;
    Dictionary<GameObject, Vector3> nearbyCoinsDict = new Dictionary<GameObject, Vector3>();
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxHealth = health;
    }

    private void Update()
    {
        HandleMovement();
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);

        if (Input.GetKeyDown(KeyCode.Space)) // Détection de la touche "Entrée"
        {
            CollectNearbyCoins();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleMovement()
    {
        float mx = Input.GetAxisRaw("Horizontal");
        float my = Input.GetAxisRaw("Vertical");
        movementInput = new Vector2(mx, my).normalized;
        animator.SetFloat("Horizontal", movementInput.x);
        animator.SetFloat("Vertical", movementInput.y);
        animator.SetFloat("Speed", movementInput.sqrMagnitude);
    }

    private void MovePlayer()
    {
        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coins"))
        {   
            cm.coinCount++;
            Destroy(other.gameObject);
            if (nearbyCoinsDict.ContainsKey(other.gameObject))
            {
                nearbyCoinsDict.Remove(other.gameObject);
            }
        }
    }

    private List<GameObject> GetCoinsInRadius()
    {
        List<GameObject> nearbyCoins = new List<GameObject>();
        foreach (var coin in cm.coins)
        {
            if (coin != null)
            {
                if (nearbyCoinsDict.ContainsKey(coin))
                {
                    nearbyCoins.Add(coin);
                }
                else
                {
                    float distance = Vector3.Distance(transform.position, coin.transform.position);
                    if (distance <= coinDetectionRadius)
                    {
                        nearbyCoins.Add(coin);
                        nearbyCoinsDict[coin] = coin.transform.position;
                    }
                }
            }
        }
        return nearbyCoins;
    }

    private void CollectNearbyCoins()
    {
        List<GameObject> nearbyCoins = GetCoinsInRadius();
        if (nearbyCoins.Count > 0)
        {
            Debug.Log("Found " + nearbyCoins.Count + " coins within detection radius.");
            StartCoroutine(MoveToCollectCoins(nearbyCoins));
        }
        else
        {
            Debug.Log("No coins found within detection radius.");
        }
    }

    private IEnumerator MoveToCollectCoins(List<GameObject> coins)
    {
        foreach (var coin in coins)
        {
            if (coin != null)
            {
                while (coin != null && Vector3.Distance(transform.position, coin.transform.position) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, coin.transform.position, speed * Time.deltaTime);
                    yield return null;
                }

                if (coin != null)
                {
                    cm.CollectCoin(coin);
                    nearbyCoinsDict.Remove(coin);
                    Debug.Log("Collected coin at " + coin.transform.position);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, coinDetectionRadius);
    }
}
