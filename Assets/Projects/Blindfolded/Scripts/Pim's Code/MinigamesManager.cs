using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamesManager : Manager
{
    //List to track which minigames have already been played.
    private List<int> alreadyPlayed = new List<int>();
    
    public override void Start()
    {
        //Can test the alreadyPlayed list here.
    }

    //Update for the minigame that is currently running
    public override void Update()
    {
        if (gameManager.currentMinigame != null)
        {
            gameManager.currentMinigame.CurrentlyRunning();
        }
    }

    //Picks a random minigame and takes into account if it already played the minigame.
    public void PickRandom()
    {
        int random = RollTillValid();
        Minigame randomGame = gameManager.GetMinigamesList()[random];
        gameManager.currentMinigame = randomGame;
        alreadyPlayed.Add(random);
        gameManager.currentMinigame.EntryPoint();
    }

    //Pick a minigame by name
    public void PickByName(string name) 
    {
        for (int i = 0; i < gameManager.GetMinigamesList().Count; i++)
        {
            Debug.LogError("Minigames array: " + gameManager.GetMinigamesList()[i].minigameName);
            Debug.LogError("Minigame name: " + name);
            if (gameManager.GetMinigamesList()[i] != null)
            {
                if (gameManager.GetMinigamesList()[i].minigameName == name)
                {
                    gameManager.currentMinigame = gameManager.GetMinigamesList()[i];
                    alreadyPlayed.Add(i);
                    gameManager.currentMinigame.EntryPoint();
                }
            }
            else
            {
                Debug.LogError("There is a minigame returning null, checking for more");
            }
          
        }

        if (gameManager.currentMinigame == null)
        {
            Debug.LogError("Minigame with the given name does not exist");
        }
    }

    //Disables the currently running minigame
    public void DisableMinigame()
    {
        gameManager.currentMinigame.ResetGame();
        ClearList();
        gameManager.currentMinigame = null;
    }

    //Rolls a minigame till it finds one that has not been done already.
    public int RollTillValid()
    {
        int random = Random.Range(0, gameManager.GetMinigamesList().Count);
        if (alreadyPlayed.Count > 0)
        {
            for (int i = 0; i < alreadyPlayed.Count; i++)
            {
                if (alreadyPlayed[i] == random)
                {
                    RollTillValid();
                    break;
                }
            }
            return random;
        }
        else
        {
            return 0;
        }
       
    }

    /// <summary>
    /// Clears the minigames list
    /// </summary>
    public void ClearList() 
    {
        alreadyPlayed.Clear();
    }


}
