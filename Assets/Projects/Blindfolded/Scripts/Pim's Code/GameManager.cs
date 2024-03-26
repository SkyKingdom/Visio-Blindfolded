using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private static Manager[] managers;

    [Header("-----------------------------------------------------------------------")]
    [Header("Player Reference")]
    [Space(5)]
    public GameObject player;
    [Header("-----------------------------------------------------------------------")]
    [Header("Minigame Manager Variables")]
    [Space(5)]
    public Minigame[] minigames;
    public Minigame currentMinigame;
    public List<GameObject> nodes;
    public bool startOnAwake;
    //Maybe something like a node group (to randomize nodes in a certain area.

    [Header("-----------------------------------------------------------------------")]
    [Header("Seed Manager Variables")]
    [Space(5)]
    public int Seed;
    public System.Random LevelSeed;
    public bool AutoGenerateSeed;

    [Header("-----------------------------------------------------------------------")]
    [Header("Nodes Manager Variables")]
    [Space(5)]
    [Header("Connecting nodes")]
    public List<ScriptableObject> StoryPrompts = new List<ScriptableObject>();
    [HideInInspector] public int NodePromtListLength;

    [Header("Story")]
    public int StoryProgressionNumber;
    public bool StoryHasReachedWaypoint;
    public string CurrentObjectiveNodeString;

    [Header("Story Tasks")]
    public ScriptableObject CurrentStoryNodeTask;
    public ScriptableObject CurrentStoryEndNode;
    public List<ScriptableObject> ThisStoryTaskList = new List<ScriptableObject>();

    public int RandomSelectedMasterNode;
    public MasterNodeSO masternodeSO;

    public int SelectingNodeFailCheck;
    public int masterNodeStoryLength;
    public int masterCurrentStoryLength;
#if UNITY_EDITOR
    [CustomEditor(typeof(GameManager))]
    public class NodeManagerEditor : Editor
    {
        private bool errorHandlingFoldout = true;

        public override void OnInspectorGUI()
        {
            errorHandlingFoldout = EditorGUILayout.Foldout(errorHandlingFoldout, "Error Handling");

            if (errorHandlingFoldout)
            {
                EditorGUI.indentLevel++;

                instance.RandomSelectedMasterNode = EditorGUILayout.IntField("RandomSlectedMasterNode", instance.RandomSelectedMasterNode);
                EditorGUILayout.LabelField("The startup has failed: ", $"{instance.SelectingNodeFailCheck} out of {1} tasks");
                EditorGUILayout.LabelField("Story has Spawned", $"{instance.masterCurrentStoryLength + 1} out of {instance.masterNodeStoryLength + 1}");

                EditorGUI.indentLevel--;
            }
            DrawDefaultInspector();
        }
    }
#endif


    GameManager()
    {
        instance = this;

        managers = new Manager[]
        {
          new MinigamesManager(),
          new AudioManager(),
          new SeedManager(),
          new NodeManager(),
        };
    }

    public void OutputAudioSources() 
    {
        for (int i = 0; i < GetManager<AudioManager>().audioSources.Count; i++)
        {
            if (GetManager<AudioManager>().audioSources[i]!= null)
            {
                print(GetManager<AudioManager>().audioSources[i].ToString() + " | AudioSource active");
            }
        }

        print(GetManager<AudioManager>().audioSources.Count + "| Audiosources count");
    
    }

    public static T GetManager<T>() where T : Manager
    {
        for (int i = 0; i < managers.Length; i++)
        {
            if (typeof(T) == managers[i].GetType())
            {
                return (T)managers[i];
            }
        }
        return default;
    }

    // Start is called before the first frame update
    public void Start()
    {
        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Start();
        }
        if (startOnAwake)
        {
            StartMiniGame();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Update();
        }
        OutputAudioSources();
    }

    public void StartMiniGame()
    {
        GetManager<MinigamesManager>().PickRandom();
    }
}
