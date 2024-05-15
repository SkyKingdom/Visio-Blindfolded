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
        //Time.timeScale = 1f; // UnPause the game
        AudioListener.pause = false; // UnPause all audio
    }

    public void LoadNextLevel() //Has to be TESTED
    {
        // Get the index of the next scene in the build order
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Check if there's a scene at the next index
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // If there's no next scene, you can handle it accordingly
            Debug.LogWarning("No next scene available.");
        }
    }

    //// Method to handle back to school button click
    public void BackToSchoolButtonClicked()
    {
        // Load the main menu scene or any other desired scene
        GameManager.SceneLoader(Levels.levels.Main); // Replace "MainMenu" with your scene name
    }

    // Method to handle quit button click
    public void QuitButtonClicked()
    {
        // Quit the application
        Application.Quit();
    }
}