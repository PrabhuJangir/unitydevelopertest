using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerText;           // UI Text element to display the timer
    public float timeLimit = 120f;   // Total time limit for the game in seconds
    private float timeRemaining;     // Remaining time in seconds
    public bool isGameOver = false;  // Flag to indicate if the game is over

    void Start()
    {
        // Initialize the remaining time with the time limit
        timeRemaining = timeLimit;
        // Update the timer UI at the start of the game
        UpdateTimerUI();
    }

    void Update()
    {
        // If the game is over, do not update the timer
        if (isGameOver) return;

        // Decrease the remaining time by the time elapsed since the last frame
        timeRemaining -= Time.deltaTime;

        // Check if the remaining time has reached zero
        if (timeRemaining <= 0)
        {
            // Set the remaining time to zero and end the game
            timeRemaining = 0;
            EndGame();
        }

        // Update the timer UI every frame
        UpdateTimerUI();
    }

    // Update the timer UI with the current remaining time
    void UpdateTimerUI()
    {
        // Calculate the minutes and seconds from the remaining time
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Format the time as MM:SS and update the UI text element
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // End the game when the timer reaches zero
    public void EndGame()
    {
        // Set the game over flag to true
        isGameOver = true;

        // Log a message to the console
        Debug.Log("Game Over!");

        // Add any additional game over logic here (e.g., show a game over screen)
    }
}
