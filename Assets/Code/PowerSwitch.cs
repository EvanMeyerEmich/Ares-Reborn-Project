using UnityEngine;
using UnityEngine.Rendering.Universal;  // For 2D Lights

public class PowerSwitch : MonoBehaviour
{
    public Light2D globalLight;
    public GameObject eIcon;
    public Transform player;
    public float interactDistance = 3f;
    public bool isPoweredOn = true;

    void Start()
    {
        eIcon.SetActive(false);
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer < interactDistance)
        {
            eIcon.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                TogglePower();
            }
        }
        else
        {
            eIcon.SetActive(false);
        }
    }

    void TogglePower()
    {
        isPoweredOn = !isPoweredOn;
        globalLight.enabled = isPoweredOn;  // Toggle the 2D light
    }
}
