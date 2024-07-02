using JetBrains.Annotations;
using Oculus.Haptics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;

public class GuessingDirectionMinigame : Minigame
{

    public string[] sounds;
    public LayerMask canHit;
    private Timer timer;
    private Timer waitingTimer;
    public GameObject[] UIButtons;
    private Canvas UICanvas;
    private bool isLeft;
    private bool isRight;
    private bool isFront;
    private bool isBack;
    private bool UIFrontPressed = false, UIBackPressed = false, UILeftPressed = false, UIRightPressed = false;
    private bool canGuess = false;
    private bool isPlayingAudio = false;
    public LineRenderer line;
    public OVRPlayerController controllerReference;
    public EventSystem eventSystem;
    private bool onlyOnce = false;
    private bool isPlayingVoicePrompt = false;
    private bool executeOnce = false;


    /// <summary>
    /// Constructor of the minigame
    /// </summary>
    GuessingDirectionMinigame()
    {
        timer = new Timer();
        waitingTimer = new Timer();
    }

    
    /// <summary>
    /// Adds this minigame to the gamemanager
    /// </summary>
    public void Update()
    {
        if (!executeOnce )
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.ClearMinigames();
                GameManager.instance.AddMinigameToList(gameObject);
                for (int i = 0; i < GameManager.instance.GetMinigamesList().Count; i++)
                {
                    Debug.LogError(GameManager.instance.GetMinigamesList()[i].name);
                }
                executeOnce = true;
            }
        }
    }


    // <summary>
    // Called whenever the minigame is initialized.
    /// </summary>
    public override void EntryPoint()
    {
        //Put audio for the beginning here.
        float explainingAudio = GameManager.GetManager<AudioManager>().PlaySound("directionexplain", new Vector3(GameManager.instance.player.transform.position.x
            , GameManager.instance.player.transform.position.y + 2
            , GameManager.instance.player.transform.position.z)
            , false, 85);
        StartCoroutine(GenericVoicePrompt(explainingAudio));
        waitingTimer.SetTimer(explainingAudio + 1);
        controllerReference = FindAnyObjectByType<OVRPlayerController>().GetComponent<OVRPlayerController>();
    }



    /// <summary>
    /// Acts like a update method. Refer to the Minigame class and the minigames manager class.
    /// </summary>
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
            controllerReference.Acceleration = 0.1f;
            waitingTimer.StopTimer();
        }
        PlayerInput();
        OVRInput.Update();
        QuitGame();
    }


    /// <summary>
    /// Called whenever a certain action has been completed within the minigame.
    /// For example guessing the sound in this method.
    /// </summary>
    public override void RelocateToNode()
    {
        GameManager.GetManager<AudioManager>().ClearAllSounds();
        int randomSound = UnityEngine.Random.Range(0, sounds.Length);
        int randomVolume = UnityEngine.Random.Range(90, 100);
        print("gets here IsRunning");
        int random = UnityEngine.Random.Range(0, locationsToFind.Length);
        locationToFind = locationsToFind[random];
        float soundTime = GameManager.GetManager<AudioManager>().PlaySound(sounds[randomSound], locationToFind.position, false, randomVolume);
        //Add voice prompt
        print("Sounds length: " + GameManager.GetManager<AudioManager>().audioSources.Count);
        timer.SetTimer(soundTime);

    }

    //Konstantin' Code
    public void DirectionRayCast()
    {
        controllerReference.Acceleration = 0;
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

    /// <summary>
    /// When the player can choose an option this will handle the input.
    /// </summary>
    private void PlayerInput()
    {
        if (canGuess == true)
        {
            foreach (GameObject item in UIButtons)
            {
                item.SetActive(true);
            }
            line.gameObject.SetActive(true);
            if (!onlyOnce)
            {
                eventSystem.SetSelectedGameObject(UIButtons[0]);
                onlyOnce = true;
            }


            //Get OVRInput
            if (!isPlayingVoicePrompt && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) ||
                !isPlayingVoicePrompt && OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) ||
                !isPlayingVoicePrompt && OVRInput.GetDown(OVRInput.Button.Two) ||
                !isPlayingVoicePrompt && OVRInput.GetDown(OVRInput.Button.One))
            {
                if (isLeft && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) ||
                    isRight && OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) ||
                    isFront && OVRInput.GetDown(OVRInput.Button.Two) ||
                    isBack && OVRInput.GetDown(OVRInput.Button.One))
                {
                    float soundTime = GameManager.GetManager<AudioManager>().PlaySound("voiceguessright", new Vector3(GameManager.instance.player.transform.position.x,
                        GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), false, 85);
                    Debug.Log("Correct guess!"); //add voice prompts for feedback
                    currentScore++;
                    canGuess = false;
                    if (currentScore >= maxScore)
                    {
                        GameManager.GetManager<AudioManager>().PlaySound("directionend", GameManager.instance.player.transform.position, false, 85);
                        OnMinigameComplete.Invoke();
                        GameManager.GetManager<MinigamesManager>().DisableMinigame();
                    }
                    waitingTimer.SetTimer(soundTime);
                }
                else
                {
                    float soundTime = GameManager.GetManager<AudioManager>().PlaySound("voiceguesswrong", new Vector3(GameManager.instance.player.transform.position.x,
                        GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), false, 85);
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

    /// <summary>
    /// Method for exeuting the voice prompt
    /// </summary>
    public void PlayVoicePrompt()
    {
        isPlayingVoicePrompt = true;

        StartCoroutine(WaitForVoicePromptToEnd());

    }
    /// <summary>
    /// IEnumrator executing code after the first voice prompt is done.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForVoicePromptToEnd()
    {
        float soundTime = GameManager.GetManager<AudioManager>().PlaySound("voicestart", new Vector3(GameManager.instance.player.transform.position.x,
            GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), false, 85);
        yield return new WaitForSeconds(soundTime); // Wait for the voice prompt to finish playing
        isPlayingVoicePrompt = false;
        canGuess = true;
    }


    /// <summary>
    /// Method for waiting for any possible voice prompt.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator GenericVoicePrompt(float time)
    {
        isPlayingVoicePrompt = true;
        yield return new WaitForSecondsRealtime(time);
        isPlayingVoicePrompt = false;
    }

    /// <summary>
    /// Reset the game with this method.
    /// </summary>
    public override void ResetGame()
    {
        base.ResetGame();
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


    /// <summary>
    /// Method for handling optional UI input.
    /// </summary>
    /// <param name="_value"></param>
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
                    float soundTime = GameManager.GetManager<AudioManager>().PlaySound("voiceguessright", new Vector3(GameManager.instance.player.transform.position.x,
                        GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), false, 85);
                    Debug.Log("Correct guess!"); //add voice prompts for feedback
                    currentScore++;
                    canGuess = false;
                    if (currentScore >= maxScore)
                    {
                        GameManager.GetManager<AudioManager>().PlaySound("directionend", GameManager.instance.player.transform.position, false, 85);
                        OnMinigameComplete.Invoke();
                        GameManager.GetManager<MinigamesManager>().DisableMinigame();
                    }
                    waitingTimer.SetTimer(soundTime);
                }
                else
                {
                    float soundTime = GameManager.GetManager<AudioManager>().PlaySound("voiceguesswrong", new Vector3(GameManager.instance.player.transform.position.x,
                        GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), false, 85);
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

    /// <summary>
    /// Quits the application, does not work in the editor.
    /// </summary>
    public void QuitGame()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            controllerReference.enabled = true;
            GameManager.GetManager<MinigamesManager>().DisableMinigame();
        }

    }
    /// <summary>
    /// Enum for the sides.
    /// </summary>
    public enum Side
    {
        front,
        back,
        left,
        right
    }
}