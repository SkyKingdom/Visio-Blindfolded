using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskVerifier : MonoBehaviour
{
    public string Task_Number;
    public NodeManager nodeManager;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && nodeManager.StoryProgressionNumber != 7)
        {
            if (nodeManager.CurrentObjectiveNodeString == Task_Number)
            {
                Debug.Log("Hit");
                nodeManager.StoryHasReachedWaypoint = (true);
                nodeManager.StoryProgressionBookmark();
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
        if (nodeManager.StoryProgressionNumber != 7 && nodeManager.CurrentObjectiveNodeString == Task_Number)
        {
            Debug.Log("Hit");
            nodeManager.StoryHasReachedWaypoint = (true);
            nodeManager.StoryProgressionBookmark();
        }
    }
}
