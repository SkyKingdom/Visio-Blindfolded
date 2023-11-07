using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeManager : MonoBehaviour
{
    [CustomEditor(typeof(NodeManager))]
    public class NodeManagerEditor : Editor
    {
        private bool errorHandlingFoldout = true;

        public override void OnInspectorGUI()
        {
            NodeManager script = (NodeManager)target;
            errorHandlingFoldout = EditorGUILayout.Foldout(errorHandlingFoldout, "Error Handling");

            if (errorHandlingFoldout)
            {
                EditorGUI.indentLevel++;

                script.RandomSelectedMasterNode = EditorGUILayout.IntField("RandomSlectedMasterNode", script.RandomSelectedMasterNode);
                EditorGUILayout.LabelField("The startup has failed: ", $"{script.SelectingNodeFailCheck} out of {1} tasks");
                EditorGUILayout.LabelField("Story has Spawned", $"{script.masterCurrentStoryLength} out of {script.masterNodeStoryLength}");

                EditorGUI.indentLevel--;
            }
            DrawDefaultInspector();
        }
    }

    [Header("Connecting nodes")]
    public List<ScriptableObject> NodePrompts = new List<ScriptableObject>();
    private int NodePromtListLength;

    [Header("Story")]
    public int StoryProgressionNumber;
    public bool StoryHasReachedWaypoint;
    public string CurrentObjectiveNodeString;

    public ScriptableObject SelectedNodeStory;
    public List<ScriptableObject> ThisStoryNodeList = new List<ScriptableObject>();

    private int RandomSelectedMasterNode;
    private MasterNodeSO masternodeSO;

    private int SelectingNodeFailCheck;
    private int masterNodeStoryLength;
    private int masterCurrentStoryLength;

    void Start()
    {
        SelectingNodeStory();
        StoryProgressionBookmark();
    }
    private void Update()
    {
        StoryProgressionBookmark(); 
    }

    private void SelectingNodeStory()
    {
        if (RandomSelectedMasterNode == 0)
        {
            NodePromtListLength = NodePrompts.Count;
            RandomSelectedMasterNode = Random.Range(0, NodePromtListLength);
            SelectedNodeStory = NodePrompts[RandomSelectedMasterNode];
            BuildStory(SelectedNodeStory);
        }
        else if (SelectingNodeFailCheck == 0)
        {
            SelectingNodeFailCheck++;
            Debug.LogError("Thrown Exception: Level failed to spawn task, number of tasks: " + NodePromtListLength);
            NodePromtListLength = NodePrompts.Count;
            RandomSelectedMasterNode = Random.Range(0, NodePromtListLength);
            SelectedNodeStory = NodePrompts[RandomSelectedMasterNode];
            SelectingNodeStory();
        }
        else
        {
            Debug.LogError("Fata/l Error: Level failed to spawn task.");
            return;
        }

    }

    private void BuildStory(ScriptableObject currentNode)
    {
        if (currentNode == null)
        {
            return;
        }

        if (currentNode is MasterNodeSO)
        {
            MasterNodeSO masterNodeSO = (MasterNodeSO)currentNode;
            masterNodeStoryLength = (masterNodeSO.StoryLength + 1);
            ThisStoryNodeList.Add(masterNodeSO);
            List<ScriptableObject> connectingNodes = masterNodeSO.ConntectingNodes;

            if (ThisStoryNodeList.Count < masterNodeStoryLength)
            {
                int randomConnectingNodeIndex = Random.Range(0, connectingNodes.Count);
                ScriptableObject nextNode = connectingNodes[randomConnectingNodeIndex];
                ThisStoryNodeList.Add(nextNode);

                BuildStory(nextNode);
            }
            else if (ThisStoryNodeList.Count == masterNodeStoryLength)
            {
                ThisStoryNodeList.Add(masterNodeSO.ConclusionNode);
                masterCurrentStoryLength = masterNodeStoryLength;
                BuildStory(null);
            }
        }
        else if (currentNode is ObjectiveNodeSO)
        {
            ObjectiveNodeSO objectiveNodeSO = (ObjectiveNodeSO)currentNode;
            List<ScriptableObject> connectingNodes = objectiveNodeSO.ConntectingNodes;

            if (ThisStoryNodeList.Count < masterNodeStoryLength)
            {
                int randomConnectingNodeIndex = Random.Range(0, connectingNodes.Count);
                ScriptableObject nextNode = connectingNodes[randomConnectingNodeIndex];
                ThisStoryNodeList.Add(nextNode);

                BuildStory(nextNode);
            }
            else if (ThisStoryNodeList.Count == masterNodeStoryLength)
            {
                ThisStoryNodeList.Add(objectiveNodeSO.LeadConcludingNode);
                masterCurrentStoryLength = masterNodeStoryLength;
                BuildStory(null);
            }
        }
    }

    public void StoryProgressionBookmark()
    {
        if (StoryHasReachedWaypoint && StoryProgressionNumber != masterCurrentStoryLength + 1)
        {
            SelectedNodeStory = ThisStoryNodeList[StoryProgressionNumber];

            CurrentObjectiveNodeString = string.Empty;

            switch (SelectedNodeStory)
            {
                case MasterNodeSO masterNode:
                    CurrentObjectiveNodeString = masterNode.LocationNode;
                    break;
                case ObjectiveNodeSO objectiveNode:
                    CurrentObjectiveNodeString = objectiveNode.LocationNode;
                    break;
                case ConclusionNodeSO conclusionNode:
                    CurrentObjectiveNodeString = conclusionNode.LocationNode;
                    break;
                default:
                    Debug.Log("Failed Fataly.");
                    break;
            }

            StoryProgressionNumber++;
            StoryHasReachedWaypoint = !StoryHasReachedWaypoint;
            Debug.Log(StoryProgressionNumber + "Out of" + masterNodeStoryLength + 1);
        }
        if (StoryHasReachedWaypoint && StoryProgressionNumber == masterNodeStoryLength + 1)
        {
            StoryProgressionNumber++;
            StoryHasReachedWaypoint = !StoryHasReachedWaypoint;
            Debug.LogError("Story has ended.");
        }
    }
}
