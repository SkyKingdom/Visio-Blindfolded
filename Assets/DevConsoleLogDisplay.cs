using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DevConsoleLogDisplay : MonoBehaviour
{

    TextMeshProUGUI consoleTxt;
    string timestamp = System.DateTime.Now.ToString("HH:mm:ss");

    private string logBuffer = "";

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello World");
        consoleTxt = GetComponent<TextMeshProUGUI>();
        Application.logMessageReceived += HandleLog;
    }
    private void Update()
    {
        //LogMessage("This is a custom log message.");
        Debug.LogError("Helllo there");
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Append the log message to your log buffer
        
        logBuffer += "[" + timestamp + "] " + logString + "\n";
        consoleTxt.text = logBuffer; // Update the text on your UI element
    }

    public void LogMessage(string message)
    {
        // Append the custom log message to the log buffer
        consoleTxt.text += message + "\n";
    }


}


