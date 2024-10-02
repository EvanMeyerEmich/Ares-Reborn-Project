using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;  // Assign damage value for the bullet

    void Start()
    {
        // You can add more bullet-related behavior here if needed
        Destroy(gameObject, 5f);  // Optional: Destroy bullet after 5 seconds to avoid memory leaks
    }
}
