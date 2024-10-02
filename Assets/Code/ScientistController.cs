using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistController : MonoBehaviour
{
    public int maxHealth = 100;  // Max health (scientist takes two shots to die)
    private int currentHealth;
    
    public float waddleSpeed = 1.0f;  // Speed of waddling
    public float panicSpeed = 3.0f;  // Speed when in panic
    public GameObject legs;
    public GameObject topPart;
    public Animator topAnimator;  // Animator for the top part (for working and panic)
    public Animator bottomAnimator;  // Animator for the bottom part (for waddling and running)
    public GameObject bloodDecalPrefab;
    private bool isPanic = false;  // Is the scientist in panic mode?
    private bool isDead = false;  // Is the scientist dead?

    private Rigidbody2D rb;  // Rigidbody for movement

    void Start()
    {
        topPart = transform.Find("TopPart");
        PickRandomWaddleDirection();
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

        if (health <= 0)
        {
            Die();
        }
        Debug.Log("hit health = " + health);
    }

    void Update()
    {
        

    

    void RotateLegsTowardsMovement(Vector2 movementDirection)
    {
        if (movementDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = (player.position - firePoint.position).normalized * 10f;
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
        Destroy(visionConeLight);
        Instantiate(bloodDecalPrefab, transform.position, Quaternion.identity);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
