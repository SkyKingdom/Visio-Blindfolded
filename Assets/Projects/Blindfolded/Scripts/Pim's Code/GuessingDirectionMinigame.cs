using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;

public class GuessingDirectionMinigame : Minigame
{

    [SerializeField] private string[] sounds;
    public LayerMask canHit;
    private Timer timer;
    [SerializeField] private Timer waitingTimer;
    [SerializeField] GameObject[] UIButtons;
    private Canvas UICanvas;
    private bool isLeft;
    private bool isRight;
    private bool isFront;
    private bool isBack;
    private bool UIFrontPressed = false, UIBackPressed = false, UILeftPressed = false, UIRightPressed = false;
    private bool canGuess = false;
    private bool isPlayingAudio = false;
    [SerializeField] private LineRenderer line;

    private bool isPlayingVoicePrompt = false;



    GuessingDirectionMinigame()
    {
        timer = new Timer();
        waitingTimer = new Timer();
    }

    public override void EntryPoint()
    {
        //Put audio for the beginning here.
        float explainingAudio = GameManager.GetManager<AudioManager>().PlaySound("directionexplain", new Vector3(GameManager.instance.player.transform.position.x
            , GameManager.instance.player.transform.position.y + 2
            , GameManager.instance.player.transform.position.z)
            , false, 100);
        StartCoroutine(GenericVoicePrompt(explainingAudio));
        waitingTimer.SetTimer(explainingAudio + 1);
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
        float soundTime = GameManager.GetManager<AudioManager>().PlaySound(sounds[randomSound], locationToFind.position, false, randomVolume);
        //Add voice prompt
        print("Sounds length: " + GameManager.GetManager<AudioManager>().audioSources.Count);
        if (GameManager.GetManager<AudioManager>().IsInList(sounds[randomSound]))
        {
            timer.SetTimer(soundTime);
        }

    }

    //Konstantin' Code
    public void DirectionRayCast()
    {
        Vector3 directionToPlayer = GameManager.instance.player.transform.position - locationToFind.position;
        RaycastHit hit;

        //Add a LayerMask to detect the player better.
        if (Physics.Raycast(locationToFind.position, directionToPlayer, out hit, 100, canHit))
        {

            Debug.LogError(hit.collider.transform.name);
            if (hit.collider.transform == GameManager.instance.player.transform)
            {

                //// Determine if the player is on the left, right, front, or back side
                Vector3 objectForward = transform.forward;
                float dotProductForward = Vector3.Dot(objectForward, directionToPlayer);
                Vector3 objectRight = transform.right;
                float dotProductRight = Vector3.Dot(objectRight, directionToPlayer);


                // Define the threshold angle for considering the player in front, behind, left or right
                float angleThreshold = Mathf.Cos(Mathf.Deg2Rad * 30); // 30 degrees threshold

                if (dotProductForward > angleThreshold) //(dotProduct > 0)
                {
                    isFront = false;
                    isBack = true;
                    isLeft = false;
                    isRight = false;
                    Debug.Log("Player is in front of the object./The sound came from behind");
                    //Debug.Log("Player is on the right side of the object./The sound came from the left.");
                }
                else if (dotProductForward < -angleThreshold)
                {
                    isFront = true;
                    isBack = false;
                    isLeft = false;
                    isRight = false;
                    Debug.Log("Player is behind the object./The sound came from in front");
                    //Debug.Log("Player is on the left side of the object./The sound came from the right.");
                }
                else if (dotProductRight > angleThreshold)
                {
                    isFront = false;
                    isBack = false;
                    isLeft = true;
                    isRight = false;
                    Debug.Log("Player is on the right side of the object./ The sound came from the left.");
                    //Debug.Log("Player is directly in front or behind of the object.");
                }
                else if (dotProductRight < -angleThreshold)
                {
                    isFront = false;
                    isBack = false;
                    isLeft = false;
                    isRight = true;
                    Debug.Log("Player is on the left side of the object./ The sound came from the right");
                }
                PlayVoicePrompt();
            }
        }
    }

    private void PlayerInput()
    {
        if (canGuess == true)
        {
            foreach (GameObject item in UIButtons)
            {
                item.SetActive(true);
            }
            line.gameObject.SetActive(true);
            //Get OVRInput
            if (!isPlayingVoicePrompt && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
                !isPlayingVoicePrompt && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) ||
                !isPlayingVoicePrompt && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) ||
                !isPlayingVoicePrompt && OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
            {
                if (isLeft && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
                    isRight && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) ||
                    isFront && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) ||
                    isBack && OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
                {
                    float soundTime = GameManager.GetManager<AudioManager>().PlaySound("voiceguessright", new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), false, 100);
                    Debug.Log("Correct guess!"); //add voice prompts for feedback
                    currentScore++;
                    canGuess = false;
                    if (currentScore >= maxScore)
                    {
                        GameManager.GetManager<AudioManager>().PlaySound("directionend", GameManager.instance.player.transform.position, false, 1f);
                        OnMinigameComplete.Invoke();
                        GameManager.GetManager<MinigamesManager>().DisableMinigame();
                    }
                    waitingTimer.SetTimer(soundTime);
                }
                else
                {
                    float soundTime = GameManager.GetManager<AudioManager>().PlaySound("voiceguesswrong", new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), false, 100);
                    Debug.Log("Incorrect guess.");
                    canGuess = false;
                    waitingTimer.SetTimer(soundTime);
                }
            }
        }
        else
        {

            foreach (GameObject item in UIButtons)
            {
                item.SetActive(false);
            }
            line.gameObject.SetActive(false);
        }
    }


    public void PlayVoicePrompt()
    {
        isPlayingVoicePrompt = true;

        StartCoroutine(WaitForVoicePromptToEnd());

    }

    IEnumerator WaitForVoicePromptToEnd()
    {
        float soundTime = GameManager.GetManager<AudioManager>().PlaySound("voicestart", new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), false, 100);
        yield return new WaitForSeconds(soundTime); // Wait for the voice prompt to finish playing
        isPlayingVoicePrompt = false;
        canGuess = true;
    }



    IEnumerator GenericVoicePrompt(float time)
    {
        isPlayingVoicePrompt = true;
        yield return new WaitForSecondsRealtime(time);
        isPlayingVoicePrompt = false;
    }


    public override void Reset()
    {
        base.Reset();
        isLeft = false;
        isRight = false;
        isFront = false;
        isBack = false;
        GameManager.GetManager<AudioManager>().ClearAllSounds();
        canGuess = false;
        isPlayingAudio = false;
        isPlayingVoicePrompt = false;
        timer.StopTimer();
        waitingTimer.StopTimer();
    }



    public void HandleUIButton(int _value)
    {
        if (canGuess)
        {
            if (!isPlayingVoicePrompt)
            {
                switch (_value)
                {
                    case (int)Side.front:

                        UIFrontPressed = true;

                        break;
                    case (int)Side.back:

                        UIBackPressed = true;

                        break;
                    case (int)Side.left:

                        UILeftPressed = true;

                        break;
                    case (int)Side.right:

                        UIRightPressed = true;

                        break;
                    default:
                        break;
                }



                if (UIRightPressed && isRight ||
                    UILeftPressed && isLeft ||
                    UIFrontPressed && isFront ||
                    UIBackPressed && isBack)
                {
                    float soundTime = GameManager.GetManager<AudioManager>().PlaySound("voiceguessright", new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), false, 100);
                    Debug.Log("Correct guess!"); //add voice prompts for feedback
                    currentScore++;
                    canGuess = false;
                    if (currentScore >= maxScore)
                    {
                        GameManager.GetManager<AudioManager>().PlaySound("directionend", GameManager.instance.player.transform.position, false, 1f);
                        OnMinigameComplete.Invoke();
                        GameManager.GetManager<MinigamesManager>().DisableMinigame();
                    }
                    waitingTimer.SetTimer(soundTime);
                }
                else
                {
                    float soundTime = GameManager.GetManager<AudioManager>().PlaySound("voiceguesswrong", new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), false, 100);
                    Debug.Log("Incorrect guess.");
                    canGuess = false;
                    waitingTimer.SetTimer(soundTime);
                }
                UIRightPressed = false;
                UILeftPressed = false;
                UIFrontPressed = false;
                UIBackPressed = false;


            }


        }


    }

    public enum Side
    {
        front,
        back,
        left,
        right
    }

}