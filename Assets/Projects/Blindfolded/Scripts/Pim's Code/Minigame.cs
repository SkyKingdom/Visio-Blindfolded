using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Minigame : MonoBehaviour
{
    public UnityEvent OnMinigameComplete;
    protected bool isRunning = false;
    protected bool inIt = false;
    [Tooltip("TaskVerifier that launches this minigame should be here as a reference")]
    public TaskVerifier node;
    public int currentScore;
    public int maxScore;
    public string nodeLocation;
    public Transform locationToFind;
    public Transform[] locationsToFind;
    public string minigameName;

   
    /// <summary>
    /// This is called whenever the minigame is initizalized.
    /// </summary>
    public virtual void EntryPoint() { }


    /// <summary>
    /// Works as a update method.
    /// </summary>
    public virtual void CurrentlyRunning() { }



    /// <summary>
    /// Action based method, this is being called whenever  a certain action or criteria is met.
    /// </summary>
    public virtual void RelocateToNode() { }

    /// <summary>
    /// Resets the game.
    /// </summary>
    public virtual void ResetGame()
    {
        inIt = false;
        isRunning = false;
        currentScore = 0;
        locationToFind = null;
        node.isTriggerd = false;
    }

}
