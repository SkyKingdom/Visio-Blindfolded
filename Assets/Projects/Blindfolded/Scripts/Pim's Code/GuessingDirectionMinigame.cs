using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessingDirectionMinigame : Minigame
{
    private bool isRunning = false;
    private bool inIt = false;
       
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
                print("gets here 1");
                GameManager.GetManager<AudioManager>().PlaySound("tickingtimer", locationToFind.position, true, 50);
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
            int random = Random.Range(0, locationsToFind.Length);
            locationToFind = locationsToFind[random];
        }
    }
}
