using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameDetectCollision : MonoBehaviour
{
    /// <summary>
    /// When the player triggers the collider it will relocate the node in a minigame
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Relocate per minigame
            GameManager.instance.currentMinigame.RelocateToNode();

            GameManager.GetManager<AudioManager>().ClearAllSounds();
            //Disable Sound per minigame
            Destroy(gameObject);
        }
    }
}
