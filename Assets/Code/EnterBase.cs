using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterBase : MonoBehaviour
{
    public GameObject fIcon;
    public Transform player;
    public float interactDistance = 3f;
    public PowerSwitch PowerSwitch;

    // Start is called before the first frame update
    void Start()
    {
        fIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        if(PowerSwitch.power == true){
            if (distanceToPlayer < interactDistance)
            {
                fIcon.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
            else
            {
                fIcon.SetActive(false);
            }
        }
        
    }
}
