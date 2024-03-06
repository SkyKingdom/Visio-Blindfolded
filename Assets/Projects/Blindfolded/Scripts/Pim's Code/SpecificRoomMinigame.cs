using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificRoomMinigame : Minigame
{

    private bool isRunning;
    private bool inIt;
    public Transform[] rooms;






    public override void EntryPoint()
    {
        //Randomize a room 
    }


    public override void CurrentlyRunning()
    {
        isRunning = true;
    }


    public override void RelocateToNode()
    {
        if (isRunning) 
        {
          if (inIt) 
            {
            
            
            }
            else
            {
                inIt = true;
            }
        }

    }

}
