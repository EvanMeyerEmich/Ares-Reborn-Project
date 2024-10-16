using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistController : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float panicSpeed = 4f;
    public float health = 100f; 

    private bool isPanicking = false;
    private Vector2 randomPatrolDirection;
    private float directionChangeInterval = 3f;
    private float directionChangeTimer;

    public Animator legsAnimator;
    public Animator topAnimator;
    public GameObject legs;

    public GameObject bloodDecalPrefab;
    public AudioSource audioSource;
    public AudioClip shotSound;

    void Start()
    {
        PickRandomPatrolDirection();
        directionChangeTimer = directionChangeInterval;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(40);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        topAnimator.SetTrigger("hit");  // Trigger the hit animation
        audioSource.PlayOneShot(shotSound);  // Play the shot sound

        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Enter panic mode if not dead
            isPanicking = true;
        }
        Debug.Log("Hit, health = " + health);
    }

    void Update()
    {
        if (isPanicking)
        {
            Panic();
            return;  // Skip patrolling if panicking
        }

        Patrol();
    }

    void Panic()
    {
        // Run in random directions quickly while panicking
        transform.position += (Vector3)(randomPatrolDirection * panicSpeed * Time.deltaTime);
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            PickRandomPatrolDirection();
            directionChangeTimer = directionChangeInterval;
        }
        RotateLegsTowardsMovement(randomPatrolDirection);
        legsAnimator.SetBool("isMoving", true);
    }

    void RotateLegsTowardsMovement(Vector2 movementDirection)
    {
        if (movementDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void Patrol()
    {
        transform.position += (Vector3)(randomPatrolDirection * patrolSpeed * Time.deltaTime);
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            PickRandomPatrolDirection();
            directionChangeTimer = directionChangeInterval;
        }
        RotateLegsTowardsMovement(randomPatrolDirection);
        legsAnimator.SetBool("isMoving", true);
    }

    void PickRandomPatrolDirection()
    {
        randomPatrolDirection = Random.insideUnitCircle.normalized;
    }

    public void Die()
    {
        topAnimator.SetTrigger("dead");
        legsAnimator.SetBool("isMoving", false);
        Destroy(legs);
        Instantiate(bloodDecalPrefab, transform.position, Quaternion.identity);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
