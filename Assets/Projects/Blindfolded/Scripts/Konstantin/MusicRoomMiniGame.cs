using System.Collections;
using UnityEngine;

public class MusicRoomMiniGame : MonoBehaviour
{
    public OVRPlayerController playerController;

    public GameObject[] instruments; // Array of instrument game objects
    public AudioClip[] instrumentSounds; // Array of instrument sounds

    private bool gameStarted = false;
    private int correctAnswers = 0;
    private int totalRounds = 5;
    private bool isGameOver = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !gameStarted && !isGameOver)
        {
            gameStarted = true;
            StartCoroutine(StartMiniGame());
        }
    }

    IEnumerator StartMiniGame()
    {
        while (correctAnswers < totalRounds)
        {
            // Choose a random instrument
            int randomIndex = Random.Range(0, instruments.Length);
            GameObject chosenInstrument = instruments[randomIndex];

            // Play a random sound from the chosen instrument
            AudioSource audioSource = chosenInstrument.GetComponent<AudioSource>();
            audioSource.clip = instrumentSounds[Random.Range(0, instrumentSounds.Length)];
            audioSource.Play();

            // Wait for the sound to finish playing
            yield return new WaitForSeconds(audioSource.clip.length);

            // Prompt the player to guess the direction
            // For simplicity, assume left and right are determined by player's position relative to the instrument
            string prompt = "Click the left controller if you think the sound came from the left or click the right controller if you think it came from the right";
            // Display prompt in VR UI

            // Wait for player input
            while (!Input.GetButtonDown("LeftTrigger") && !Input.GetButtonDown("RightTrigger"))
            {
                yield return null;
            }

            // Check player's guess
            bool isCorrect = CheckAnswer(chosenInstrument.transform.position);
            if (isCorrect)
            {
                correctAnswers++;
                Debug.Log("Correct!");
            }
            else
            {
                Debug.Log("Incorrect! Try again.");
            }

            // Reset player's position and rotation
            // For simplicity, assume teleporting the player back to the center of the room

            // Wait before starting the next round
            yield return new WaitForSeconds(1f);
        }

        // Game Over
        Debug.Log("Congratulations! You won!");
        isGameOver = true;
    }

    bool CheckAnswer(Vector3 instrumentPosition)
    {
        // For simplicity, assume left and right are determined by player's position relative to the instrument
        Vector3 playerPosition = playerController.transform.position;
        if (playerPosition.x < instrumentPosition.x)
        {
            // Player is on the left side of the instrument
            return Input.GetButtonDown("LeftTrigger");
        }
        else
        {
            // Player is on the right side of the instrument
            return Input.GetButtonDown("RightTrigger");
        }
    }
}
