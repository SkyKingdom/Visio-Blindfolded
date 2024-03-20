using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GuessingDirectionMinigame : Minigame
{
   
    [SerializeField] private string[] sounds;
    private Timer timer;

    GuessingDirectionMinigame() 
    {
        timer = new Timer();
    }

    public override void EntryPoint()
    {
        RelocateToNode();
    }

    public override void CurrentlyRunning()
    {
        isRunning = true;
        Debug.Log("Currently Running Minigame:" + gameObject.name);

        if (timer.isActive && timer.TimerDone())
        {
            DirectionRayCast();
            timer.StopTimer();
        }

    }

    public override void RelocateToNode()
    {
        if (isRunning)
        {
            int randomSound = UnityEngine.Random.Range(0, sounds.Length);
            int randomVolume = UnityEngine.Random.Range(50, 100);
            GameManager.GetManager<AudioManager>().ClearAllSounds();
            print("gets here IsRunning");
            if (inIt)
            {
                print("gets here Init true");
                currentScore++;
            }
            else
            {
                inIt = true;
                RelocateToNode();
                print("gets here Init false");
            }

            if (currentScore >= maxScore)
            {
                OnMinigameComplete.Invoke();
                GameManager.GetManager<MinigamesManager>().DisableMinigame();
            }
            int random = UnityEngine.Random.Range(0, locationsToFind.Length);
            locationToFind = locationsToFind[random];
            GameManager.GetManager<AudioManager>().PlaySound(sounds[randomSound], locationToFind.position, true, randomVolume);
            print("Sounds length: " + GameManager.GetManager<AudioManager>().audioSources.Count);
            if (GameManager.GetManager<AudioManager>().IsInList(sounds[randomSound]))
            {
                timer.SetTimer(GameManager.GetManager<AudioManager>().getAudioByName(sounds[randomSound]).GetComponent<AudioSource>().clip.length);
            }
        }
    }

    //Konstantin' Code
    public void DirectionRayCast()
    {
        Vector3 directionToPlayer = GameManager.instance.player.transform.position - locationToFind.position;
        RaycastHit hit;

        if (Physics.Raycast(locationToFind.position, directionToPlayer, out hit))
        {
            if (hit.collider.transform == GameManager.instance.player.transform)
            {
                Vector3 objectRight = locationToFind.right;
                float dotProduct = Vector3.Dot(objectRight, directionToPlayer);

                if (dotProduct > 0)
                {
                    Debug.Log("Player is on the right side of the object./The sound came from the left.");
                }
                else if (dotProduct < 0)
                {
                    Debug.Log("Player is on the left side of the object./The sound came from the right.");
                }
                else
                {
                    Debug.Log("Player is directly in front or behind of the object.");
                }
            }
        }
    }
    


}