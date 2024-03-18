using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Music : MonoBehaviour
{ 
    public GameObject closeRoom; //collider to close the room after minigame starts
    public GameObject[] instruments;

    private Dictionary<GameObject, AudioClip> instrumentSounds = new Dictionary<GameObject, AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < instruments.Length; i++)
        {
            // Add instrument GameObject and its respective sound to the dictionary
            instrumentSounds.Add(instruments[i], instruments[i].GetComponent<AudioSource>().clip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMusicMinigame()
    {
        closeRoom.SetActive(true); //Close the exit

        // Check if there are instruments in the list
        if (instruments.Length > 0)
        {
            // Choose a random instrument
            GameObject randomInstrument = instruments[Random.Range(0, instruments.Length)];

            // Play the sound associated with the selected instrument
            PlaySound(randomInstrument);
        }
        else
        {
            Debug.LogError("No instruments found in the list!");
        }
    }

    void PlaySound(GameObject instrument)
    {
        // Check if the instrument is in the dictionary
        if (instrumentSounds.ContainsKey(instrument))
        {
            // Get the audio clip associated with the instrument
            AudioClip audioClip = instrumentSounds[instrument];

            // Play the audio clip
            AudioSource.PlayClipAtPoint(audioClip, instrument.transform.position);
        }
        else
        {
            Debug.LogWarning("No sound found for the instrument: " + instrument.name);
        }
    }
}
