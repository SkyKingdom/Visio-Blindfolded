using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensActivation : MonoBehaviour
{
    public static ScreensActivation instance; // Singleton instance

    public GameObject winScreenCanvas;
    public GameObject loseScreenCanvas;

    //This is just for now in the main project we can assign the audio source to the object 
    public AudioSource winSound;
    public AudioSource loseSound;

    private bool winSoundPlayed = false;
    private bool loseSoundPlayed = false;
    void Awake()
    {
        if (instance == null)
        {
            instance = this; // Set the singleton instance
        }
        else
        {
            Destroy(gameObject); // Destroy any additional instances
        }
    }

    public void ActivateLoseScreen() //Referenced in CarController Script
    {
        if (!loseSoundPlayed)
        {
            //Show the lose screen canvas
            loseScreenCanvas.SetActive(true);

            // Play the lose sound
            loseSound.Play();
            loseSoundPlayed = true;

            StartCoroutine(WaitForLoseSound());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Win Condition
        if (other.CompareTag("FinishZone"))
        {
            if (!winSoundPlayed)
            {
                // Show the win screen canvas
                winScreenCanvas.SetActive(true);
                // Play the win sound
                winSound.Play();
                winSoundPlayed = true;

                StartCoroutine(WaitForWinSound());
            }
        }
    }

    IEnumerator WaitForWinSound()
    {
        yield return new WaitForSeconds(winSound.clip.length);

        // Stop all objects' movement and sound
        Time.timeScale = 0f; // Pause the game
        AudioListener.pause = true; // Pause all audio
        StopAllCoroutines(); // Stop any ongoing coroutines. Actually I don't need this I think it just protective measurments
    }

    IEnumerator WaitForLoseSound()
    {
        yield return new WaitForSeconds(loseSound.clip.length);

        // Stop all objects' movement and sound
        Time.timeScale = 0f; // Pause the game
        AudioListener.pause = true; // Pause all audio
        StopAllCoroutines(); // Stop any ongoing coroutines. Actually I don't need this I think it just protective measurments
    }
}

