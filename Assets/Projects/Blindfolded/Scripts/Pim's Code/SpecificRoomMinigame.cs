using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificRoomMinigame : Minigame
{
    //Make some kind of room node
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
