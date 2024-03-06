using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeManager : Manager
{
    public override void Start()
    {
        SelectingNodeStory();
        StoryProgressionBookmark();
    }
    public override void Update()
    {
        StoryProgressionBookmark();
    }

    private void SelectingNodeStory()
    {
        if (gameManager.RandomSelectedMasterNode == 0)
        {
            gameManager.NodePromtListLength = gameManager.StoryPrompts.Count;
            gameManager.RandomSelectedMasterNode = gameManager.LevelSeed.Next(0, gameManager.NodePromtListLength);
            gameManager.CurrentStoryNodeTask = gameManager.StoryPrompts[gameManager.RandomSelectedMasterNode];
            BuildStory(gameManager.CurrentStoryNodeTask);
        }
        else if (gameManager.SelectingNodeFailCheck == 0)
        {
            gameManager.SelectingNodeFailCheck++;
            Debug.LogError("Thrown Exception: Level failed to spawn task, number of tasks: " + gameManager.NodePromtListLength);
            gameManager.NodePromtListLength = gameManager.StoryPrompts.Count;
            gameManager.RandomSelectedMasterNode = gameManager.LevelSeed.Next(0, gameManager.NodePromtListLength);
            gameManager.CurrentStoryNodeTask = gameManager.StoryPrompts[gameManager.RandomSelectedMasterNode];
            SelectingNodeStory();
        }
        else
        {
            Debug.LogError("Fatal Error: Level failed to spawn task.");
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
            gameManager.masterNodeStoryLength = (masterNodeSO.StoryLength + 1);
            gameManager.ThisStoryTaskList.Add(masterNodeSO);
            gameManager.CurrentStoryEndNode = masterNodeSO.ConclusionNode;
            List<ScriptableObject> connectingNodes = masterNodeSO.ConntectingNodes;

            if (gameManager.ThisStoryTaskList.Count < gameManager.masterNodeStoryLength)
            {
                int randomConnectingNodeIndex = gameManager.LevelSeed.Next(0, connectingNodes.Count);
                ScriptableObject nextNode = connectingNodes[randomConnectingNodeIndex];
                gameManager.ThisStoryTaskList.Add(nextNode);

                BuildStory(nextNode);
            }
            else if (gameManager.ThisStoryTaskList.Count == gameManager.masterNodeStoryLength)
            {
                gameManager.ThisStoryTaskList.Add(masterNodeSO.ConclusionNode);
                gameManager.masterCurrentStoryLength = gameManager.masterNodeStoryLength;
                BuildStory(null);
            }
        }
        else if (currentNode is ObjectiveNodeSO)
        {
            ObjectiveNodeSO objectiveNodeSO = (ObjectiveNodeSO)currentNode;
            List<ScriptableObject> connectingNodes = objectiveNodeSO.ConntectingNodes;

            if (gameManager.ThisStoryTaskList.Count < gameManager.masterNodeStoryLength)
            {
                int randomConnectingNodeIndex = gameManager.LevelSeed.Next(0, connectingNodes.Count);
                ScriptableObject nextNode = connectingNodes[randomConnectingNodeIndex];
                gameManager.ThisStoryTaskList.Add(nextNode);

                BuildStory(nextNode);
            }
            else if (gameManager.ThisStoryTaskList.Count == gameManager.masterNodeStoryLength)
            {
                gameManager.ThisStoryTaskList.Add(gameManager.CurrentStoryEndNode);
                gameManager.masterCurrentStoryLength = gameManager.masterNodeStoryLength;
                BuildStory(null);
            }
        }
    }

    public void StoryProgressionBookmark()
    {
        if (gameManager.StoryHasReachedWaypoint && gameManager.StoryProgressionNumber != gameManager.masterCurrentStoryLength + 1)
        {
            gameManager.CurrentStoryNodeTask = gameManager.ThisStoryTaskList[gameManager.StoryProgressionNumber];

            gameManager.CurrentObjectiveNodeString = string.Empty;

            switch (gameManager.CurrentStoryNodeTask)
            {
                case MasterNodeSO masterNode:
                    gameManager.CurrentObjectiveNodeString = masterNode.LocationNode;
                    break;
                case ObjectiveNodeSO objectiveNode:
                    gameManager.CurrentObjectiveNodeString = objectiveNode.LocationNode;
                    if (objectiveNode.hasMinigame)
                    {
                        GameManager.GetManager<MinigamesManager>().PickRandom();
                    }
                    //Add minigames setup here?
                    break;
                case ConclusionNodeSO conclusionNode:
                    gameManager.CurrentObjectiveNodeString = conclusionNode.LocationNode;
                    break;
                default:
                    Debug.Log("Failed Fataly.");
                    break;
            }

            gameManager.StoryProgressionNumber++;
            gameManager.StoryHasReachedWaypoint = !gameManager.StoryHasReachedWaypoint;
            Debug.Log(gameManager.StoryProgressionNumber + "Out of" + gameManager.masterNodeStoryLength + 1);
        }
        if (gameManager.StoryHasReachedWaypoint && gameManager.StoryProgressionNumber == gameManager.masterNodeStoryLength + 1)
        {
            gameManager.StoryProgressionNumber++;
            gameManager.StoryHasReachedWaypoint = !gameManager.StoryHasReachedWaypoint;
            Debug.LogError("Story has ended.");
        }
    }
}
