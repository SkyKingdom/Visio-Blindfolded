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

   

    public virtual void EntryPoint() { }


    public virtual void CurrentlyRunning() { }


    public virtual void RelocateToNode() { }

    public virtual void ResetGame()
    {
        inIt = false;
        isRunning = false;
        currentScore = 0;
        locationToFind = null;
        node.isTriggerd = false;
    }

}
