using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GuessingDirectionMinigame : Minigame
{
    private bool isRunning = false;
    private bool inIt = false;
   [SerializeField] private string[] sounds;

    public override void EntryPoint()
    {
        RelocateToNode();
    }

    public override void CurrentlyRunning()
    {
        isRunning = true;
        Debug.Log("Currently Running Minigame:" + gameObject.name);
    }

    public override void RelocateToNode()
    {
        if (isRunning)
        {
            print("gets here 0");
            if (inIt)
            {
                int randomSound = UnityEngine.Random.Range(0, sounds.Length);
               
                print("gets here 1");
                GameManager.GetManager<AudioManager>().PlaySound(sounds[randomSound], locationToFind.position, true, 50);
                currentScore++;
            }
            else
            {
                inIt = true;
            }

            if (currentScore >= maxScore)
            {
                OnMinigameComplete.Invoke();
                GameManager.GetManager<MinigamesManager>().DisableMinigame();
            }
            int random = UnityEngine.Random.Range(0, locationsToFind.Length);
            locationToFind = locationsToFind[random];
        }
    }
}