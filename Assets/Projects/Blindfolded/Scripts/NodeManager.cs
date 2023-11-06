using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [Header("Connecting nodes")]
    public List<ScriptableObject> NodePrompts = new List<ScriptableObject>();
    private int NodePromtListLength;
    public int RandomSelectedNodeNumber;
    public ScriptableObject SelectedNodeStory;

    private int SelectingNodeFailCheck;
    // Start is called before the first frame update
    void Start()
    {
        SelectingNodeStory();
    }

    private void SelectingNodeStory()
    {
        if (RandomSelectedNodeNumber == 0)
        {
            NodePromtListLength = NodePrompts.Count;
            RandomSelectedNodeNumber = Random.Range(0, NodePromtListLength);
            SelectedNodeStory = NodePrompts[RandomSelectedNodeNumber];
        }
        else if (SelectingNodeFailCheck == 0)
        {
            SelectingNodeFailCheck++;
            Debug.LogError("Thrown Exception: Level failed to spawn task, number of tasks: " + NodePromtListLength);
            NodePromtListLength = NodePrompts.Count;
            RandomSelectedNodeNumber = Random.Range(0, NodePromtListLength);
            SelectedNodeStory = NodePrompts[RandomSelectedNodeNumber];
            SelectingNodeStory();
        }
        else
        {
            Debug.LogError("Fatal Error: Level failed to spawn task.");
            return;
        }

    }
}
