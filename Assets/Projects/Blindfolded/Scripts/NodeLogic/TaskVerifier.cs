using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TaskVerifier : MonoBehaviour
{
    public string Task_Number;
    public NodeManager nodeManager;
    [SerializeField] bool RequireNodeContact = true;
    [SerializeField] bool RequireEventComplete = false;

    [HideInInspector]public bool NodeContacted = false;
    [HideInInspector]public bool EventCompleted = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NodeContacted = true;
            if (!RequireEventComplete || RequireEventComplete && EventCompleted)
            {
                CompleteTask();
            }
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
