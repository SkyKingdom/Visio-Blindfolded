using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSoundFinderManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Nodes;

    [SerializeField] AudioSource SoundObj;
    [SerializeField] AudioSource VictorySoundObj;

    // Start is called before the first frame update
    void Start()
    {
        ResetMinigame();
    }

    public void ResetMinigame()
    {
        VictorySoundObj.Play();
        SoundObj.transform.position = Nodes[Random.Range(0, Nodes.Count)].transform.position;
    }
}
