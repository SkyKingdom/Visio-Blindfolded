using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReference : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        GameManager.instance.player = null;
        GameManager.instance.player = gameObject;
        Debug.LogError(GameManager.instance.player.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
