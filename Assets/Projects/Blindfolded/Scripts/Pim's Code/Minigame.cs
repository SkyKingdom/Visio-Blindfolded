using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Minigame : MonoBehaviour
{
    public UnityEvent OnMinigameComplete;

    protected bool isRunning = false;
    protected bool inIt = false;

    public int currentScore;
    public int maxScore;
    public string nodeLocation;
    public Transform locationToFind;
    public Transform[] locationsToFind;


    public virtual void EntryPoint() { }


    public virtual void CurrentlyRunning() { }


    public virtual void RelocateToNode() { }

}
