using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour
{
    // Method to handle try again button click
    public void TryAgainButtonClicked()
    {
        // Reload the current scene (restart the game)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Stop all objects' movement and sound
        Time.timeScale = 1f; // Pause the game
        AudioListener.pause = false; // Pause all audio
    }

    //// Method to handle back to school button click
    //public void BackToSchoolButtonClicked()
    //{
    //    // Load the main menu scene or any other desired scene
    //    SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your scene name
    //}

    // Method to handle quit button click
    public void QuitButtonClicked()
    {
        // Quit the application
        Application.Quit();
    }
}