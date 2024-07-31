using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager script to notify when a cube is collected

    // This method is called when another collider enters the trigger collider attached to the GameObject
    void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered the trigger has the tag "Player"
        if (other.gameObject.tag == "Collect")
        {
            // Notify the GameManager that a cube has been collected
            gameManager.CollectCube();

            // Destroy the cube GameObject to simulate it being collected
            Destroy(other.gameObject);
        }
    }
}
