using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : MonoBehaviour
{
    //Groups of audio stored in the library
    public AudioGroup[] audioGroups;

    /// <summary>
    /// Get the clip from the library by name, if there are multiple clips in a group it will pick a random sound.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AudioClip GetClip(string name)
    {
        if (audioGroups != null)
        {
            for (int i = 0; i < audioGroups.Length; i++)
            {
                if (audioGroups[i].name == name)
                {
                    AudioGroup group = audioGroups[i];
                    return group.clips[Random.Range(0, group.clips.Length)];
                }
            }
        }
        return null;
    }

    //Serializable so you can fill it in in the inspector.
    [System.Serializable]
    public struct AudioGroup
    {
        public string name;
        public AudioClip[] clips;
    }
}
