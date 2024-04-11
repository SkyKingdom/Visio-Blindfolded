using System.Collections;
using UnityEngine;

public class MusicMinigame : MonoBehaviour
{
    public AudioClip[] instrumentSounds; // Array of instrument sounds
    public AudioSource audioSource; // AudioSource component to play the sounds

    private int correctGuesses = 0; // Number of correct guesses
    private bool gameActive = false; // Flag to indicate if the game is active

    void Start()
    {   
        StartCoroutine(PlaySoundRoutine());
    }

    IEnumerator PlaySoundRoutine()
    {
        while (true)
        {
            if (gameActive)
            {
                // Play a random instrument sound
                AudioClip randomSound = instrumentSounds[Random.Range(0, instrumentSounds.Length)];
                audioSource.PlayOneShot(randomSound);

                // Wait for a short duration before playing the next sound
                yield return new WaitForSeconds(3f);

                // After playing the sound, prompt the player for direction
                PromptForDirection();
            }
            yield return null;
        }
    }

    void PromptForDirection()
    {
        // Prompt the player for direction
        Debug.Log("Listen carefully to the sound. Was it from the left or the right?");

        // You can trigger feedback for the player here, like vibrating the controller

        // Logic to check the player's input and update correct guesses
        // For this example, let's assume the player inputs direction using buttons or gestures
        // You need to implement this according to your input method
    }

    // Method to be called when the player makes a correct guess
    public void CorrectGuess()
    {
        correctGuesses++;
        if (correctGuesses >= 5)
        {
            // Player has won the minigame
            Debug.Log("Congratulations! You've won the minigame!");
            gameActive = false;
        }
    }

    // Method to start the minigame
    public void StartMinigame()
    {
        correctGuesses = 0;
        gameActive = true;
    }

    // Method to end the minigame
    public void EndMinigame()
    {
        gameActive = false;
    }
}
