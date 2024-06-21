using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

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
    public List<Minigame> minigames = new();
    public Minigame currentMinigame;
    public List<GameObject> nodes;
    public bool startOnAwake;
    public AudioMixer mixer;
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


    public static void SceneLoader(Levels.levels level)
    {
        SceneManager.LoadScene((int)level);
    }


    /// <summary>
    /// First method being run when application launches.
    /// Setting instance of this class and making the manager instances.
    /// </summary>
    GameManager()
    {
        if (instance == null)
        {
            instance = this;
        }

        managers = new Manager[]
        {
          new MinigamesManager(),
          new AudioManager(),
          new SeedManager(),
          new NodeManager(),
        };
    }


    /// <summary>
    /// Debug method for checking audio sources.
    /// </summary>
    public void OutputAudioSources()
    {
        for (int i = 0; i < GetManager<AudioManager>().audioSources.Count; i++)
        {
            if (GetManager<AudioManager>().audioSources[i] != null)
            {
                print(GetManager<AudioManager>().audioSources[i].ToString() + " | AudioSource active");
            }
        }
        print(GetManager<AudioManager>().audioSources.Count + "| Audiosources count");
    }

    /// <summary>
    /// Get a managar in the manager list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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

    /// <summary>
    /// Awake, Called after constructor.
    /// </summary>
    public void Awake()
    {
        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Awake();
        }
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


        if (OVRInput.Get(OVRInput.Button.Start) && OVRInput.Get(OVRInput.Button.One))
        {
            RestartAndroid();
        }


    }

    /// <summary>
    /// Debug method for calling a minigame.
    /// </summary>
    public void StartMiniGame()
    {
        GetManager<MinigamesManager>().PickRandom();
    }

#if UNITY_ANDROID
    private static void RestartAndroid()
    {
        if (Application.isEditor) return;

        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            const int kIntent_FLAG_ACTIVITY_CLEAR_TASK = 0x00008000;
            const int kIntent_FLAG_ACTIVITY_NEW_TASK = 0x10000000;

            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var pm = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            var intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", Application.identifier);

            intent.Call<AndroidJavaObject>("setFlags", kIntent_FLAG_ACTIVITY_NEW_TASK | kIntent_FLAG_ACTIVITY_CLEAR_TASK);
            currentActivity.Call("startActivity", intent);
            currentActivity.Call("finish");
            var process = new AndroidJavaClass("android.os.Process");
            int pid = process.CallStatic<int>("myPid");
            process.CallStatic("killProcess", pid);
        }
    }
#endif
}

/// <summary>
/// Enum of Levels being used for the sceneloader
/// </summary>
public class Levels
{
    public enum levels
    {
        //Scenes should be in order as the build settings.
        Main,
        Crossing
    }

}