using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistController : MonoBehaviour
{
    public int maxHealth = 2;  // Max health (scientist takes two shots to die)
    private int currentHealth;
    
    public float waddleSpeed = 1.0f;  // Speed of waddling
    public float panicSpeed = 3.0f;  // Speed when in panic
    public GameObject legs;
    public Animator topAnimator;  // Animator for the top part (for working and panic)
    public Animator bottomAnimator;  // Animator for the bottom part (for waddling and running)
    public GameObject bloodDecalPrefab;
    private bool isPanic = false;  // Is the scientist in panic mode?
    private bool isDead = false;  // Is the scientist dead?

    private Rigidbody2D rb;  // Rigidbody for movement

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        // Start with waddling animation
        SetWorkingMode();
    }

    void Update()
    {
        if (isDead)
        {
            // If dead, don't do anything
            return;
        }

        if (isPanic)
        {
            RunAroundInPanic();
        }
        else
        {
            WaddleAround();
        }
    }

    // Function to handle waddling around
    void WaddleAround()
    {
        // Simple random movement to simulate waddling
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        rb.velocity = randomDirection * waddleSpeed;

        // Update the bottom part to waddle animation
        topAnimator.SetBool("true", true);
        bottomAnimator.SetBool("isWalking", true);
    }

    // Function to handle the panic state
    void RunAroundInPanic()
    {
        // Simulate random running movement in panic
        Vector2 randomPanicDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        rb.velocity = randomPanicDirection * panicSpeed;

        // Update the top and bottom part to panic/run animation
        topAnimator.SetBool("isRunning", true);
        bottomAnimator.SetBool("isRunning", true);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                GetShot();
            }

        }
    }
    // Function to handle getting shot
    public void GetShot()
    {
        currentHealth--;
        topAnimator.SetTrigger("hit");

        if (currentHealth > 0)
        {
            // Enter panic mode after being shot
            EnterPanicMode();
        }
        else
        {
            // If health reaches 0, the scientist dies
            Die();
        }
    }

    // Enter panic mode
    void EnterPanicMode()
    {
        isPanic = true;
        topAnimator.SetBool("isWorking", false);
        bottomAnimator.SetBool("isWalking", false);
    }

    // Set the scientist to be working and waddling
    void SetWorkingMode()
    {
        topAnimator.SetBool("isWorking", true);
        bottomAnimator.SetBool("isWalking", true);
    }

    // Handle scientist death
    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;  // Stop movement
        Destroy(legs);
        // Trigger death animation (if available)
        topAnimator.SetTrigger("isDead");
        Instantiate(bloodDecalPrefab, transform.position, Quaternion.identity);
        this.enabled = false;
        // Disable further interaction (optional)
        GetComponent<Collider2D>().enabled = false;
    
    }
}
