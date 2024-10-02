using UnityEngine;

public class SpotlightController : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public Camera mainCamera; // Reference to the main camera

    void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;  // Ensure the Z axis is 0, as it's a 2D game

        // Calculate the direction from the player to the mouse
        Vector3 direction = mousePos - player.position;

        // Calculate the angle to rotate the light
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the light
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90
            ));
    }
}
