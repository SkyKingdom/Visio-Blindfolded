using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoorLogic : MonoBehaviour
{
    [Header("AnimatorHandler")]
    public Animator DoorAnimation;

  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DoorAnimation.SetBool("PlayerEntersSlideDoorCollider", true);
            GameManager.GetManager<AudioManager>().PlaySound("open_door", gameObject.transform.position, false, 100);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DoorAnimation.SetBool("PlayerEntersSlideDoorCollider", false);
            
        }
    }
}
