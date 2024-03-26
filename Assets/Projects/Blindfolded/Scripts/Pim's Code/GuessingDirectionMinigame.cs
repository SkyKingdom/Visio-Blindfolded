using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GuessingDirectionMinigame : Minigame
{

    [SerializeField] private string[] sounds;
    private Timer timer;
    private Timer waitingTimer;

    public bool isLeft;
    public bool isRight;

    public bool canGuess = false;

    public bool isPlayingVoicePrompt = false;

   

    GuessingDirectionMinigame()
    {
        timer = new Timer();
        waitingTimer = new Timer();
        
    }

    public override void EntryPoint()
    {
        RelocateToNode();
    }

    public override void CurrentlyRunning()
    {

        Debug.Log("Currently Running Minigame:" + gameObject.name);

        if (timer.isActive && timer.TimerDone())
        {
            DirectionRayCast();
            timer.StopTimer();
        }

        if (waitingTimer.isActive && waitingTimer.TimerDone())
        {
            RelocateToNode();
            waitingTimer.StopTimer();
        }
        PlayerInput();

    }

    public override void RelocateToNode()
    {
        GameManager.GetManager<AudioManager>().ClearAllSounds();
        int randomSound = UnityEngine.Random.Range(0, sounds.Length);
        int randomVolume = UnityEngine.Random.Range(50, 100);
        print("gets here IsRunning");
        int random = UnityEngine.Random.Range(0, locationsToFind.Length);
        locationToFind = locationsToFind[random];
        GameManager.GetManager<AudioManager>().PlaySound(sounds[randomSound], locationToFind.position, false, randomVolume);
        //Add voice prompt
        print("Sounds length: " + GameManager.GetManager<AudioManager>().audioSources.Count);
        if (GameManager.GetManager<AudioManager>().IsInList(sounds[randomSound]))
        {
            timer.SetTimer(GameManager.GetManager<AudioManager>().getAudioByName(sounds[randomSound]).GetComponent<AudioSource>().clip.length);
        }

    }

    //Konstantin' Code
    public void DirectionRayCast()
    {
        Vector3 directionToPlayer = GameManager.instance.player.transform.position - locationToFind.position;
        RaycastHit hit;

        if (Physics.Raycast(locationToFind.position, directionToPlayer, out hit))
        {
            Debug.LogError(hit.collider.transform.name);
            if (hit.collider.transform == GameManager.instance.player.transform)
            {
                Vector3 objectRight = locationToFind.right;
                float dotProduct = Vector3.Dot(objectRight, directionToPlayer);

                if (dotProduct > 0)
                {
                    isLeft = true;
                    isRight = false;
                    Debug.Log("Player is on the right side of the object./The sound came from the left.");
                }
                else if (dotProduct < 0)
                {
                    isLeft = false;
                    isRight = true;
                    Debug.Log("Player is on the left side of the object./The sound came from the right.");
                }
                else
                {
                    isLeft = false;
                    isRight = false;
                    Debug.Log("Player is directly in front or behind of the object.");
                }
                PlayVoicePrompt();
            }
        }
    }

    private void PlayerInput()
    {
        if (canGuess == true)
        {
            //Get OVRInput
            if (!isPlayingVoicePrompt && (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                if (isLeft && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || (isRight && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)))
                {
                    GameManager.GetManager<AudioManager>().PlaySound("voiceguessright", new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), false, 100);
                    Debug.Log("Correct guess!"); //add voice prompts for feedback
                    currentScore++;
                    canGuess = false;
                    if (currentScore >= maxScore)
                    {
                        OnMinigameComplete.Invoke();
                        GameManager.GetManager<MinigamesManager>().DisableMinigame();
                    }
                    waitingTimer.SetTimer(3);
                }
                else
                {
                    GameManager.GetManager<AudioManager>().PlaySound("voiceguesswrong", new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), false, 100);
                    Debug.Log("Incorrect guess.");
                    canGuess = false;
                    waitingTimer.SetTimer(3);

                }
            }
        }
    }


    public void PlayVoicePrompt()
    {
        isPlayingVoicePrompt = true;

        StartCoroutine(WaitForVoicePromptToEnd());

    }

    IEnumerator WaitForVoicePromptToEnd()
    {
        GameManager.GetManager<AudioManager>().PlaySound("voicestart", new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), false, 100);
      
        float waitingTime = 0;
        if (GameManager.GetManager<AudioManager>().IsInList("voicestart"))
        {
            GameObject audioObject = GameManager.GetManager<AudioManager>().getAudioByName("voicestart");
            waitingTime = audioObject.GetComponent<AudioSource>().clip.length;
        }
        else 
        {
            waitingTime = 1f;
        }
        yield return new WaitForSeconds(waitingTime); // Wait for the voice prompt to finish playing
        isPlayingVoicePrompt = false;
        canGuess = true;
    }




}