using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToScene : MonoBehaviour
{
    public Levels.levels level;

    /// <summary>
    /// Change to certain scene
    /// </summary>
    /// <param name="coll"></param>
    private void OnTriggerEnter(Collider coll) 
    {
        if(coll.gameObject.CompareTag("Player"))
        {
            GameManager.SceneLoader(level);
        }
    
    }
}
