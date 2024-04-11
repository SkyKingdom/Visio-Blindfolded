using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggercheck : MonoBehaviour
{
    public Music music;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter Detected!");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collision is detected");
            music.StartMusicMinigame();
        }
    }
}
