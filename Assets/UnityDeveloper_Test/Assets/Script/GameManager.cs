using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameTimer gameTimer;     // Reference to the GameTimer script to manage game time
    public int totalCubes = 5;      // Total number of cubes that need to be collected to win
    private int cubesCollected = 0; // Counter for the number of cubes collected by the player

    void Start()
    {
        // Ensure the gameTimer reference is set, either from the Inspector or by finding the component
        if (gameTimer == null)
        {
            gameTimer = GetComponent<GameTimer>();
        }
    }

    // Method to be called when the player collects a cube
    public void CollectCube()
    {
        // Increment the count of collected cubes
        cubesCollected++;

        // Check if the player has collected enough cubes to win the game
        if (cubesCollected >= totalCubes)
        {
            WinGame();
        }
    }

    // Method to handle the logic for winning the game
    void WinGame()
    {
        // Log a message to the console indicating the player has won
        Debug.Log("You Win!");

        // Stop the game timer to indicate the game is over
        if (gameTimer != null)
        {
            gameTimer.isGameOver = true;
        }

        // Add any additional win logic here (e.g., display a win screen, play a sound, etc.)
    }
}
