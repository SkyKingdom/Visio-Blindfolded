using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameFindObjectV2 : MinigameBase
{

    [SerializeField] GameObject ObjectToFind;

    [SerializeField] List<GameObject> SpawnPoints;

    [SerializeField] AudioSource SoundObj;
    [SerializeField] AudioSource VictorySoundObj;

    bool Init = false;

    // Start is called before the first frame update
    void Start()
    {
        ResetMinigame();
    }

    public void ResetMinigame()
    {
        VictorySoundObj.transform.position = ObjectToFind.transform.position;
        if (Init)
        {
            VictorySoundObj.Play();
        }else
        {
            Init = true;
        }

        ObjectToFind.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Count)].transform.position;
        ObjectToFind.SetActive(true);

    }
}
