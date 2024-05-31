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

    Vector2 movementInput;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxHealth = health;
    }

    private void Update()
    {
        HandleMovement();
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth , 0 ,1);
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
        }
    }
}
