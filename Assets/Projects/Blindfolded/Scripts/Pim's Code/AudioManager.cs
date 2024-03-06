using SteamAudio;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : Manager
{
    //All of the audio is stored in this list for easy access
    public List<AudioSource> audioSources = new List<AudioSource>();

    /// <summary>
    /// Plays a sound based on location given and has logic for multiple sound types. 
    /// Volume is in percentage. The shouldLoop is for sound that has to stay and be interacted with.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fromLocation"></param>
    /// <param name="shouldLoop"></param>
    public void PlaySound(string name, Vector3 fromLocation, bool shouldLoop, float volume)
    {
        GameObject source = new();
        //Seperate into methods
        AudioSource audioSource = source.AddComponent<AudioSource>();
        SphereCollider collider = source.AddComponent<SphereCollider>();
        SteamAudioSource steamAudio = source.AddComponent<SteamAudioSource>();
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1;
        //Min Distance is the same as it was before i started this project.
        audioSource.minDistance = 11.16278f;
        audioSource.maxDistance = 500f;
        audioSource.name = name;
        float volumeValue = volume / 100;
        audioSource.volume = volumeValue;
        audioSource.clip = gameManager.GetComponent<AudioLibrary>().GetClip(name);
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }


        if (shouldLoop)
        {
            audioSource.loop = true;
            source.AddComponent<MinigameDetectCollision>();
        }
        else
        {
            audioSource.loop = false;
            Object.Destroy(source, audioSource.clip.length);
        }
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
       
       
        audioSources.Add(audioSource);
        source.transform.position = fromLocation;
    }

    /// <summary>
    /// Stops playing the given sound
    /// </summary>
    /// <param name="name"></param>
    public void StopPlaying(string name)
    {
        if (audioSources.Count > 0)
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (audioSources[i].name == name)
                {
                    audioSources[i].Stop();
                }
            }
        }
    }

    public void ClearAllSounds()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            Object.Destroy(audioSources[i]);
        }
        audioSources.Clear();
    }
}
