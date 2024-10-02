using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public float detectionRadius = 5f;  // Radius within which the zombie detects the player
    public float attackRange = 1f;      // Range within which the zombie attacks the player
    public float moveSpeed = 2f;        // Speed at which the zombie moves towards the player
    public int health = 3;              // Health of the zombie
    public Transform player;            // Reference to the player
    public Animator animator;           // Animator for the zombie animations

    public GameObject[] bloodSplatterPrefabs; // Array to hold blood splatter prefabs
    public Transform bloodParent;             // Parent object to hold blood splatters in the hierarchy

    private bool isDead = false;        // Track whether the zombie is dead
    private bool isAttacking = false;   // Track if the zombie is attacking
    private Rigidbody2D rb;
    private Collider2D zombieCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        zombieCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isDead)
            return;

        // Check the distance between the zombie and the player
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            // If the player is within attack range, attack
            StartCoroutine(Attack());
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            // If the player is within detection range but not within attack range, move towards the player
            MoveTowardsPlayer();
        }
        else
        {
            // Stop moving if the player is out of range
            animator.SetBool("isMoving", false);
            rb.velocity = Vector2.zero;
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Move the zombie towards the player
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

        // Play moving animation
        animator.SetBool("isMoving", true);

        // Rotate to face the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        // Play attack animation
        animator.SetTrigger("attack");

        // Wait for the attack animation to play out
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Check if the player is still in attack range
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceToPlayer <= attackRange)
        {
            // Optionally: Deal damage to the player (add your player health/damage system here)
            Debug.Log("Zombie attacks player!");
        }

        // Allow the zombie to attack again after a cooldown (if needed)
        yield return new WaitForSeconds(1f);  // You can adjust this cooldown time

        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        health -= damage;

        // Spawn blood splatter when hit
        SpawnBloodSplatter();

        if (health <= 0)
        {
            Die();
        }
    }

    void SpawnBloodSplatter()
    {
        // Randomly select a blood splatter prefab
        GameObject bloodSplatter = bloodSplatterPrefabs[Random.Range(0, bloodSplatterPrefabs.Length)];

        // Instantiate the blood splatter at the zombie's position and parent it to "bloodParent" to keep the hierarchy clean
        Instantiate(bloodSplatter, transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)), bloodParent);
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        // Stop movement
        rb.velocity = Vector2.zero;
        zombieCollider.enabled = false;

        // Create a final blood splatter that remains on the ground
        SpawnBloodSplatter();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle what happens when the zombie collides with something (optional)
    }
}
