using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private bool isGamePaused = false; // Flag to track if the game is paused
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoseGame();
        }
    }

    public void LoseGame()
    {
        // Pause the game
        Time.timeScale = 0f; // Set time scale to 0 to pause everything

        // Display the lose screen
        // You can show a UI canvas with the lose screen here
    }

    //public void ResumeGame()
    //{
    //    // Unpause the game
    //    Time.timeScale = 1f; // Set time scale back to 1 to resume everything

    //    // Hide the lose screen
    //    // You can hide the UI canvas with the lose screen here

    //    // Optional: Resume any paused game mechanics or sounds
    //    // For example:
    //    // carController.enabled = true; // Enable car movement again
    //    // environmentAudioSource.UnPause(); // Resume environment sounds
    //}
}
