using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;  // Import URP for 2D Light

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5f;
    public float attackRadius = 3f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3f;
    public float fireRate = 1f;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float health = 100f; 

    private float nextFireTime;
    private Transform topPart;
    private bool isChasing = false;

    public Animator legsAnimator;
    public Animator topAnimator;
    public GameObject legs;
    public UnityEngine.Rendering.Universal.Light2D visionConeLight;  // Light2D for vision cone

    private Vector2 randomPatrolDirection;
    private float directionChangeInterval = 3f;
    private float directionChangeTimer;

    public GameObject bloodDecalPrefab;

    void Start()
    {
        topPart = transform.Find("TopPart");
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

        if (health <= 0)
        {
            Die();
        }
        Debug.Log("hit health = " + health);
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            visionConeLight.intensity = 1.5f;  // Brighten the vision cone when detecting the player
            RotateTowardsPlayer();

            if (distanceToPlayer < attackRadius)
            {
                isChasing = true;
                topAnimator.SetBool("isShooting", true);
                legsAnimator.SetBool("isMoving", false);

                if (Time.time > nextFireTime)
                {
                    Shoot();
                    nextFireTime = Time.time + 1f / fireRate;
                }
            }
            else
            {
                topAnimator.SetBool("isShooting", false);
                legsAnimator.SetBool("isMoving", true);
                ChasePlayer();
            }
        }
        else
        {
            visionConeLight.intensity = 0.5f;  // Dim the vision cone when not detecting the player
            if (!isChasing)
            {
                legsAnimator.SetBool("isMoving", true);
                Patrol();
            }
            else
            {
                legsAnimator.SetBool("isMoving", false);
                isChasing = false;
            }
        }
    }

    void RotateTowardsPlayer()
    {
        Vector2 direction = (player.position - topPart.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    }

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

    void ChasePlayer()
    {
        Vector2 movementDirection = (player.position - transform.position).normalized;
        RotateLegsTowardsMovement(movementDirection);
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
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
