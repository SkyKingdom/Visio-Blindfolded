using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AudioManager : Manager
{
    //All of the audio is stored in this list for easy access
    public List<GameObject> audioSources = new List<GameObject>();

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
        LayerMask mask = 2;
        collider.transform.gameObject.layer = mask;
        
        // SteamAudioSource steamAudio = source.AddComponent<SteamAudioSource>();
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1;
        //Min Distance is the same as it was before i started this project.
        audioSource.minDistance = 11.16278f;
        audioSource.maxDistance = 500f;
        audioSource.name = name;
        source.name = name;
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
            collider.isTrigger = true;
        }
        else
        {
            audioSource.loop = false;
            Object.Destroy(source, audioSource.clip.length);

        }
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSources.Add(source);
        if (fromLocation != null)
        {
            source.transform.position = fromLocation;
        }
       
    }


    public void RemoveFromlist(GameObject source) 
    {
        if (IsInList(source.GetComponent<AudioSource>().name))
        {
            audioSources.Remove(source);
        }
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
                    audioSources[i].GetComponent<AudioSource>().Stop();
                }
            }
        }
    }

    public GameObject getAudioByName(string name) 
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i] != null)
            {
                if (audioSources[i].name == name)
                {
                    return audioSources[i];
                }
            }
        }
        return null;
    }

    public bool IsInList(string name) 
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i] != null)
            {
                if (audioSources[i].name == name)
                {
                    return true;
                }
            }
        }
        return false;
    }


    public void ClearAllSounds()
    {
        Debug.Log("Cleared all sounds");
        for (int i = 0; i < audioSources.Count; i++)
        {
            Object.Destroy(audioSources[i]);
        }
        audioSources.Clear();
    }
}
