using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameObjectFindCollectable : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<MinigameBase>().OnMinigameComplete.Invoke();
            //gameObject.SetActive(false);
        }
    }
}
