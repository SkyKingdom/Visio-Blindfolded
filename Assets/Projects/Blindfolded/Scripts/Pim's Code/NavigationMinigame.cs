using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationMinigame : Minigame
{
    
    public override void CurrentlyRunning()
    {
       isRunning = true;
    }
    
    //Use this for setting up the minigame
    public override void EntryPoint() 
    {
        RelocateToNode();
    }

    public override void RelocateToNode() 
    {
        if (isRunning)
        {
            if (inIt)
            {
                GameManager.GetManager<AudioManager>().PlaySound("tickingtimer", locationToFind.position, true, 100);
                currentScore++;
            }
            else
            {
                inIt = true;
            }
            //Reposition sound to another location


            //Check if maxPoints are met and then completeminigame
            if (currentScore >= maxScore)
            {
                OnMinigameComplete.Invoke();
                GameManager.GetManager<MinigamesManager>().DisableMinigame();
            }
            locationToFind.position = GameManager.instance.nodes[Random.Range(0, GameManager.instance.nodes.Count)].transform.position;
        }
    }
}
