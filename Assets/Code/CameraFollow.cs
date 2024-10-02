using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public float smoothSpeed = 0.125f;  // How smoothly the camera follows the player
    public Vector3 offset;  // The offset distance between the camera and player

    public float minZoom = 5f;  // Minimum zoom distance
    public float maxZoom = 15f;  // Maximum zoom distance
    public float zoomSpeed = 2f;  // Speed of zooming

    public float rotationSpeed = 100f;  // Speed of rotation when pressing Q or E

    private void LateUpdate()
    {
        // Camera follows the player
        FollowPlayer();

        // Camera rotates when Q or E are pressed
        RotateCamera();

        // Camera zooms in/out with mouse wheel
        ZoomCamera();
    }

    void FollowPlayer()
    {
        // Define target position based on player position and offset
        Vector3 desiredPosition = player.position + offset;

        // Smoothly move the camera to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    void RotateCamera()
    {
        // Rotate camera around player when Q or E are pressed
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(player.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(player.position, Vector3.down, rotationSpeed * Time.deltaTime);
        }
    }

    void ZoomCamera()
    {
        // Get scroll input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Adjust offset's Z value to zoom in or out
        float desiredZoom = offset.z + scrollInput * zoomSpeed;

        // Clamp the zoom to make sure it's within the defined min and max zoom limits
        offset.z = Mathf.Clamp(desiredZoom, -maxZoom, -minZoom);
    }
}
