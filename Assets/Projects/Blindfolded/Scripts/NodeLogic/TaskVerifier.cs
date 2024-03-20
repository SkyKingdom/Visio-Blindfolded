using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskVerifier : MonoBehaviour
{
    public string Task_Number;
    [SerializeField] bool RequireNodeContact;
    [SerializeField] bool RequireEventCompleted;
    bool EventCompleted = false;
    bool NodeContacted = false;
    [SerializeField] bool isMinigame;
    [SerializeField] string roomName;
    [SerializeField] string minigameName;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NodeContacted = true;
            if (isMinigame)
            {
                GameManager.GetManager<MinigamesManager>().PickByName(minigameName);
            }
            else if (!RequireEventCompleted || RequireEventCompleted && EventCompleted)
            {
                CompleteTask();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NodeContacted = false;
        }
    }

    public void EventComplete()
    {
        EventCompleted = true;
        if (!RequireNodeContact || RequireNodeContact && NodeContacted)
        {
            CompleteTask();
        }
    }

    void CompleteTask()
    {
        if (GameManager.instance.StoryProgressionNumber != 7 && GameManager.instance.CurrentObjectiveNodeString == Task_Number)
        {
            Debug.Log("Hit");
            GameManager.instance.StoryHasReachedWaypoint = true;
            GameManager.GetManager<NodeManager>().StoryProgressionBookmark();
        }
    }
}
