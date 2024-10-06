using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Transform feet;
    public Transform top;
    public Text ammoCounter;
    public GameObject bulletPrefab; // Bullet prefab reference
    public Transform firePoint; // Empty GameObject attached to the gun's position
    public float bulletSpeed = 10f;
    public int maxAmmo = 15;
    public float reloadTime = 0f;
    private int currentAmmo;
    private bool isReloading = false;
    Vector2 mousePos;
    private int rifleAmmo = 120;
    private bool rifle = false;
    private bool pistol = true;

    public AudioSource audioSource;
    public AudioClip reloadSound;
    public AudioClip fireSound;

    
    // Weapon-related fields
    public Transform pistolFirePoint; // FirePoint for pistol
    public Transform rifleFirePoint;  // FirePoint for rifle
    private enum Weapon { Pistol, Rifle };
    private Weapon currentWeapon = Weapon.Pistol;

    public Animator legsAnimator;
    public Animator topAnimator;
    public int health =100;
    void Start()
    {
        currentAmmo = maxAmmo;
        ammoCounter.text = currentAmmo + "/15";
        firePoint = pistolFirePoint;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)){
            Reload();
        }
            

        // Weapon switching logic
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            pistol = true;
            rifle = false;
            topAnimator.SetBool("pistol", true);
            
            firePoint = pistolFirePoint; // Set firePoint to pistolFirePoint
            Debug.Log("Switched to Pistol");

        }
       

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            pistol = false;
            rifle = true;
            firePoint = rifleFirePoint; // Set firePoint to rifleFirePoint
            topAnimator.SetBool("rifle", true);
            currentAmmo = 30;
            ammoCounter.text = currentAmmo + "/" + rifleAmmo;
            Debug.Log("Switched to Rifle");
        }

        // Get the mouse position
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Set animation states based on movement input
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        legsAnimator.SetBool("isMoving", isMoving);
        topAnimator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
    if (rifle)
    {
        currentAmmo--;
        ammoCounter.text = currentAmmo + "/" + rifleAmmo;
        topAnimator.SetTrigger("shooting");
    }
    else if (pistol)
    {
        audioSource.clip = fireSound;
        audioSource.Play();
        currentAmmo--;
        ammoCounter.text = currentAmmo + "/15";
        topAnimator.SetTrigger("shooting");
    }
    

    // Instantiate and shoot the bullet from the current firePoint
    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
    
    // Set the bullet's velocity based on the firePoint's forward direction
    bulletRb.velocity = firePoint.right * bulletSpeed; // Use firePoint's right direction for bullet travel

    if (currentAmmo <= 0)
    {
        Reload();
    }
}


    void Reload()
    {
        topAnimator.SetTrigger("reload");
        Debug.Log("Reloading...");
        
        if (rifle == true)
        {
            rifleAmmo -= (30 - currentAmmo);
            currentAmmo = 30;
            ammoCounter.text = currentAmmo + "/" + rifleAmmo;
        }
        if (pistol == true)
        {
            audioSource.clip = reloadSound;
            audioSource.Play();
            currentAmmo = maxAmmo;  // Refill ammo
            ammoCounter.text = currentAmmo + "/15";
        }
    }


    void FixedUpdate()
    {
        // Calculate the direction towards the mouse
        Vector2 direction = mousePos - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the player's top and feet towards the mouse
        top.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        feet.rotation = top.rotation;

        // Define movement direction based on keys pressed
        Vector2 moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            // Move forward (towards the mouse)
            moveDirection += direction.normalized;
        }

        if (Input.GetKey(KeyCode.S))
        {
            // Move backward (away from the mouse)
            moveDirection -= direction.normalized;
        }

        if (Input.GetKey(KeyCode.A))
        {
            // Move left (strafe left, perpendicular to the mouse direction)
            Vector2 leftPerpendicular = new Vector2(-direction.y, direction.x).normalized;
            moveDirection += leftPerpendicular;
        }

        if (Input.GetKey(KeyCode.D))
        {
            // Move right (strafe right, perpendicular to the mouse direction)
            Vector2 rightPerpendicular = new Vector2(direction.y, -direction.x).normalized;
            moveDirection += rightPerpendicular;
        }

        // Move the player based on the combined movement direction
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}
